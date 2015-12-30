using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatelliteImageClassification
{
    class SegmentationData
    {
        private static Random rand = new Random();
        public static int MAX_SEGMENT_SIZE = 20;

        private SegmentationData() { }

        public static TrainingData GetTrainingData(string pathToOriginals, string pathToSegments, string pathToBuildings, out Bitmap originalImageRes)
        {
            TrainingData trainingData = new TrainingData();
            originalImageRes = null;
            
            string[] originals = Directory.GetFiles(pathToOriginals);
            string[] segments = Directory.GetFiles(pathToSegments);
            string[] buildings = Directory.GetFiles(pathToBuildings);

            Array.Sort(originals, StringComparer.InvariantCulture);
            Array.Sort(segments, StringComparer.InvariantCulture);
            Array.Sort(buildings, StringComparer.InvariantCulture);

            List<double[]> segmentsList = new List<double[]>();
            List<double[]> idealList = new List<double[]>();

            if (originals.Length != segments.Length || originals.Length != buildings.Length)
                throw new NotImplementedException();
            
            for (int i = 0; i < originals.Length; i++)
            {
                Dictionary<int, SegmentData> nrOfElementsInSegment = new Dictionary<int, SegmentData>();
                int[,] matrix = null;
                int average = int.MaxValue;

                Bitmap originalImage = originalImageRes = new Bitmap(originals[i]);
                Bitmap buildingsImage = new Bitmap(buildings[i]);
                Bitmap segmentsImage = LoadImage(segments[i], out matrix, out nrOfElementsInSegment, out average);

                /*Bitmap idealResult = new Bitmap(originalImage.Width, originalImage.Height);
                for (int x = 0; x < idealResult.Width; x++)
                    for (int y = 0; y < idealResult.Height; y++)
                        if (buildingsImage.GetPixel(x, y).ToArgb() != Color.White.ToArgb())
                            idealResult.SetPixel(x, y, originalImage.GetPixel(x, y));
                        else
                            idealResult.SetPixel(x, y, Color.White);
                ImageForm idealForm = new ImageForm(idealResult);
                idealForm.Text = "IDEAL RESULT";
                idealForm.ShowDialog();

                ImageForm segForm = new ImageForm(segmentsImage);
                segForm.ShowDialog();*/

                List<Point> points;
                List<Bitmap> segmentsImages;
                Tuple<List<double[]>, List<double[]>> result = LoadSegments(originalImage, buildingsImage, matrix, nrOfElementsInSegment, average, out points, out segmentsImages);
                trainingData.Positions = points.ToArray();
                trainingData.Segments = segmentsImages.ToArray();
                trainingData.OriginalImage = originalImage;
                trainingData.SegmentsImage = buildingsImage;
                segmentsList.AddRange(result.Item1);
                idealList.AddRange(result.Item2);
            }

            trainingData.Vectors = segmentsList.ToArray();
            trainingData.Ideal = idealList.ToArray();
            //int nrofbuildings = trainingData.Ideal.Where(x => x[0] == 1).Count();
            //Console.WriteLine(nrofbuildings);
            return trainingData;
        }
        private static Bitmap LoadImage(string filePath, out int[,] matrix, out Dictionary<int, SegmentData> nrOfElementsInSegment, out int average)
        {
            string[] lines = File.ReadAllLines(filePath);
            int x = lines[0].Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries).Length;
            int y = lines.Length - 1;

            string sourcePath = lines[lines.Length - 1].Trim();
            //sourceImage = new Bitmap(sourcePath);

            matrix = new int[x, y];
            nrOfElementsInSegment = new Dictionary<int, SegmentData>();
            for (int i = 0; i < y; i++)
            {
                string[] parts = lines[i].Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < parts.Length; j++)
                {
                    int index = Int32.Parse(parts[j]);
                    if (index == -1)
                        continue;
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
                if (/*nrOfElementsInSegment.ElementAt(i).Value.NrOfElements > average || */nrOfElementsInSegment.ElementAt(i).Value.NrOfElements < 16)
                    nrOfElementsInSegment[nrOfElementsInSegment.ElementAt(i).Key].ColorOfSegment = Color.Black;
            }

            Bitmap generatedImage = new Bitmap(x, y);
            /*for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    if (matrix[i, j] == -1)
                        generatedImage.SetPixel(i, j, Color.Black);
                    else
                        generatedImage.SetPixel(i, j, nrOfElementsInSegment[matrix[i, j]].ColorOfSegment);
                }
            }*/

            return generatedImage;
        }
        private static Tuple<List<double[]>, List<double[]>> LoadSegments(Bitmap sourceImage, Bitmap buildingsImage, int[,] matrix, Dictionary<int, SegmentData> nrOfElementsInSegment, int average, out List<Point> positions, out List<Bitmap> segmentsImages)
        {
            List<double[]> segments = new List<double[]>();
            List<double[]> ideal = new List<double[]>();

            positions = new List<Point>();
            segmentsImages = new List<Bitmap>();

            foreach (KeyValuePair<int, SegmentData> elem in nrOfElementsInSegment)
            {
                if (/*elem.Value.NrOfElements > average ||*/ elem.Value.NrOfElements < 16)
                    continue;
                double[,][] segment = new double[elem.Value.MaxX - elem.Value.MinX + 1, elem.Value.MaxY - elem.Value.MinY + 1][];
                bool[,] isBuildingPixel = new bool[elem.Value.MaxX - elem.Value.MinX + 1, elem.Value.MaxY - elem.Value.MinY + 1];
                for (int x = 0; x < segment.GetLength(0); x++)
                {
                    for (int y = 0; y < segment.GetLength(1); y++)
                    {
                        segment[x, y] = new double[3];
                        if (matrix[x + elem.Value.MinX, y + elem.Value.MinY] == elem.Key)
                        {
                            Color c2 = buildingsImage.GetPixel(x + elem.Value.MinX, y + elem.Value.MinY);
                            Color color = sourceImage.GetPixel(x + elem.Value.MinX, y + elem.Value.MinY);
                            segment[x, y] = new double[] { color.R, color.G, color.B };
                            isBuildingPixel[x, y] = c2.ToArgb() != Color.White.ToArgb();
                        }
                        else
                        {
                            segment[x, y] = new double[] { -1, -1, -1 };
                            isBuildingPixel[x, y] = false;
                        }
                    }
                }
                //Bitmap bitmap = new Bitmap(isBuildingPixel.GetLength(0), isBuildingPixel.GetLength(1));
                //Bitmap segBitmap = GetBitmapFromDoubleArray(segment);
                //for (int x = 0; x < segBitmap.Width; x++)
                //    for (int y = 0; y < segBitmap.Height; y++)
                //        if (isBuildingPixel[x, y])
                //            bitmap.SetPixel(x, y, segBitmap.GetPixel(x, y));
                //        else
                //            bitmap.SetPixel(x, y, Color.White);
                /*ImageForm imageForm = new ImageForm(segBitmap);
                imageForm.Text = "X = " + elem.Value.MinX + " Y = " + elem.Value.MinY;
                imageForm.Show();*/
                // Trzeba rozszerzyć segment
                if (segment.GetLength(0) < MAX_SEGMENT_SIZE || segment.GetLength(1) < MAX_SEGMENT_SIZE)
                {
                    double[,][] subSegment = new double[Math.Max(MAX_SEGMENT_SIZE, segment.GetLength(0)), Math.Max(MAX_SEGMENT_SIZE, segment.GetLength(1))][];
                    for (int x = 0; x < subSegment.GetLength(0); x++)
                        for (int y = 0; y < subSegment.GetLength(1); y++)
                        {
                            if (x >= segment.GetLength(0) || y >= segment.GetLength(1))
                                subSegment[x, y] = new double[] { -1, -1, -1 };
                            else
                                subSegment[x, y] = segment[x, y];
                        }
                    segment = subSegment;
                    bool[,] newIsBuildingPixel = new bool[segment.GetLength(0), segment.GetLength(1)];
                    for (int x = 0; x < newIsBuildingPixel.GetLength(0); x++)
                        for (int y = 0; y < newIsBuildingPixel.GetLength(1); y++)
                        {
                            if (x >= isBuildingPixel.GetLength(0) || y >= isBuildingPixel.GetLength(1))
                                newIsBuildingPixel[x, y] = false;
                            else
                                newIsBuildingPixel[x, y] = isBuildingPixel[x, y];
                        }
                    isBuildingPixel = newIsBuildingPixel;
                }

                // Trzeba podzielić
                if (segment.GetLength(0) > MAX_SEGMENT_SIZE || segment.GetLength(1) > MAX_SEGMENT_SIZE)
                {
                    int div_x = segment.GetLength(0) / MAX_SEGMENT_SIZE;
                    int rest_x = segment.GetLength(0) - MAX_SEGMENT_SIZE * div_x;
                    int stop_x = (rest_x == 0) ? div_x : div_x + 1;

                    int div_y = segment.GetLength(1) / MAX_SEGMENT_SIZE;
                    int rest_y = segment.GetLength(1) - MAX_SEGMENT_SIZE * div_y;
                    int stop_y = (rest_y == 0) ? div_y : div_y + 1;

                    List<double[,][]> subSegments = new List<double[,][]>();
                    // Liczenenie punktów startowych do podziału
                    List<Point> startPoints = new List<Point>();
                    for (int x = 0; x < stop_x; x++)
                    {
                        int point_x = x * MAX_SEGMENT_SIZE;
                        //if (x == div_x)
                        //    point_x -= (MAX_SEGMENT_SIZE - rest_x);
                        for (int y = 0; y < stop_y; y++)
                        {
                            int point_y = y * MAX_SEGMENT_SIZE;
                        //    if (y == div_y)
                        //        point_y -= (MAX_SEGMENT_SIZE - rest_y);

                            startPoints.Add(new Point(point_x, point_y));
                        }
                    }

                    // Tworzenie segmentów o odpowiednim rozmiarze
                    foreach (var startPoint in startPoints)
                    {
                        double[,][] subSegment = new double[MAX_SEGMENT_SIZE, MAX_SEGMENT_SIZE][];
                        for (int x = 0; x < MAX_SEGMENT_SIZE; x++)
                            for (int y = 0; y < MAX_SEGMENT_SIZE; y++)
                            {
                                if (x + startPoint.X >= segment.GetLength(0) || y + startPoint.Y >= segment.GetLength(1))
                                    subSegment[x, y] = new double[] { -1, -1, -1 };
                                else
                                    subSegment[x, y] = segment[x + startPoint.X, y + startPoint.Y];
                            }

                        int isBuilding = 0;
                        int isSomethingElse = 0;
                        for (int x = 0; x < subSegment.GetLength(0); x++)
                        {
                            for (int y = 0; y < subSegment.GetLength(1); y++)
                            {
                                if (x + startPoint.X >= isBuildingPixel.GetLength(0) || y + startPoint.Y >= isBuildingPixel.GetLength(1))
                                    continue;
                                else if (subSegment[x, y][0] == -1)
                                    continue;
                                else if (isBuildingPixel[x + startPoint.X, y + startPoint.Y])
                                    isBuilding++;
                                else
                                    isSomethingElse++;
                            }
                        }
                        if (isBuilding + isSomethingElse < MAX_SEGMENT_SIZE * MAX_SEGMENT_SIZE / 4)
                            continue;
                        if (isBuilding >= isSomethingElse)
                            ideal.Add(new double[] { 1, 0, 0 });
                        else
                            ideal.Add(new double[] { 0, 0, 1 });

                        double[] neighbours = FindNeighbours(startPoints, startPoint);
                        segments.Add(ConvertSegmentsToVector(subSegment, neighbours));

                        Point position = new Point(elem.Value.MinX + startPoint.X, elem.Value.MinY + startPoint.Y);

                        segmentsImages.Add(GetBitmapFromDoubleArray(subSegment));
                        positions.Add(position);

                        /*if (isBuilding > isSomethingElse)
                        {
                            ImageForm imForm = new ImageForm(GetBitmapFromDoubleArray(subSegment));
                            imForm.ShowDialog();
                        }*/
                    }
                }
                else
                {
                    int isBuilding = 0;
                    int isSomethingElse = 0;
                    for (int x = 0; x < isBuildingPixel.GetLength(0); x++)
                    {
                        for (int y = 0; y < isBuildingPixel.GetLength(1); y++)
                        {
                            if (segment[x, y][0] == -1)
                                continue;
                            if (isBuildingPixel[x, y])
                                isBuilding++;
                            else
                                isSomethingElse++;
                        }
                    }
                    if (isBuilding + isSomethingElse < MAX_SEGMENT_SIZE * MAX_SEGMENT_SIZE / 4)
                        continue;
                    if (isBuilding >= isSomethingElse)
                        ideal.Add(new double[] { 1, 0, 0 });
                    else
                        ideal.Add(new double[] { 0, 0, 1 });
                    segments.Add(ConvertSegmentsToVector(segment, new double[] { 0, 0, 0, 0}));

                    Point position = new Point(elem.Value.MinX, elem.Value.MinY);

                    segmentsImages.Add(GetBitmapFromDoubleArray(segment));
                    positions.Add(position);
                }
            }
            Tuple<List<double[]>, List<double[]>> result = new Tuple<List<double[]>, List<double[]>>(segments, ideal);
            return result;
        }

        private static double[] FindNeighbours(List<Point> startPoints, Point startPoint)
        {
            int neighbour_UP = 0;
            int neighbour_DOWN = 0;
            int neighbour_LEFT = 0;
            int neighbour_RIGHT = 0;

            int max_X = int.MinValue;
            int min_X = int.MaxValue;
            int max_Y = int.MinValue;
            int min_Y= int.MaxValue;

            foreach (Point point in startPoints)
            {
                if (point.X < min_X)
                    min_X = point.X;
                if (point.X > max_X)
                    max_X = point.X;
                if (point.Y < min_Y)
                    min_Y = point.Y;
                if (point.Y > max_Y)
                    max_Y = point.Y;
            }

            if (startPoint.Y > min_Y)
                neighbour_UP = 1;
            if (startPoint.Y < max_Y)
                neighbour_DOWN = 1;
            if (startPoint.X > min_X)
                neighbour_LEFT = 1;
            if (startPoint.X < max_X)
                neighbour_RIGHT = 1;

            return new double[] { neighbour_UP, neighbour_DOWN, neighbour_LEFT, neighbour_RIGHT };
        }

        private static double[][] ConvertSegmentsToVector(List<double[,][]> segments)
        {
            double[][] result = new double[segments.Count][];

            int i = 0;
            foreach (var segment in segments)
            {
                if (segment.GetLength(0) == MAX_SEGMENT_SIZE && segment.GetLength(1) == MAX_SEGMENT_SIZE)
                {
                    result[i] = new double[segment.GetLength(0) * segment.GetLength(1) * 3 + 4];
                    for (int y = 0; y < segment.GetLength(1); y++)
                    {
                        for (int x = 0; x < segment.GetLength(0); x++)
                        {
                            result[i][y * segment.GetLength(0) + 3 * x] = segment[x, y][0];
                            result[i][y * segment.GetLength(0) + 3 * x + 1] = segment[x, y][1];
                            result[i][y * segment.GetLength(0) + 3 * x + 2] = segment[x, y][2];
                        }
                    }
                }
                else
                    throw new Exception();
                i++;
            }

            return result;
        }

        private static double[] ConvertSegmentsToVector(double[,][] segment, double[] neighbours)
        {
            /*Bitmap bitmap = GetBitmapFromDoubleArray(segment);
            ImageForm if_1 = new ImageForm(bitmap);
            if_1.Show();*/

            double[] result;
            if (segment.GetLength(0) == MAX_SEGMENT_SIZE && segment.GetLength(1) == MAX_SEGMENT_SIZE)
            {
                result = new double[MAX_SEGMENT_SIZE * MAX_SEGMENT_SIZE * 3 + neighbours.Length];
                for (int y = 0; y < MAX_SEGMENT_SIZE; y++)
                {
                    for (int x = 0; x < MAX_SEGMENT_SIZE; x++)
                    {
                        result[y * MAX_SEGMENT_SIZE * 3 + 3 * x] = segment[x, y][0];
                        result[y * MAX_SEGMENT_SIZE * 3 + 3 * x + 1] = segment[x, y][1];
                        result[y * MAX_SEGMENT_SIZE * 3 + 3 * x + 2] = segment[x, y][2];
                    }
                }
                for (int i = result.Length - neighbours.Length, j = 0; i < result.Length; i++, j++)
                {
                    result[i] = neighbours[j];
                }
            }
            else
                throw new Exception();

            /*Bitmap bitmap_2 = GetBitmapFromDoubleArray(result);
            ImageForm if_2 = new ImageForm(bitmap_2);
            if_2.ShowDialog();*/

            return result;
        }

        private static Bitmap GetBitmapFromDoubleArray(double[,][] pixels)
        {
            Bitmap bitmap = new Bitmap(pixels.GetLength(0), pixels.GetLength(1));
            for (int x = 0; x < bitmap.Width; x++)
                for (int y = 0; y < bitmap.Height; y++)
                {
                    int R = (int)pixels[x, y][0];
                    int G = (int)pixels[x, y][1];
                    int B = (int)pixels[x, y][2];
                    if (R >= 0 && G >= 0 && B >= 0)
                        bitmap.SetPixel(x, y, Color.FromArgb(R, G, B));
                    else
                        bitmap.SetPixel(x, y, Color.White);
                }
            return bitmap;
        }

        public static Bitmap GetBitmapFromDoubleArray(double[] data)
        {
            int size = (int)Math.Sqrt((data.Length - 4) / 3);
            double[,][] pixels = new double[size, size][];
            for (int x = 0; x < size; x++)
                for (int y = 0; y < size; y++)
                    pixels[x, y] = new double[] { data[y * size * 3 + 3 * x], data[y * size * 3 + 3 * x + 1], data[y * size * 3 + 3 * x + 2] };

            Bitmap bitmap = new Bitmap(pixels.GetLength(0), pixels.GetLength(1));
            for (int x = 0; x < bitmap.Width; x++)
                for (int y = 0; y < bitmap.Height; y++)
                {
                    int R = (int)pixels[x, y][0];
                    int G = (int)pixels[x, y][1];
                    int B = (int)pixels[x, y][2];
                    if (R >= 0 && G >= 0 && B >= 0)
                        bitmap.SetPixel(x, y, Color.FromArgb(R, G, B));
                    else
                        bitmap.SetPixel(x, y, Color.White);
                }
            return bitmap;
        }

        public static double[,][] CovertFrom1DArray(double[] data)
        {
            int size = (int)Math.Sqrt((data.Length - 4) / 3);
            double[,][] pixels = new double[size, size][];
            for (int x = 0; x < size; x++)
                for (int y = 0; y < size; y++)
                    pixels[x, y] = new double[] { data[y * size * 3 + 3 * x], data[y * size * 3 + 3 * x + 1], data[y * size * 3 + 3 * x + 2] };

            return pixels;
        }
    }
}
