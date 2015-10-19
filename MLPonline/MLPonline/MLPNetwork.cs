using Encog.Engine.Network.Activation;
using Encog.ML.Data;
using Encog.ML.Data.Basic;
using Encog.ML.Train;
using Encog.Neural.Networks;
using Encog.Neural.Networks.Layers;
using Encog.Neural.Networks.Training.Propagation.Back;
using Encog.Persist;
using Encog.Util.CSV;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;

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
        private ProblemType problemType;

        private int outputSize;

        protected IMLDataSet trainingData;
        protected IMLDataSet validationData;
        protected List<IMLData> testData;

        private BasicNetwork network;

        private double[] idealMin;
        private double[] idealMax;
        private double normParam = 1.0;


        private IActivationFunction CreateActivationFunction()
        {
            switch (activationFunType)
            {
                case ActivationFunctionType.Bipolar:
                    return new ActivationTANH(); //ActivationBipolarSteepenedSigmoid();
                case ActivationFunctionType.Unipolar:
                    return new ActivationSigmoid();
                default:
                    return null;
            }
        }

        public MLPNetwork()
        { }

        public MLPNetwork(int layersCount, int neuronsCount, bool bias, ActivationFunctionType aft, ProblemType problemType, string inputFileName)
        {
            this.layersCount = layersCount;
            this.neuronsCount = neuronsCount;
            this.bias = bias;
            this.activationFunType = aft;
            this.problemType = problemType;

            LoadTrainingData(inputFileName);

            network = new BasicNetwork();
            network.AddLayer(new BasicLayer(null, bias, trainingData.InputSize));
            for (int i = 0; i < layersCount; i++)
                network.AddLayer(new BasicLayer(CreateActivationFunction(), bias, neuronsCount));
            network.AddLayer(new BasicLayer(CreateActivationFunction(), false, outputSize));
            network.Structure.FinalizeStructure();
            network.Reset();
        }

        public double[][] Train(int iterationsCount, double learningRate, double momentum)
        {
            var train = new Backpropagation(network, trainingData, learningRate, momentum);
            train.BatchSize = 1;
            double[][] errors = new double[2][];
            errors[0] = new double[iterationsCount];
            errors[1] = new double[iterationsCount];
            for (int i = 0; i < iterationsCount; i++)
            {
                train.Iteration(1);
                errors[0][i] = train.Error;
                errors[1][i] = network.CalculateError(validationData);
            }
            train.FinishTraining();
            return errors;
        }

        public void SaveNetwork(string fileName)
        {
            //EncogDirectoryPersistence.SaveObject(new FileInfo(fileName), network);
        }

        public void LoadNetwork(string fileName)
        {
            network = (BasicNetwork)EncogDirectoryPersistence.LoadObject(new FileInfo(fileName));
        }


        private void LoadTrainingData(string fileName)
        {
            List<double[]> inputList = new List<double[]>();
            List<double[]> idealListBeforeTransformation = new List<double[]>();
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
                    double b;
                    if (!double.TryParse(splittedLine[splittedLine.Length - 1], out b))
                        MessageBox.Show("Error parsing line: " + lineNumber);
                    idealListBeforeTransformation.Add(new double[] { b });
                }
            }

            //zmieniamy ideal na poprawna wersje dla KLASYFIKACJI
            List<double[]> ideal = new List<double[]>();
            if (problemType == ProblemType.Classification)
            {
                double min_val = (activationFunType == ActivationFunctionType.Bipolar) ? -1.0 : 0.0;
                List<int> idealInts = new List<int>();
                foreach (double[] dLine in idealListBeforeTransformation)
                    idealInts.Add((int)dLine[0]);

                HashSet<int> uniqueHashSet = new HashSet<int>(idealInts);
                int[] unique = uniqueHashSet.ToArray();
                outputSize = unique.Length;

                foreach (int val in idealInts)
                {
                    double[] idealElement = new double[unique.Length];
                    for (int i = 0; i < unique.Length; i++)
                        idealElement[i] = unique[i] == val ? 1.0 : min_val;
                    ideal.Add(idealElement);
                }
            }

            Normalize(ref inputList);

            if (problemType == ProblemType.Regression) // dla regresji
            {
                ideal = idealListBeforeTransformation;
                outputSize = 1;
                NormalizeIdeal(ref ideal);
            }

            Randomize(ref inputList, ref ideal);

            int validation_size = inputList.Count / 10;

            validationData = new BasicMLDataSet(inputList.Take(validation_size).ToArray(), ideal.Take(validation_size).ToArray());
            trainingData = new BasicMLDataSet(inputList.Skip(validation_size).ToArray(), ideal.Skip(validation_size).ToArray());
        }

        private List<double[]> LoadTestData(string testFile)
        {
            List<double[]> testValues = new List<double[]>();
            List<double[]> originalTestValues = new List<double[]>();

            using (StreamReader sr = new StreamReader(testFile))
            {
                string headerLine = sr.ReadLine();
                int lineNumber = 1;
                string line = string.Empty;
                while ((line = sr.ReadLine()) != null)
                {
                    lineNumber++;
                    var splittedLine = line.Split(',');
                    double[] lineData = new double[splittedLine.Length];
                    for (int j = 0; j < lineData.Length; j++)
                    {
                        double a;
                        if (!double.TryParse(splittedLine[j], out a))
                            MessageBox.Show("Error parsing line: " + lineNumber);
                        lineData[j] = a;
                    }
                    testValues.Add(lineData);
                    originalTestValues.Add((double[])lineData.Clone());
                }
            }

            Normalize(ref testValues);

            testData = new List<IMLData>();
            foreach (var val in testValues)
                testData.Add(new BasicMLData(val));
            return originalTestValues;
        }

        public void Test(string testSetFile, string outputFile, Chart resultsChart)
        {
            List<double[]> originalTestValues = LoadTestData(testSetFile);

            string resultString = string.Empty;
            //klasyfikacja
            if (problemType == ProblemType.Classification)
            {
                resultsChart.Visible = true;
                List<Color> colors = new List<Color>() { Color.Red, Color.Green, Color.Blue, Color.Yellow, Color.Violet, Color.Orange, Color.Beige, Color.Brown };
                for (int i = 0; i < outputSize; i++)
                {
                    var serie = new System.Windows.Forms.DataVisualization.Charting.Series
                    {
                        Name = ("klasa " + (i + 1)),
                        Color = colors[i],
                        ChartType = SeriesChartType.FastPoint
                    };

                    resultsChart.Series.Add(serie);
                }

                for (int j = 0; j < testData.Count; j++)
                {
                    var d = network.Compute(testData[j]);
                    int cls = 1;
                    for (int i = 1; i < outputSize; i++)
                    {
                        if (d[i] > d[i - 1])
                            cls++;
                    }
                    resultsChart.Series["klasa " + cls].Points.AddXY(originalTestValues[j][0], originalTestValues[j][1]);
                    resultsChart.Invalidate();
                    foreach (double val in originalTestValues[j])
                        resultString += (val + ",");
                    resultString += (cls + Environment.NewLine);
                }
            }
            else //regresja
            {
                resultsChart.Visible = true;

                var serie = new System.Windows.Forms.DataVisualization.Charting.Series
                {
                    Name = ("regresja"),
                    Color = Color.Blue,
                    ChartType = SeriesChartType.FastPoint
                };

                resultsChart.Series.Add(serie);

                double minVal = (activationFunType == ActivationFunctionType.Bipolar) ? -1.0 : 0.0;
                double maxVal = 1.0;
                double normSize = (maxVal - minVal) * normParam;

                for (int j = 0; j < testData.Count; j++)
                {
                    var res = network.Compute(testData[j])[0];
                    //denormalizacja
                    double dSize = idealMax[0] - idealMin[0];
                    res = idealMin[0] + ((res - minVal) * dSize / normSize);
                    resultsChart.Series["regresja"].Points.AddXY(originalTestValues[j][0], res);
                    resultsChart.Invalidate();
                    foreach (double val in originalTestValues[j])
                        resultString += (val + ",");
                    resultString += (res + Environment.NewLine);
                }
            }

            using (StreamWriter outfile = new StreamWriter(outputFile))
            {
                outfile.Write(resultString);
            }
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

        private void NormalizeIdeal(ref List<double[]> data)
        {
            idealMin = new double[data[0].Length];
            idealMax = new double[data[0].Length];
            for (int i = 0; i < data[0].Length; i++)
            {
                idealMin[i] = double.PositiveInfinity;
                idealMax[i] = double.NegativeInfinity;
            }

            foreach (double[] item in data)
            {
                for (int i = 0; i < item.Length; i++)
                {
                    if (item[i] < idealMin[i])
                        idealMin[i] = item[i];

                    if (item[i] > idealMax[i])
                        idealMax[i] = item[i];
                }
            }

            double minVal = (activationFunType == ActivationFunctionType.Bipolar) ? -1.0 : 0.0;
            double maxVal = 1.0;
            double normSize = (maxVal - minVal) * normParam;

            for (int i = 0; i < data[0].Length; i++)
            {
                double dSize = idealMax[i] - idealMin[i];
                foreach (double[] elem in data)
                    elem[i] = ((elem[i] - idealMin[i]) * normSize / dSize) + minVal;
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
