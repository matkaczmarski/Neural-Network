using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatelliteImageClassification
{
    class TestResult
    {
        public int CorrectPixels { get; set; }
        public int WrongPixels { get; set; }
        public string Percentage 
        { 
            get
            {
                return string.Format("{0:f2}", (((float)CorrectPixels)) / ((float)CorrectPixels + WrongPixels));
            }
        }
        public int BuildingPixels { get; set; }
        public int WrongBuildingPixels { get; set; }
        public int TerrainPixels { get; set; }
        public int WrongTerrainPixels { get; set; }
        public int TruePositiveBuildings { get; set; }
        public int TruePositiveTerrain { get; set; }

        public int TP { get; set; }
        public int FP { get; set; }
        public int TN { get; set; }
        public float DP
        {
            get
            {
                if (TP + TN != 0)
                    return 100 * (float)TP / ((float)TP + (float)TN);
                else
                    return 100;
            }
        }
        public float BF
        {
            get
            {
                if (TP + FP != 0)
                    return 100 * (float)FP / ((float)TP + (float)FP);
                else
                    return 0;
            }
        }

    }
}
