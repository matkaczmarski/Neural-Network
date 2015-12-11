using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatelliteImageClassification
{
    public class ImageConverter
    {
        const int DIGITSIZE = 20;
        const int ROWSPERDIGIT = 5;
        const int COLUMNS = 100;
        const int DIGITSCOUNT = 10;
        Bitmap image;

        public ImageConverter(string fileName)
        {
            image = new Bitmap(fileName);
        }

        public double[][] GetTrainingData(out double[][] ideal)
        {
            double[][] data = new double[COLUMNS * ROWSPERDIGIT * DIGITSCOUNT][];
            ideal = new double[COLUMNS * ROWSPERDIGIT * DIGITSCOUNT][];
            int symbol = -1;
            int i = 0;
            for(int r=0; r<DIGITSCOUNT*ROWSPERDIGIT; r++)
            {
                if (r % ROWSPERDIGIT == 0)
                    symbol++;
                for(int c=0; c<COLUMNS; c++)
                {
                    List<double> digitPixels = new List<double>() { };
                    //r*digitsize, c*digitsize
                    for(int rr=0; rr<DIGITSIZE; rr++)
                        for(int cc=0; cc<DIGITSIZE; cc++)
                        {
                            int rInd = r*DIGITSIZE + rr;
                            int cInd = c*DIGITSIZE + cc;
                            if(rInd >= image.Height || cInd >= image.Width)
                            {
                                continue;
                            }
                            Color pixelColor = image.GetPixel(c * DIGITSIZE + cc, r * DIGITSIZE + rr);
                            digitPixels.Add(pixelColor.R/255.0);
                        }
                    double[] idealVec = new double[10];
                    idealVec[symbol] = 1;
                    ideal[i] = idealVec;
                    data[i++] = digitPixels.ToArray();                  
                }
            }

            return data;

        }
    }
}
