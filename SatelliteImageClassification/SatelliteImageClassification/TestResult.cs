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
    }
}
