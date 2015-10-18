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
        private MLPNetwork mlpNetwork;
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

            chartErrors.Series["Błędy test."].Points.Clear();
            chartErrors.Series["Błędy walid."].Points.Clear();
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

            mlpNetwork = new MLPNetwork(layersCount, neuronsCount, checkBoxBias.Checked,comboBoxActivationFunction.Text == "bipolarna" ? ActivationFunctionType.Bipolar : ActivationFunctionType.Unipolar, inputDataFileName );

            double[][] errors = mlpNetwork.Train(iterationsCount, learningRate, momentum);

            chartErrors.Series["Błędy test."].Points.Clear();
            chartErrors.Series["Błędy walid."].Points.Clear();

            for (int i = 0; i < errors[0].Length; i++)
            {
                chartErrors.Series["Błędy test."].Points.AddXY(i + 1, errors[0][i]);
                chartErrors.Series["Błędy walid."].Points.AddXY(i + 1, errors[1][i]);
            }

            mlpNetwork.SaveNetwork("network");
           
        }



        private void buttonTest_Click(object sender, EventArgs e)
        {
            if(mlpNetwork == null)
            {
                MessageBox.Show("Brak sieci.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV Files (.csv)|*.csv|All Files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                mlpNetwork.Test(openFileDialog.FileName, "result.csv");
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
                mlpNetwork = new MLPNetwork();
                try
                {
                    mlpNetwork.LoadNetwork(openFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    mlpNetwork = null;
                }
            }
        }
    }
}
