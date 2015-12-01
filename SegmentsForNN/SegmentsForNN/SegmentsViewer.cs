using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SegmentsForNN
{
    public partial class SegmentsViewer : Form
    {
        public List<Bitmap> Segments { get; set; }
        private int index = 0;
        public SegmentsViewer(List<Bitmap> segments)
        {
            InitializeComponent();

            Segments = segments;

            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;

            pictureBox1.Image = Segments[0];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = Segments[(++index % Segments.Count)];
        }
    }
}
