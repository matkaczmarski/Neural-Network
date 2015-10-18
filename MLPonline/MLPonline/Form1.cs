using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Encog.Neural.Networks;
using Encog.Neural.Networks.Layers;
using Encog.Engine.Network.Activation;
using Encog.ML.Data;
using Encog.Neural.Networks.Training.Propagation.Resilient;
using Encog.ML.Train;
using Encog.ML.Data.Basic;
using Encog;
using Encog.Persist;
using System.IO;
using Encog.Neural.Networks.Training.Propagation.Back;
using Encog.Util.CSV;
using Encog.ML.Data.Versatile.Sources;

namespace MLPonline
{
    public partial class mainForm : Form
    {
        private string inputDataFileName;
        private int layersCount;
        private int neuronsCount; //each layer
        private int iterationsCount;
        private double learningRate;
        private double momentum;
        bool bipolar;

        private BasicNetwork network;


        public mainForm()
        {
            InitializeComponent();
            inputDataFileName = string.Empty;
            numericUpDownIterationsCount.Maximum = Decimal.MaxValue;
            numericUpDownLayersCount.Maximum = Decimal.MaxValue;
            numericUpDownLearningRate.Maximum = Decimal.MaxValue;
            numericUpDownNeuronsCount.Maximum = Decimal.MaxValue;
            numericUpDownMomentum.Maximum = Decimal.MaxValue;

            chartErrors.Series["Błędy"].Points.Clear();
        }

        private void buttonGetInputFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV Files (.csv)|*.csv|All Files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                inputDataFileName = openFileDialog.FileName;
            }
        }

        private void buttonTrain_Click(object sender, EventArgs e)
        {
            if (numericUpDownLayersCount.Value == 0 || numericUpDownNeuronsCount.Value == 0 || numericUpDownIterationsCount.Value == 0 || comboBoxActivationFunction.Text == string.Empty)
            {
                MessageBox.Show("Brak zdefiniowanych wartości pól lub niedozwolone wartości pól.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if(inputDataFileName == string.Empty)
            {
                MessageBox.Show("Brak danych treningowych.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            layersCount = Decimal.ToInt32(numericUpDownLayersCount.Value);
            neuronsCount = Decimal.ToInt32(numericUpDownNeuronsCount.Value);
            iterationsCount = Decimal.ToInt32(numericUpDownIterationsCount.Value);
            learningRate = Decimal.ToDouble(numericUpDownLearningRate.Value);
            momentum = Decimal.ToDouble(numericUpDownMomentum.Value);
            bipolar = comboBoxActivationFunction.Text == "bipolarna";

            double[][] input;
            double[][] ideal;
            GetInputAndIdeal(inputDataFileName, out input, out ideal);
            if(input.Length == 0)
                MessageBox.Show("Błąd wczytania danych wejściowych.");
            var data = new BasicMLDataSet(input, ideal);

            network = new BasicNetwork();
            network.AddLayer(new BasicLayer(null, checkBoxBias.Checked, input[0].Length));
            if (bipolar)
            {
                for (int i = 0; i < layersCount; i++)
                {
                    network.AddLayer(new BasicLayer(new ActivationTANH(), checkBoxBias.Checked, neuronsCount));
                }
                network.AddLayer(new BasicLayer(new ActivationTANH(), false, 1));
            }
            else
            {
                for (int i = 0; i < layersCount; i++)
                {
                    network.AddLayer(new BasicLayer(new ActivationSigmoid(), checkBoxBias.Checked, neuronsCount));
                }
                network.AddLayer(new BasicLayer(new ActivationSigmoid(), false, 1));
            }
            network.Structure.FinalizeStructure();
            network.Reset();

            IMLTrain train = new Backpropagation(network, data, learningRate, momentum);
            double[] errors = new double[iterationsCount];
            for(int i=0; i<iterationsCount; i++)
            {
                train.Iteration();
                errors[i] = train.Error;
            }
            for (int i = 0; i < iterationsCount; i++)
            {
                chartErrors.Series["Błędy"].Points.AddXY(i+1, errors[i]);
            }
            //zapis i wczytanie sieci
            EncogDirectoryPersistence.SaveObject(new FileInfo("network"), network);
            //network = (BasicNetwork)EncogDirectoryPersistence.LoadObject(new FileInfo("FILENAME"));            
        }

        private void GetInputAndIdeal(string fileName, out double[][] input, out double[][] ideal)
        {
            List<double[]> inputList = new List<double[]>();
            List<double[]> idealList = new List<double[]>();
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
                    for(int j=0; j<lineData.Length; j++)
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
                    idealList.Add(new double[] { b });
                }
            }
            input = inputList.ToArray();
            ideal = idealList.ToArray();
        }

        private void buttonTest_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV Files (.csv)|*.csv|All Files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //openFileDialog.FileName;
                //testowanie sieci network
                /*
                Console.WriteLine(@"Neural Network Results:");
                foreach (IMLDataPair pair in trainingSet)
                {
                    IMLData output = network.Compute(pair.Input);
                    Console.WriteLine(pair.Input[0] + @"," + pair.Input[1]
                                      + @", actual=" + output[0] + @",ideal=" + pair.Ideal[0]);
                }
                 * */
            }
        }
    }
}
