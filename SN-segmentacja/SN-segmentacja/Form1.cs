using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SN_segmentacja
{
    public partial class Form1 : Form
    {
        //distance in px
        int seedDistance = 10;

        double maxColorDistance = 10;

        Random rand = new Random();
        Bitmap baseImage = null;
        int[,] generatedSegments;

        int segmentsCounter = 0;

        List<Point> seedPoints = new List<Point>();
        CheckKind checkKind = CheckKind.Star;

        public Form1()
        {
            InitializeComponent();
        }

        public void InitializeParameters()
        {
            seedDistance = (int)seedDistanceNumeric.Value;
            maxColorDistance = (double)maxColorDistanceNumeric.Value;
        }

        public List<Point> GenerateSeedPoints(int width, int height, int seedDistance)
        {
            List<Point> seedPoints = new List<Point>();

            int startX = seedDistance / 2;
            int startY = seedDistance / 2;

            int tempX = startX;
            int tempY = startY;

            while (tempY < height)
            {
                if (tempX >= width)
                {
                    tempX = startX;
                    tempY += seedDistance;
                    if (tempY >= height)
                        break;
                }
                seedPoints.Add(new Point(tempX, tempY));
                tempX += seedDistance;
            }

            return seedPoints;
        }

        public void GeneratePicture()
        {
            InitializeParameters();
            int width = baseImage.Width;
            int height = baseImage.Height;
            generatedSegments = new int[width, height];
            seedPoints = GenerateSeedPoints(width, height, seedDistance);

            while (seedPoints.Count > 0)
            {
                Point p = seedPoints[seedPoints.Count - 1];
                seedPoints.RemoveAt(seedPoints.Count - 1);

                Color color = baseImage.GetPixel(p.X, p.Y);
                if (generatedSegments[p.X, p.Y] == 0)
                    generatedSegments[p.X, p.Y] = ++segmentsCounter;

                if (IsPointCorrect(p.X - 1, p.Y, width, height, maxColorDistance, color))
                {
                    seedPoints.Add(new Point(p.X - 1, p.Y));
                    generatedSegments[p.X - 1, p.Y] = generatedSegments[p.X, p.Y];
                }

                if (IsPointCorrect(p.X + 1, p.Y, width, height, maxColorDistance, color))
                {
                    seedPoints.Add(new Point(p.X + 1, p.Y));
                    generatedSegments[p.X + 1, p.Y] = generatedSegments[p.X, p.Y];
                }

                if (IsPointCorrect(p.X, p.Y - 1, width, height, maxColorDistance, color))
                {
                    seedPoints.Add(new Point(p.X, p.Y - 1));
                    generatedSegments[p.X, p.Y - 1] = generatedSegments[p.X, p.Y];
                }

                if (IsPointCorrect(p.X, p.Y + 1, width, height, maxColorDistance, color))
                {
                    seedPoints.Add(new Point(p.X, p.Y + 1));
                    generatedSegments[p.X, p.Y + 1] = generatedSegments[p.X, p.Y];
                }

                if (checkKind == CheckKind.Star)
                {
                    if (IsPointCorrect(p.X - 1, p.Y - 1, width, height, maxColorDistance, color))
                    {
                        seedPoints.Add(new Point(p.X - 1, p.Y - 1));
                        generatedSegments[p.X - 1, p.Y - 1] = generatedSegments[p.X, p.Y];
                    }

                    if (IsPointCorrect(p.X + 1, p.Y - 1, width, height, maxColorDistance, color))
                    {
                        seedPoints.Add(new Point(p.X + 1, p.Y - 1));
                        generatedSegments[p.X + 1, p.Y - 1] = generatedSegments[p.X, p.Y];
                    }

                    if (IsPointCorrect(p.X - 1, p.Y + 1, width, height, maxColorDistance, color))
                    {
                        seedPoints.Add(new Point(p.X - 1, p.Y + 1));
                        generatedSegments[p.X - 1, p.Y + 1] = generatedSegments[p.X, p.Y];
                    }

                    if (IsPointCorrect(p.X + 1, p.Y + 1, width, height, maxColorDistance, color))
                    {
                        seedPoints.Add(new Point(p.X + 1, p.Y + 1));
                        generatedSegments[p.X + 1, p.Y + 1] = generatedSegments[p.X, p.Y];
                    }
                }
            }

            BasePicture basePicture = new BasePicture();
            basePicture.pictureBox1.Image = baseImage;

            Bitmap generatedImage = new Bitmap(width, height);
            Dictionary<int, Color> colors = new Dictionary<int,Color>();
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (!colors.ContainsKey(generatedSegments[x, y]))
                        colors.Add(generatedSegments[x, y], GenerateColor());
                    generatedImage.SetPixel(x, y, colors[generatedSegments[x, y]]);
                }
            }
            GeneratedPicture generatedPicture = new GeneratedPicture();
            generatedPicture.pictureBox1.Image = generatedImage;

            basePicture.Show();
            generatedPicture.Show();
        }

        public bool IsPointCorrect(int x, int y, int width, int height, double colorDistance, Color color)
        {
            return IsPointInPictureBox(x, y, width, height) && IsPointNotUsed(x, y) && (ColorDistance(color, baseImage.GetPixel(x, y)) <= colorDistance);
        }

        public bool IsPointNotUsed(int x, int y)
        {
            return generatedSegments[x, y] == 0;
        }

        public double ColorDistance(Color c1, Color c2)
        {
            return Math.Abs(ColorIntesivity(c1) - ColorIntesivity(c2));
        }

        public double ColorIntesivity(Color c)
        {
            return 0.299 * c.R + 0.587 * c.G + 0.114 * c.B;
        }

        public bool IsPointInPictureBox(int x, int y, int width, int height)
        {
            return (x >= 0) && (y >= 0) && (x < width) && (y < height);
        }

        public Color GenerateColor()
        {
            return Color.FromArgb(rand.Next(255), rand.Next(255), rand.Next(255));
        }

        public enum CheckKind
        {
            //  oxo
            //  xxx
            //  oxo
            Cross,

            //  xxx
            //  xxx
            //  xxx
            Star,
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (baseImage != null)
                GeneratePicture();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.Title = "Open Image";
            dlg.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";

            if (dlg.ShowDialog() == DialogResult.OK)
                baseImage = new Bitmap(dlg.FileName);

            dlg.Dispose();
        }

        private void crossRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (crossRadioButton.Checked)
                checkKind = CheckKind.Cross;
        }

        private void startRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (startRadioButton.Checked)
                checkKind = CheckKind.Star;
        }
    }
}
