using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatelliteImageClassification
{
    class TrainingData
    {
        public double[][] Vectors { get; set; }
        public double[][] Ideal { get; set; }
        public Bitmap OriginalImage { get; set; }
        public Bitmap SegmentsImage { get; set; }
        public Point[] Positions { get; set; }
        public Bitmap[] Segments { get; set; }
    }
}
