using Encog.Engine.Network.Activation;
using Encog.ML.Data;
using Encog.ML.Data.Basic;
using Encog.ML.Train;
using Encog.Neural.Networks;
using Encog.Neural.Networks.Layers;
using Encog.Neural.Networks.Training.Propagation.Back;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MLPonline
{

    public enum ActivationFunctionType
    {
        Unipolar,
        Bipolar
    }

    public class MLPNetwork
    {
        private int layersCount;
        private int neuronsCount; //each layer
        private bool bias;
        private ActivationFunctionType activationFunType;

        private int outputSize;

        protected IMLDataSet trainingData;
        protected IMLDataSet validationData;
        protected List<IMLData> testData;

        private BasicNetwork network;

        private IActivationFunction CreateActivationFunction()
        {
            switch (activationFunType)
            {
                case ActivationFunctionType.Bipolar:
                    return new ActivationBipolarSteepenedSigmoid();
                case ActivationFunctionType.Unipolar:
                    return new ActivationSigmoid();
                default:
                    return null;
            }
        }

        public MLPNetwork(int layersCount, int neuronsCount, bool bias, ActivationFunctionType aft, string inputFileName)
        {
            this.layersCount = layersCount;
            this.neuronsCount = neuronsCount;
            this.bias = bias;
            this.activationFunType = aft;

            GetTrainingData(inputFileName);

            network = new BasicNetwork();
            network.AddLayer(new BasicLayer(null, bias, trainingData.InputSize));
            for (int i = 0; i < layersCount; i++)
                network.AddLayer(new BasicLayer(CreateActivationFunction(), bias, neuronsCount));
            network.AddLayer(new BasicLayer(CreateActivationFunction(), false, outputSize));
            network.Structure.FinalizeStructure();
            network.Reset();
        }

        public double[] Train(int iterationsCount,  double learningRate, double momentum)
        {
            var train = new Backpropagation(network, trainingData, learningRate, momentum);
            train.BatchSize = 1;
            double[] errors = new double[iterationsCount];
            for (int i = 0; i < iterationsCount; i++)
            {
                train.Iteration(1);
                errors[i] = train.Error;// network.CalculateError(validationData); //train.Error;
            }
            train.FinishTraining();
            return errors;
        }


        private void GetTrainingData(string fileName)
        {
            List<double[]> inputList = new List<double[]>();
            List<int> idealListBeforeTransformation = new List<int>();
            using (StreamReader sr = new StreamReader(fileName))
            {
                string headerLine = sr.ReadLine();
                int lineNumber = 1;
                string line = string.Empty;
                while ((line = sr.ReadLine()) != null)
                {
                    lineNumber++;
                    var splittedLine = line.Split(',');
                    double[] lineData = new double[splittedLine.Length - 1];
                    for (int j = 0; j < lineData.Length; j++)
                    {
                        double a;
                        if (!double.TryParse(splittedLine[j], out a))
                            MessageBox.Show("Error parsing line: " + lineNumber);
                        lineData[j] = a;
                    }
                    inputList.Add(lineData);
                    int b;
                    if (!int.TryParse(splittedLine[splittedLine.Length - 1], out b))
                        MessageBox.Show("Error parsing line: " + lineNumber);
                    idealListBeforeTransformation.Add(b);
                }
            }

            //zmieniamy ideal na poprawna wersje
            List<double[]> ideal = new List<double[]>();
            double min_val = (activationFunType == ActivationFunctionType.Bipolar) ? -1.0 : 0.0;

            HashSet<int> uniqueHashSet = new HashSet<int>(idealListBeforeTransformation);
            int[] unique = uniqueHashSet.ToArray();
            outputSize = unique.Length;

            foreach (int val in idealListBeforeTransformation)
            {
                double[] idealElement = new double[unique.Length];
                for (int i = 0; i < unique.Length; i++)
                    idealElement[i] = unique[i] == val ? 1.0 : min_val;
                ideal.Add(idealElement);
            }
            Normalize(ref inputList);
            Randomize(ref inputList, ref ideal);

            int validation_size = inputList.Count / 10;

            validationData = new BasicMLDataSet(inputList.Take(validation_size).ToArray(), ideal.Take(validation_size).ToArray());
            trainingData = new BasicMLDataSet(inputList.Skip(validation_size).ToArray(), ideal.Skip(validation_size).ToArray());

           // trainingData = new BasicMLDataSet(inputList.ToArray(), ideal.ToArray());
        }


        private void Normalize(ref List<double[]> data)
        {
            double[] minVals = new double[data[0].Length];
            double[] maxVals = new double[data[0].Length];
            for (int i = 0; i < data[0].Length; i++)
            {
                minVals[i] = double.PositiveInfinity;
                maxVals[i] = double.NegativeInfinity;
            }

            foreach (double[] item in data)
            {
                for (int i = 0; i < item.Length; i++)
                {
                    if (item[i] < minVals[i])
                        minVals[i] = item[i];

                    if (item[i] > maxVals[i])
                        maxVals[i] = item[i];
                }
            }

            double minVal = (activationFunType == ActivationFunctionType.Bipolar) ? -1.0 : 0.0;
            double maxVal = 1.0;
            double normSize = maxVal - minVal;

            for (int i = 0; i < data[0].Length; i++)
            {
                double dSize = maxVals[i] - minVals[i];
                foreach (double[] elem in data)
                    elem[i] = ((elem[i] - minVals[i]) * normSize / dSize) + minVal;
            }
        }

        protected void Randomize(ref List<double[]> input, ref List<double[]> ideal)
        {
            Random r = new Random();

            List<double[]> new_input = new List<double[]>();
            List<double[]> new_ideal = new List<double[]>();

            while (input.Count > 0)
            {
                int n = r.Next(input.Count);

                new_ideal.Add(ideal[n]);
                ideal.RemoveAt(n);

                new_input.Add(input[n]);
                input.RemoveAt(n);
            }

            input = new_input;
            ideal = new_ideal;
        }


    }
}
