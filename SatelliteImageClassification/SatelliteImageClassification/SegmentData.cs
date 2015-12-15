using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatelliteImageClassification
{
    class SegmentData
    {
        public static Random rand = new Random();
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
        public static Color GenerateColor()
        {
            return Color.FromArgb(rand.Next(255), rand.Next(255), rand.Next(255));
        }
    }
}
