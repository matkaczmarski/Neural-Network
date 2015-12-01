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

namespace SatelliteImageClassification
{
    

    public partial class mainForm : Form
    {
        //private MLPNetwork mlpNetwork;
        private string inputDataFileName;

        double[][] trainingData;
        const int DIGITSIZE = 20;
        const int ROWSPERDIGIT = 5;
        const int COLUMNS = 100;
        const int DIGITSCOUNT = 10;


        public mainForm()
        {
            InitializeComponent();
            inputDataFileName = string.Empty;
            numericUpDownIterationsCount.Maximum = Decimal.MaxValue;
            numericUpDownLearningRate.Maximum = Decimal.MaxValue;
            numericUpDownMomentum.Maximum = Decimal.MaxValue;

            chartErrors.Series["Błędy test."].Points.Clear();
            chartErrors.Series["Błędy walid."].Points.Clear();
        }

        private void buttonGetInputFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            openFileDialog.FilterIndex = 1;
            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                ImageConverter ic = new ImageConverter(openFileDialog.FileName);
                trainingData = ic.GetTrainingData();
            }
            openFileDialog.Dispose();
        }

        private void buttonTrain_Click(object sender, EventArgs e)
        {
            /*
            if (numericUpDownLayersCount.Value == 0 || numericUpDownNeuronsCount.Value == 0 || numericUpDownIterationsCount.Value == 0 || comboBoxActivationFunction.Text == string.Empty || comboBoxProblemType.Text == string.Empty)
            {
                MessageBox.Show("Brak zdefiniowanych wartości pól lub niedozwolone wartości pól.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if(inputDataFileName == string.Empty)
            {
                MessageBox.Show("Brak danych treningowych.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
             * */
            int iterationsCount = Decimal.ToInt32(numericUpDownIterationsCount.Value);
            double learningRate = Decimal.ToDouble(numericUpDownLearningRate.Value);
            double momentum = Decimal.ToDouble(numericUpDownMomentum.Value);

            int digitVectorSize = DIGITSIZE * DIGITSIZE;
            Autoencoder autoencoder = new Autoencoder(new List<int>() { digitVectorSize, digitVectorSize / 4, digitVectorSize/8, digitVectorSize / 4, digitVectorSize });
            //double[][] trainingSet = GenerateRandomTrainingSet(700, 100);
            double[] errors = autoencoder.learn(trainingData);

            
            chartErrors.Series["Błędy test."].Points.Clear();
            chartErrors.Series["Błędy walid."].Points.Clear();

            for (int i = 0; i < errors.Length; i++)
            {
                chartErrors.Series["Błędy test."].Points.AddXY(i + 1, errors[i]);
            }

            //mlpNetwork.SaveNetwork("network");
        }

        private double[][] GenerateRandomTrainingSet(int vectorSize, int setSize)
        {
            double[][] set= new double[setSize][];
            Random rand = new Random();
            for (int i = 0; i < setSize; i++)
            {
                set[i] = new double[vectorSize];
                double[] vector = new double[vectorSize];
                for (int j = 0; j < vectorSize; j++ )
                    vector[j] = rand.Next(2);
                vector.CopyTo(set[i], 0);
            }
            return set;
        }



        private void buttonTest_Click(object sender, EventArgs e)
        {

            
            //if(mlpNetwork == null)
            //{
            //    MessageBox.Show("Brak sieci.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
             //   return;
            //}

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV Files (.csv)|*.csv|All Files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //mlpNetwork.Test(openFileDialog.FileName, "result.csv", chartTestResults);
            }
        }

        private void buttonLoadNetwork_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All Files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
               // mlpNetwork = new MLPNetwork();
                try
                {
                 //   mlpNetwork.LoadNetwork(openFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                 //   mlpNetwork = null;
                }
            }
        }
    }
}
