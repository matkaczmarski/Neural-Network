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
    public class Autoencoder
    {
        const double eps = 1e-6;
        const long trainMaxIter = 30;
        //List<int> layersConfiguration;

        BasicNetwork network;

        public IMLDataSet trainingSet
        {
            get;
            set;
        }

        public Autoencoder(List<int> layersConfig)
        {
            //layersConfiguration = layersConfig;
            
            if (layersConfig.Count < 2)
                return;
            network = new BasicNetwork();
            network.AddLayer(new BasicLayer(null, true, layersConfig[0]));
            for(int i=1; i<layersConfig.Count; i++)
                network.AddLayer(new BasicLayer(new ActivationSigmoid(), true, layersConfig[i]));
            network.Structure.FinalizeStructure();
            network.Reset();
             
        }

        public void tralala()
        {
            BasicNetwork net = new BasicNetwork();
            net.AddLayer(new BasicLayer(null, true, 5));
            net.AddLayer(new BasicLayer(new ActivationSigmoid(), true, 3));
            net.AddLayer(new BasicLayer(new ActivationSigmoid(), true, 2));
            net.AddLayer(new BasicLayer(new ActivationSigmoid(), true, 3));
            net.AddLayer(new BasicLayer(new ActivationSigmoid(), true, 5));
            net.Structure.FinalizeStructure();
            net.Reset();

            string dw = net.DumpWeights();

            BasicNetwork enc = new BasicNetwork();

        }

        public static BasicNetwork getDecoder(BasicNetwork net)
        {
            var dec = new BasicNetwork();
            dec.AddLayer(new BasicLayer(null, true, net.GetLayerNeuronCount(1)));
            dec.AddLayer(new BasicLayer(new ActivationSigmoid(), true, net.GetLayerNeuronCount(2)));

            dec.Structure.FinalizeStructure();

            for(var i = 1; i < 2; i++)
            {
                int n = net.GetLayerNeuronCount(i);
                int m = net.GetLayerNeuronCount(i + 1);

                Console.WriteLine("Decoder: {0} - {1}", n, m);

                for(var j = 0; j < n; j++)
                {
                    for(var k = 0; k < m; k++)
                    {
                        dec.SetWeight(i - 1, j, k, net.GetWeight(i, j, k));
                    }
                }
            }

            return dec;
        }

        public static BasicNetwork getEncoder(BasicNetwork net)
        {
            var enc = new BasicNetwork();
            enc.AddLayer(new BasicLayer(null, true, net.GetLayerNeuronCount(0)));
            enc.AddLayer(new BasicLayer(new ActivationSigmoid(), true, net.GetLayerNeuronCount(1)));

            enc.Structure.FinalizeStructure();

            for(var i = 0; i < 1; i++)
            {
                int n = net.GetLayerNeuronCount(i);
                int m = net.GetLayerNeuronCount(i + 1);

                Console.WriteLine("Encoder: {0} - {1}", n, m);

                for(var j = 0; j < n; j++)
                {
                    for(var k = 0; k < m; k++)
                    {
                        enc.SetWeight(i, j, k, net.GetWeight(i, j, k));
                    }
                }
            }

            return enc;
        }

        public double[] learn(double[][] data)
        {
            int n = data.Length;
            int m = data[0].Length;
            double[][] output = new double[n][];
            for(var i = 0; i < n; i++)
            {
                output[i] = new double[m];
                data[i].CopyTo(output[i], 0);
            }

            



            trainingSet = new BasicMLDataSet(data, output);
            IMLTrain train = new ResilientPropagation(network, trainingSet);

            int epoch = 1;
            List<double> errors = new List<double>();
            do
            {
                train.Iteration();
                Console.WriteLine(@"Epoch #" + epoch + @" Error:" + train.Error);
                epoch++;
                errors.Add(train.Error);
            } while(train.Error > eps && epoch < trainMaxIter);

            train.FinishTraining();

            try
            {
                EncogDirectoryPersistence.SaveObject(new FileInfo("autoencoder2"), network);
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return errors.ToArray();
        }
    }
}
