using Encog.Bot.Browse.Range;
using Encog.Engine.Network.Activation;
using Encog.ML.Data;
using Encog.ML.Data.Basic;
using Encog.ML.Train;
using Encog.Neural.Networks;
using Encog.Neural.Networks.Layers;
using Encog.Neural.Networks.Training.Propagation.Resilient;
using Encog.Persist;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SatelliteImageClassification
{
    public class AutoencoderWoCmp
    {
        const double EPS = 1e-6;
        const long AUTOENCODER_MAX_ITER = 1000;
        const long FINAL_NETWORK_MAX_ITER = 1500;
        List<int> layersConfiguration;

        BasicNetwork network;

        const bool BIAS = true;
        const double LEARNING_RATE = 0.1;
        const double MOMENTUM = 0.5;

        public System.Windows.Forms.Form ActiveForm { get; set; }

        private IActivationFunction CurrentActivationFunction()
        {
            return new ActivationTANH();
        }

        public AutoencoderWoCmp(List<int> layersConfig)
        {
            layersConfiguration = layersConfig;
        }


        public List<double[]> Learn(double[][] data, double[][] ideal)
        {
            double[][] origData = (double[][])data.Clone();
            int n = data.Length;
            int m = data[0].Length;
            double[][] output = new double[n][];
            double[][] sgmNeighbours = new double[n][];
            for (var i = 0; i < n; i++)
            {
                double[] sgmN = new double[SegmentationData.SEGMENT_NEIGHBOURS];
                Array.Copy(data[i], m - SegmentationData.SEGMENT_NEIGHBOURS, sgmN, 0, SegmentationData.SEGMENT_NEIGHBOURS);
                sgmNeighbours[i] = sgmN;
                data[i] = data[i].Take(m - SegmentationData.SEGMENT_NEIGHBOURS).ToArray();
                output[i] = new double[m - SegmentationData.SEGMENT_NEIGHBOURS];
                data[i].CopyTo(output[i], 0);
            }


            IMLDataSet trainingSet = new BasicMLDataSet(data, output);

            int inputLayerSize = layersConfiguration[0] - SegmentationData.SEGMENT_NEIGHBOURS;
            int trainingLayerSize = layersConfiguration[1];
            BasicNetwork oneLayerAutoencoder = new BasicNetwork();
            oneLayerAutoencoder.AddLayer(new BasicLayer(null, BIAS, inputLayerSize));
            oneLayerAutoencoder.AddLayer(new BasicLayer(CurrentActivationFunction(), BIAS, trainingLayerSize));
            oneLayerAutoencoder.AddLayer(new BasicLayer(CurrentActivationFunction(), false, inputLayerSize));
            oneLayerAutoencoder.Structure.FinalizeStructure();
            oneLayerAutoencoder.Reset();

            IMLTrain train = new ResilientPropagation(oneLayerAutoencoder, trainingSet);
            //IMLTrain train = new Backpropagation(oneLayerAutoencoder, trainingSet, LEARNING_RATE, MOMENTUM);

            int epoch = 1;
            List<double[]> errors = new List<double[]>();
            double[] trainError = new double[AUTOENCODER_MAX_ITER];
            
            do
            {
                train.Iteration();
                ActiveForm.Text = @"Epoch #" + epoch + @" Error:" + train.Error;
                Console.WriteLine(@"Epoch #" + epoch + @" Error:" + train.Error);
                trainError[epoch - 1] = train.Error;
                epoch++;
                //errors.Add(train.Error);
            } while (train.Error > EPS && epoch < AUTOENCODER_MAX_ITER);
            errors.Add(trainError);
            train.FinishTraining();
            
            BasicNetwork encoder = new BasicNetwork();
            encoder.AddLayer(new BasicLayer(null, BIAS, oneLayerAutoencoder.GetLayerNeuronCount(0)));
            encoder.AddLayer(new BasicLayer(CurrentActivationFunction(), false, oneLayerAutoencoder.GetLayerNeuronCount(1)));
            encoder.Structure.FinalizeStructure();
            encoder.Reset();

            //przypisanie wag do encodera
            for (int i = 0; i < encoder.LayerCount - 1; i++)
                for (int f = 0; f < encoder.GetLayerNeuronCount(i); f++)
                    for (int t = 0; t < encoder.GetLayerNeuronCount(i + 1); t++)
                        encoder.SetWeight(i, f, t, oneLayerAutoencoder.GetWeight(i, f, t));

            //Compare2Networks(oneLayerAutoencoder, encoder);

            for(int l=1; l<layersConfiguration.Count -2; l++)
            {
                inputLayerSize = layersConfiguration[l];
                trainingLayerSize = layersConfiguration[l+1];
                oneLayerAutoencoder = new BasicNetwork();
                oneLayerAutoencoder.AddLayer(new BasicLayer(null, BIAS, inputLayerSize));
                oneLayerAutoencoder.AddLayer(new BasicLayer(CurrentActivationFunction(), BIAS, trainingLayerSize));
                oneLayerAutoencoder.AddLayer(new BasicLayer(CurrentActivationFunction(), false, inputLayerSize));
                oneLayerAutoencoder.Structure.FinalizeStructure();
                oneLayerAutoencoder.Reset();

                //liczenie outputu z dotychczasowego encodera
                double[][] input = new double[n][];
                double[][] newOutput = new double[n][];
                for(int ni = 0; ni <n; ni++)
                {
                    IMLData res = encoder.Compute(new BasicMLData(data[ni]));
                    double[] resD = new double[res.Count];
                    for(int i=0; i<res.Count; i++)
                        resD[i] = res[i];
                    input[ni] = resD;
                    newOutput[ni] = new double[res.Count];
                    input[ni].CopyTo(newOutput[ni], 0);
                }

                BasicMLDataSet newTrainingSet = new BasicMLDataSet(input, newOutput);               
                train = new ResilientPropagation(oneLayerAutoencoder, newTrainingSet);
                //train = new Backpropagation(oneLayerAutoencoder, newTrainingSet, LEARNING_RATE, MOMENTUM);

                epoch = 1;
                trainError = new double[AUTOENCODER_MAX_ITER];
                do
                {
                    train.Iteration();
                    ActiveForm.Text = @"Epoch #" + epoch + @" Error:" + train.Error;
                    Console.WriteLine(@"Epoch #" + epoch + @" Error:" + train.Error);
                    trainError[epoch - 1] = train.Error;
                    epoch++;
                } while (train.Error > EPS && epoch < AUTOENCODER_MAX_ITER);
                errors.Add(trainError);
                train.FinishTraining();
                
                BasicNetwork extendedEncoder = new BasicNetwork();
                extendedEncoder.AddLayer(new BasicLayer(null, BIAS, encoder.GetLayerNeuronCount(0)));
                for (int el = 1; el < encoder.LayerCount; el++ )
                    extendedEncoder.AddLayer(new BasicLayer(CurrentActivationFunction(), BIAS, encoder.GetLayerNeuronCount(el)));
                extendedEncoder.AddLayer(new BasicLayer(CurrentActivationFunction(), false, oneLayerAutoencoder.GetLayerNeuronCount(1)));
                extendedEncoder.Structure.FinalizeStructure();
               

                //przypisanie wag do extendedencodera
                for (int i = 0; i < extendedEncoder.LayerCount - 1; i++)
                {
                    if (i < encoder.LayerCount-1)
                    {
                        for (int f = 0; f < extendedEncoder.GetLayerNeuronCount(i); f++)
                            for (int t = 0; t < extendedEncoder.GetLayerNeuronCount(i + 1); t++)
                                extendedEncoder.SetWeight(i, f, t, encoder.GetWeight(i, f, t));
                    }
                    else
                    {
                        for (int f = 0; f < extendedEncoder.GetLayerNeuronCount(i); f++)
                            for (int t = 0; t < extendedEncoder.GetLayerNeuronCount(i + 1); t++)
                                extendedEncoder.SetWeight(i, f, t, oneLayerAutoencoder.GetWeight(0, f, t));
                    }
                }
                encoder = extendedEncoder;

            }

            //tworzenie struktury ostatecznej sieci
            network = new BasicNetwork();
            network.AddLayer(new BasicLayer(null, BIAS, encoder.GetLayerNeuronCount(0) + SegmentationData.SEGMENT_NEIGHBOURS));
            for (int el = 1; el < encoder.LayerCount; el++)
                network.AddLayer(new BasicLayer(CurrentActivationFunction(), BIAS, encoder.GetLayerNeuronCount(el) + SegmentationData.SEGMENT_NEIGHBOURS));
            network.AddLayer(new BasicLayer(CurrentActivationFunction(), false, layersConfiguration[layersConfiguration.Count - 1]));
            network.Structure.FinalizeStructure();
            network.Reset();

            /*
            for (int i = 0; i < encoder.LayerCount - 1; i++)
                for (int f = 0; f < encoder.GetLayerNeuronCount(i); f++)
                    for (int t = 0; t < encoder.GetLayerNeuronCount(i + 1); t++)
                            network.SetWeight(i, f, t, encoder.GetWeight(i, f, t));
            */
            //dla innych ustawic wagi 0, dla samych sobie 1
            
            for (int i = 0; i < encoder.LayerCount - 1; i++)
                for (int f = 0; f < network.GetLayerNeuronCount(i); f++)
                    for (int t = 0; t < network.GetLayerNeuronCount(i + 1); t++)
                    {
                        if (f < encoder.GetLayerNeuronCount(i) && t >= encoder.GetLayerNeuronCount(i + 1))
                            network.SetWeight(i, f, t, 0);
                        else if (f >= encoder.GetLayerNeuronCount(i) && t < encoder.GetLayerNeuronCount(i + 1))
                            network.SetWeight(i, f, t, 0);
                        else if (f >= encoder.GetLayerNeuronCount(i) && t >= encoder.GetLayerNeuronCount(i + 1))
                            network.SetWeight(i, f, t, 1);
                        else
                            network.SetWeight(i, f, t, encoder.GetWeight(i, f, t));
                    }
            
            //uczenie koncowej sieci
            trainingSet = new BasicMLDataSet(origData, ideal);

            train = new ResilientPropagation(network, trainingSet);
            //train = new Backpropagation(network, trainingSet, LEARNING_RATE, MOMENTUM);

            epoch = 1;
            trainError = new double[FINAL_NETWORK_MAX_ITER];
            do
            {
                train.Iteration();
                ActiveForm.Text = @"Epoch #" + epoch + @" Error:" + train.Error;
                Console.WriteLine(@"Epoch #" + epoch + @" Error:" + train.Error);
                trainError[epoch - 1] = train.Error;
                epoch++;
                
            } while (train.Error > EPS && epoch < FINAL_NETWORK_MAX_ITER);
            errors.Add(trainError);
            train.FinishTraining();

            try
            {
                string networkFileName = "autoencoder wo cmp 300 125 50 3";
                EncogDirectoryPersistence.SaveObject(new FileInfo(networkFileName), network);
                MessageBox.Show("NETWORK SAVED TO FILE " + networkFileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return errors;
        }

        public double Compute(double[] val)
        {
            IMLData res = network.Compute(new BasicMLData(val));
            double[] resD = new double[res.Count];
            for (int i = 0; i < res.Count; i++)
                resD[i] = res[i];
            double maxValue = resD.Max();
            double maxIndex = resD.ToList().IndexOf(maxValue);
            return maxIndex;
        }

        public int[] ComputeMostPossible(double[] val, int possibleCount)
        {
            IMLData res = network.Compute(new BasicMLData(val));
            double[] resD = new double[res.Count];
            for (int i = 0; i < res.Count; i++)
                resD[i] = res[i];

            var sorted = resD.Select((x, i) => new KeyValuePair<double, int>(x, i)).OrderByDescending(x => x.Key).ToList();
            List<double> B = sorted.Select(x => x.Key).ToList();
            List<int> idx = sorted.Select(x => x.Value).ToList();

            return idx.Take(possibleCount).ToArray();
        }

        public void LoadNetwork(string fileName)
        {
            network = (BasicNetwork)EncogDirectoryPersistence.LoadObject(new FileInfo(fileName));
        }

        private void Compare2Networks(BasicNetwork n1, BasicNetwork n2)
        {
            string oneLay = string.Empty;
            for (int i = 0; i < n1.LayerCount - 1; i++)
            {
                oneLay += ("Layer: " + i + ": \n");
                for (int f = 0; f < n1.GetLayerNeuronCount(i); f++)
                {
                    oneLay += ("Neuron: " + f + "\n");
                    for (int t = 0; t < n1.GetLayerNeuronCount(i + 1); t++)
                    {
                        oneLay += (n1.GetWeight(i, f, t) + ", ");
                    }
                    oneLay += "\n";
                }
                oneLay += "\n";
            }
            oneLay += "---------------------------------------\n\n";
            for (int i = 0; i < n2.LayerCount - 1; i++)
            {
                oneLay += ("Layer: " + i + ": \n");
                for (int f = 0; f < n2.GetLayerNeuronCount(i); f++)
                {
                    oneLay += ("Neuron: " + f + "\n");
                    for (int t = 0; t < n2.GetLayerNeuronCount(i + 1); t++)
                    {
                        oneLay += (n2.GetWeight(i, f, t) + ", ");
                    }
                    oneLay += "\n";
                }
                oneLay += "\n";
            }
            MessageBox.Show(oneLay);
        }
    }
}
