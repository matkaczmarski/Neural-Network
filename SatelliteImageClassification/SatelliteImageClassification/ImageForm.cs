using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SatelliteImageClassification
{
    public partial class ImageForm : Form
    {
        public ImageForm(Bitmap image)
        {
            InitializeComponent();
            this.pictureBox1.Image = image;
        }
    }
}
