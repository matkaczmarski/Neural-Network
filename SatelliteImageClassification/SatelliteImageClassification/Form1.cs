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

        //Autoencoder autoencoder;
        AutoencoderWoCmp autoencoder;

        TrainingData training;

        Bitmap originalImage;
        int iteration = 1;

        List<TestResult> testResults = new List<TestResult>();

        public mainForm()
        {
            InitializeComponent();
            inputDataFileName = string.Empty;
            chartErrors.Series["Layer1"].Points.Clear();
            chartErrors.Series["Layer2"].Points.Clear();
            chartErrors.Series["Layer3"].Points.Clear();
            chartErrors.Series["Layer4"].Points.Clear();

            /*string path = Directory.GetCurrentDirectory() + "\\";
            SegmentationData.MAX_SEGMENT_SIZE = 10;
            training = SegmentationData.GetTrainingData(path + "originals", path + "segments", path + "buildings", out originalImage, iteration);
            trainingData = training.Vectors;
            idealData = training.Ideal;
            this.pictureBox1.Image = originalImage = training.OriginalImage;*/
            
            /*ImageForm imageForm = new ImageForm(training.OriginalImage);
            imageForm.Show();
            List<Bitmap> bitmaps = new List<Bitmap>();
            for (int i = 0; i < training.Vectors.Length; i++)
                if (idealData[i][0] == 1)
                    bitmaps.Add(SegmentationData.GetBitmapFromDoubleArray(training.Vectors[i]));
            SegmentsViewer segmentsViewer = new SegmentsViewer(bitmaps);
            segmentsViewer.ShowDialog();*/

            /*for (int i = 0; i < training.Segments.Length; i++)
            {
                for (int x = 0; x < training.Segments[i].Width; x++)
                {
                    for (int y = 0; y < training.Segments[i].Height; y++)
                    {
                        if (training.Segments[i].GetPixel(x,y).ToArgb() != Color.White.ToArgb())
                            originalImage.SetPixel(x + training.Positions[i].X, y + training.Positions[i].Y, training.Segments[i].GetPixel(x,y));
                    }
                }
            }
            ImageForm imageForm = new ImageForm(originalImage);
            imageForm.ShowDialog();*/

            /*for (int i = 0; i < training.Vectors.Length; i++)
            {
                for (int j = 0; j < training.Vectors[i].Length - 4; j++)
                {
                    if (training.Vectors[i][j] > 0)
                        training.Vectors[i][j] /= 255;
                }
            }*/
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
            //autoencoder = new Autoencoder(new List<int>() { digitVectorSize, 100, 50, 10 });//new List<int>() { digitVectorSize, 100 });
            //autoencoder = new Autoencoder(new List<int>() { SegmentationData.MAX_SEGMENT_SIZE * SegmentationData.MAX_SEGMENT_SIZE * 3 + 4, 125, 50, 3 });
            autoencoder = new AutoencoderWoCmp(new List<int>() { SegmentationData.MAX_SEGMENT_SIZE * SegmentationData.MAX_SEGMENT_SIZE * 3 + SegmentationData.SEGMENT_NEIGHBOURS, 100, 50, 3 });
            
            autoencoder.ActiveForm = this;
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

        private void GenerateData()
        {
            string path = Directory.GetCurrentDirectory() + "\\";
            SegmentationData.MAX_SEGMENT_SIZE = 6;
            training = SegmentationData.GetTrainingData(path + "originals", path + "segments", path + "buildings", out originalImage, iteration++);
            trainingData = training.Vectors;
            idealData = training.Ideal;
            this.pictureBox1.Image = originalImage = training.OriginalImage;

            /*ImageForm imageForm = new ImageForm(training.OriginalImage);
            imageForm.Show();*/
            /*List<Bitmap> bitmaps = new List<Bitmap>();
            for (int i = 0; i < training.Vectors.Length; i++)
                if (idealData[i][0] == 1)
                    bitmaps.Add(SegmentationData.GetBitmapFromDoubleArray(training.Vectors[i]));
            SegmentsViewer segmentsViewer = new SegmentsViewer(bitmaps);
            segmentsViewer.ShowDialog();*/

            /*for (int i = 0; i < training.Segments.Length; i++)
            {
                for (int x = 0; x < training.Segments[i].Width; x++)
                {
                    for (int y = 0; y < training.Segments[i].Height; y++)
                    {
                        if (training.Segments[i].GetPixel(x,y).ToArgb() != Color.White.ToArgb())
                            originalImage.SetPixel(x + training.Positions[i].X, y + training.Positions[i].Y, training.Segments[i].GetPixel(x,y));
                    }
                }
            }
            ImageForm imageForm = new ImageForm(originalImage);
            imageForm.ShowDialog();*/

            for (int i = 0; i < training.Vectors.Length; i++)
            {
                for (int j = 0; j < training.Vectors[i].Length - 4; j++)
                {
                    if (training.Vectors[i][j] > 0)
                        training.Vectors[i][j] /= 255;
                }
            }
        }

        private void buttonTest_Click(object sender, EventArgs e)
        {
            GenerateData();
            int testSize = training.Segments.Length;
            double[][] testSet = new double[testSize][];
            int[] testResult = new int[testSize];
            for (int i = 0; i < testSize; i++)
            {
                testSet[i] = trainingData[i];
                for (int j = 0; j < idealData[i].Length; j++)
                    if (idealData[i][j] == 1)
                        testResult[i] = j;
            }
            int wrongAnswers = 0;

            Bitmap resultBitmap = new Bitmap(training.OriginalImage.Width, training.OriginalImage.Height);
            for (int x = 0; x < resultBitmap.Width; x++)
                for (int y = 0; y < resultBitmap.Height; y++)
                    resultBitmap.SetPixel(x, y, Color.White);

            for (int i = 0; i < testSet.Length; i++)
            {
                double[,][] segment = SegmentationData.CovertFrom1DArray(trainingData[i]);
                int computedVal = (int)autoencoder.Compute(testSet[i]);
                if (computedVal == 0)
                {
                    for (int x = 0; x < training.Segments[i].Width; x++)
                    {
                        for (int y = 0; y < training.Segments[i].Height; y++)
                        {
                            if (segment[x, y][0] >= 0)
                            {
                                resultBitmap.SetPixel(x + training.Positions[i].X, y + training.Positions[i].Y, Color.Red);
                                originalImage.SetPixel(x + training.Positions[i].X, y + training.Positions[i].Y, Color.FromArgb(50, 255, 255, 255));
                            }
                        }
                    } 
                }
                if (computedVal != testResult[i])
                {
                    wrongAnswers++;
                }
                /*
                if (computedVal == testResult[i])
                {
                    for (int x = 0; x < training.Segments[i].Width; x++)
                    {
                        for (int y = 0; y < training.Segments[i].Height; y++)
                        {
                            originalImage.SetPixel(x + training.Positions[i].X, y + training.Positions[i].Y, Color.FromArgb(50, 255, 255, 255));
                        }
                    }                
                }
                 * */
            }
            this.pictureBox1.Image = originalImage;
            
            TestResult res = TestResultBitmap(resultBitmap, training.SegmentsImage);
            /*ImageForm if1 = new ImageForm(resultBitmap);
            if1.Text = "Result";
            if1.Show();

            ImageForm if2 = new ImageForm(training.SegmentsImage);
            if2.Text = "Ideal";
            if2.Show();*/

            testResults.Add(res);
            button1.Text = "Zapisz " + (iteration - 1) + "/9";

            //MessageBox.Show("\n LICZBA BŁĘDNYCH PIXELI: " + res.WrongPixels + "\n LICZBA POPRAWNYCH PIXELI: " + res.CorrectPixels + "\n PROCENT POPRAWNYCH PIXELI: " + res.Percentage + "\n LICZBA PIXELI BUDYNKÓW: " + res.BuildingPixels + "\n LICZBA BŁĘDNYCH PIXELI BUDYNKÓW: " + res.WrongBuildingPixels + "\n LICZBA PIXELI TERENU: " + res.TerrainPixels + "\n LCIZBA BŁĘDNYCH PIXELI TERENU: " + res.WrongTerrainPixels + "\n TP BUILDINGS: " + res.TruePositiveBuildings + "\n TP TERRAIN: " + res.TruePositiveTerrain);
        }

        private TestResult TestResultBitmap(Bitmap result, Bitmap ideal)
        {
            TestResult testResult = new TestResult();

            if (result.Width != ideal.Width || result.Height != ideal.Height)
                throw new Exception();

            for (int x = 0; x < result.Width; x++)
                for (int y = 0; y < result.Height; y++)
                {
                    if (result.GetPixel(x,y).ToArgb() != Color.White.ToArgb())
                    {
                        testResult.BuildingPixels++;
                        if (ideal.GetPixel(x, y).ToArgb() != Color.White.ToArgb())
                        {
                            testResult.TP++;
                            testResult.CorrectPixels++;
                        }
                        else
                        {
                            testResult.FP++;
                            testResult.WrongPixels++;
                            testResult.WrongBuildingPixels++;
                        }
                    }
                    else
                    {
                        testResult.TerrainPixels++;
                        if (ideal.GetPixel(x, y).ToArgb() != Color.White.ToArgb())
                        {
                            testResult.TN++;
                            testResult.WrongPixels++;
                            testResult.WrongTerrainPixels++;
                        }
                        else
                            testResult.CorrectPixels++;
                    }
                }

            return testResult;
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
                    //autoencoder = new Autoencoder(null);
                    autoencoder = new AutoencoderWoCmp(null);
                    autoencoder.LoadNetwork(openFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string path = @"C:\Users\Kuba\Desktop\wyniki\TestResults.txt";
            if (!File.Exists(path))
            {
                FileStream fs = File.Create(path);
                TextWriter tw = new StreamWriter(fs);
                tw.WriteLine("RESULTS:");

                int i = 1;
                foreach (TestResult ts in testResults)
                {
                    tw.WriteLine();
                    tw.WriteLine("#" + i);
                    tw.WriteLine();
                    tw.WriteLine("TP = " + ts.TP);
                    tw.WriteLine("TN = " + ts.TN);
                    tw.WriteLine("FP = " + ts.FP);
                    tw.WriteLine("DP = " + ts.DP.ToString("0.00"));
                    tw.WriteLine("BF = " + ts.BF.ToString("0.00"));
                    tw.WriteLine();

                    i++;
                }

                tw.Close();
            }
        }
    }
}
