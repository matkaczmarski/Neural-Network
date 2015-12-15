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
        private static int MAX_SEGMENT_SIZE = 8;

        private SegmentationData() { }

        public static TrainingData GetTrainingData(string pathToOriginals, string pathToSegments, string pathToBuildings)
        {
            TrainingData trainingData = new TrainingData();
            
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

                Bitmap originalImage = new Bitmap(originals[i]);
                Bitmap buildingsImage = new Bitmap(buildings[i]);
                Bitmap segmentsImage = LoadImage(segments[i], out matrix, out nrOfElementsInSegment, out average);

                Tuple<List<double[]>, List<double[]>> result = LoadSegments(originalImage, buildingsImage, matrix, nrOfElementsInSegment, average);
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
        private static Tuple<List<double[]>, List<double[]>> LoadSegments(Bitmap sourceImage, Bitmap buildingsImage, int[,] matrix, Dictionary<int, SegmentData> nrOfElementsInSegment, int average)
        {
            List<double[]> segments = new List<double[]>();
            List<double[]> ideal = new List<double[]>();

            foreach (KeyValuePair<int, SegmentData> elem in nrOfElementsInSegment)
            {
                if (elem.Value.NrOfElements > average || elem.Value.NrOfElements < average / 4)
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
                            Color color = sourceImage.GetPixel(x + elem.Value.MinX, y + elem.Value.MinY);
                            segment[x, y] = new double[] { color.R, color.G, color.B };
                            isBuildingPixel[x, y] = color != Color.White;
                        }
                        else
                        {
                            segment[x, y] = new double[] { -1, -1, -1 };
                            isBuildingPixel[x, y] = false;
                        }
                    }
                }

                // Trzeba rozszerzyć segment
                if (segment.GetLength(0) < MAX_SEGMENT_SIZE || segment.GetLength(1) < MAX_SEGMENT_SIZE)
                {
                    double[,][] subSegment = new double[Math.Max(MAX_SEGMENT_SIZE, segment.GetLength(0)), Math.Max(MAX_SEGMENT_SIZE, segment.GetLength(1))][];
                    for (int x = 0; x < subSegment.GetLength(0); x++)
                        for (int y = 0; y < subSegment.GetLength(1); y++)
                        {
                            if (x >= segment.GetLength(0) || y >= segment.GetLength(0))
                                subSegment[x, y] = new double[] { -1, -1, -1 };
                            else
                                subSegment[x, y] = segment[x, y];
                        }
                    segment = subSegment;
                }

                // Trzeba podzielić
                if (segment.GetLength(0) > MAX_SEGMENT_SIZE || segment.GetLength(1) > MAX_SEGMENT_SIZE)
                {
                    int div_x = segment.GetLength(0) / MAX_SEGMENT_SIZE;
                    int rest_x = segment.GetLength(0) - MAX_SEGMENT_SIZE * div_x;

                    int div_y = segment.GetLength(1) / MAX_SEGMENT_SIZE;
                    int rest_y = segment.GetLength(1) - MAX_SEGMENT_SIZE * div_y;

                    List<double[,][]> subSegments = new List<double[,][]>();
                    // Liczenenie punktów startowych do podziału
                    List<Point> startPoints = new List<Point>();
                    for (int x = 0; x < div_x + 1; x++)
                    {
                        int point_x = x * MAX_SEGMENT_SIZE;
                        if (x == div_x)
                            point_x -= (MAX_SEGMENT_SIZE - rest_x);
                        for (int y = 0; y < div_y + 1; y++)
                        {
                            int point_y = y * MAX_SEGMENT_SIZE;
                            if (y == div_y)
                                point_y -= (MAX_SEGMENT_SIZE - rest_y);

                            startPoints.Add(new Point(point_x, point_y));
                        }
                    }

                    // Tworzenie segmentów o odpowiednim rozmiarze
                    foreach (var startPoint in startPoints)
                    {
                        double[,][] subSegment = new double[MAX_SEGMENT_SIZE, MAX_SEGMENT_SIZE][];
                        for (int x = 0; x < MAX_SEGMENT_SIZE; x++)
                            for (int y = 0; y < MAX_SEGMENT_SIZE; y++)
                                subSegment[x, y] = segment[x + startPoint.X, y + startPoint.Y];

                        int isBuilding = 0;
                        int isSomethingElse = 0;
                        for (int x = 0; x < subSegment.GetLength(0); x++)
                        {
                            for (int y = 0; y < subSegment.GetLength(1); y++)
                            {
                                if (isBuildingPixel[x + startPoint.X, y + startPoint.Y])
                                    isBuilding++;
                                else
                                    isSomethingElse++;
                            }
                        }
                        if (isBuilding >= isSomethingElse)
                            ideal.Add(new double[] { 1, 0, 0 });
                        else
                            ideal.Add(new double[] { 0, 0, 1 });
                        segments.Add(ConvertSegmentsToVector(subSegment));
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
                            if (isBuildingPixel[x, y])
                                isBuilding++;
                            else
                                isSomethingElse++;
                        }
                    }
                    if (isBuilding >= isSomethingElse)
                        ideal.Add(new double[] { 1, 0, 0 });
                    else
                        ideal.Add(new double[] { 0, 0, 1 });
                    segments.Add(ConvertSegmentsToVector(segment));
                }
            }
            Tuple<List<double[]>, List<double[]>> result = new Tuple<List<double[]>, List<double[]>>(segments, ideal);
            return result;
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

        private static double[] ConvertSegmentsToVector(double[,][] segment)
        {
            double[] result;
            if (segment.GetLength(0) == MAX_SEGMENT_SIZE && segment.GetLength(1) == MAX_SEGMENT_SIZE)
            {
                result = new double[segment.GetLength(0) * segment.GetLength(1) * 3 + 4];
                for (int y = 0; y < segment.GetLength(1); y++)
                {
                    for (int x = 0; x < segment.GetLength(0); x++)
                    {
                        result[y * segment.GetLength(0) + 3 * x] = segment[x, y][0];
                        result[y * segment.GetLength(0) + 3 * x + 1] = segment[x, y][1];
                        result[y * segment.GetLength(0) + 3 * x + 2] = segment[x, y][2];
                    }
                }
            }
            else
                throw new Exception();


            return result;
        }
    }
}
