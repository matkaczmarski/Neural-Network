using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SegmentsForNN
{
    public partial class Form1 : Form
    {
        private static Random rand = new Random();

        Dictionary<int, SegmentData> nrOfElementsInSegment = new Dictionary<int, SegmentData>();
        int[,] matrix = null;
        int average = int.MaxValue;
        
        public Form1()
        {
            InitializeComponent();

            Bitmap sourceImage = new Bitmap("C:\\Users\\Kuba\\Desktop\\lab4\\lab4\\IN_OpenCV\\test1.jpg");
            Bitmap image = LoadImage("C:\\Users\\Kuba\\Desktop\\lab4\\lab4\\IN_OpenCV\\some_name.txt");//, out sourceImage);
            pictureBox1.Image = image;

            List<Bitmap> segments = LoadSegments(sourceImage, image);
            SegmentsViewer sv = new SegmentsViewer(segments);
            sv.Show();
        }

        private List<Bitmap> LoadSegments(Bitmap sourceImage, Bitmap image)
        {
            List<Bitmap> segments = new List<Bitmap>();
            foreach (KeyValuePair<int, SegmentData> elem in nrOfElementsInSegment)
            {
                if (elem.Value.NrOfElements > average || elem.Value.NrOfElements < average / 4)
                    continue;
                Bitmap bitmap = new Bitmap(elem.Value.MaxX - elem.Value.MinX + 1, elem.Value.MaxY - elem.Value.MinY + 1);
                for (int x = 0; x < bitmap.Width; x++)
                {
                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        bitmap.SetPixel(x, y, matrix[x + elem.Value.MinX, y + elem.Value.MinY] == elem.Key ? sourceImage.GetPixel(x + elem.Value.MinX, y + elem.Value.MinY) : Color.White);
                    }
                }
                segments.Add(bitmap);
            }
            return segments;
        }

        public Bitmap LoadImage(string filePath)//, out Bitmap sourceImage)
        {
            string[] lines = File.ReadAllLines(filePath);
            int x = lines[0].Split(new string[] {"\t"}, StringSplitOptions.RemoveEmptyEntries).Length;
            int y = lines.Length - 1;

            string sourcePath = lines[lines.Length - 1].Trim();
            //sourceImage = new Bitmap(sourcePath);

            matrix = new int[x, y];
            for (int i = 0; i < y; i++)
            {
                string[] parts = lines[i].Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < parts.Length; j++)
                {
                    int index = Int32.Parse(parts[j]);
                    if (nrOfElementsInSegment.ContainsKey(index))
                    {
                        nrOfElementsInSegment[index].NrOfElements++;
                        if (nrOfElementsInSegment[index].MinX > j)
                            nrOfElementsInSegment[index].MinX = j;
                        if (nrOfElementsInSegment[index].MinY > i)
                            nrOfElementsInSegment[index].MinY = i;
                        if (nrOfElementsInSegment[index].MaxX < j)
                            nrOfElementsInSegment[index].MaxX = j;
                        if (nrOfElementsInSegment[index].MaxY < i)
                            nrOfElementsInSegment[index].MaxY = i;
                    }
                    else
                        nrOfElementsInSegment.Add(index, new SegmentData());
                    matrix[j, i] = index;
                }
            }
            
            int sum = 0;
            foreach (KeyValuePair<int, SegmentData> elem in nrOfElementsInSegment)
            {
                sum += elem.Value.NrOfElements;
            }

            average = 2 * sum / nrOfElementsInSegment.Count;

            for (int i = 0; i < nrOfElementsInSegment.Count; i++)
            {
                if (nrOfElementsInSegment.ElementAt(i).Value.NrOfElements > average || nrOfElementsInSegment.ElementAt(i).Value.NrOfElements < average / 4)
                    nrOfElementsInSegment[nrOfElementsInSegment.ElementAt(i).Key].ColorOfSegment = Color.Black;
            }

            Bitmap generatedImage = new Bitmap(x, y);
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    generatedImage.SetPixel(i, j, nrOfElementsInSegment[matrix[i, j]].ColorOfSegment);
                }
            }

            return generatedImage;
        }
        
        public static Color GenerateColor()
        {
            return Color.FromArgb(rand.Next(255), rand.Next(255), rand.Next(255));
        }

        public class SegmentData
        {
            public int NrOfElements { get; set; }
            public Color ColorOfSegment { get; set; }
            public int MaxX { get; set; }
            public int MinX { get; set; }
            public int MaxY { get; set; }
            public int MinY { get; set; }

            public SegmentData()
            {
                this.NrOfElements = 1;
                this.ColorOfSegment = GenerateColor();
                MaxX = MaxY = int.MinValue;
                MinX = MinY = int.MaxValue;
            }

        }
    }
}
