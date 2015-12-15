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
        private string inputDataFileName;

        double[][] trainingData;
        double[][] idealData;
        const int DIGITSIZE = 20;
        const int ROWSPERDIGIT = 5;
        const int COLUMNS = 100;
        const int DIGITSCOUNT = 10;

        const int TEST_SIZE = 20;

        Autoencoder autoencoder;


        public mainForm()
        {
            InitializeComponent();
            inputDataFileName = string.Empty;
            chartErrors.Series["Layer1"].Points.Clear();
            chartErrors.Series["Layer2"].Points.Clear();
            chartErrors.Series["Layer3"].Points.Clear();
            chartErrors.Series["Layer4"].Points.Clear();
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
                trainingData = ic.GetTrainingData(out idealData);
                MessageBox.Show("TRAINING DATA LOADED");
            }
            openFileDialog.Dispose();
        }

        private void buttonTrain_Click(object sender, EventArgs e)
        {

            int digitVectorSize = DIGITSIZE * DIGITSIZE;
            autoencoder = new Autoencoder(new List<int>() { digitVectorSize, 100, 50, 10 });//new List<int>() { digitVectorSize, 100 });
            List<double[]> errors = autoencoder.Learn(trainingData, idealData);
            
            chartErrors.Series["Layer1"].Points.Clear();
            chartErrors.Series["Layer2"].Points.Clear();
            chartErrors.Series["Layer3"].Points.Clear();
            chartErrors.Series["Layer4"].Points.Clear();
            
            for (int i = 0; i < errors[2].Length; i++)
            {   
                if(i < errors[0].Length)
                    chartErrors.Series["Layer1"].Points.AddXY(i + 1, errors[0][i]);
                if (i < errors[1].Length)
                    chartErrors.Series["Layer2"].Points.AddXY(i + 1, errors[1][i]);
                chartErrors.Series["Layer3"].Points.AddXY(i + 1, errors[2][i]);
            }
            
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
            double[][] testSet = new double[TEST_SIZE][];
            int[] testResult = new int[TEST_SIZE];
            Random rand = new Random();
            for (int i = 0; i < TEST_SIZE; i++)
            {
                int randInd = rand.Next(trainingData.Length);
                testSet[i] = trainingData[randInd];
                for (int j = 0; j < idealData[randInd].Length; j++)
                    if (idealData[randInd][j] == 1)
                        testResult[i] = j;
            }
            string debugg = string.Empty;
            int wrongAnswers = 0;
            for (int i = 0; i < testSet.Length; i++ )
            {
                //double computedVal = autoencoder.Compute(testSet[i]);
                int[] possibleVals = autoencoder.ComputeMostPossible(testSet[i], 3);
                if (possibleVals[0] == testResult[i])
                    debugg += "JEST: " + string.Join(", ", possibleVals) + ", MIAŁO BYC: " + testResult[i] + ";  ";
                else
                {
                    wrongAnswers++;
                    debugg += "##JEST: " + string.Join(", ", possibleVals) + ", MIAŁO BYC: " + testResult[i] + ";  ##";
                }
                //debugg += "JEST: " + computedVal + ", MIAŁO BYC: " + testResult[i] + ";  ";
                
                //if (i % 10 == 0)
                debugg += "\n";
            }
            MessageBox.Show(debugg + "\n LICZBA BŁĘDNYCH:" + wrongAnswers);

                return;
            /*
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV Files (.csv)|*.csv|All Files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //mlpNetwork.Test(openFileDialog.FileName, "result.csv", chartTestResults);
            }
             */
        }

        private void buttonLoadNetwork_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All Files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    autoencoder = new Autoencoder(null);
                    autoencoder.LoadNetwork(openFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
