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
            int layersCount = Decimal.ToInt32(numericUpDownLayersCount.Value);
            int neuronsCount = Decimal.ToInt32(numericUpDownNeuronsCount.Value);
            int iterationsCount = Decimal.ToInt32(numericUpDownIterationsCount.Value);
            double learningRate = Decimal.ToDouble(numericUpDownLearningRate.Value);
            double momentum = Decimal.ToDouble(numericUpDownMomentum.Value);


            MLPNetwork mlpNetwork = new MLPNetwork(layersCount, neuronsCount, checkBoxBias.Checked,comboBoxActivationFunction.Text == "bipolarna" ? ActivationFunctionType.Bipolar : ActivationFunctionType.Unipolar, inputDataFileName );

            double[] errors = mlpNetwork.Train(iterationsCount, learningRate, momentum);
            for (int i = 0; i < errors.Length; i++)
            {
                chartErrors.Series["Błędy"].Points.AddXY(i+1, errors[i]);
            }
            //zapis i wczytanie sieci
           // EncogDirectoryPersistence.SaveObject(new FileInfo("network"), network);
            //network = (BasicNetwork)EncogDirectoryPersistence.LoadObject(new FileInfo("FILENAME"));
            

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
