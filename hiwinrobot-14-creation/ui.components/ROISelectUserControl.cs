using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace hiwinrobot_14_creation.ui.components
{
    public partial class ROISelectUserControl : UserControl
    {

        public Rectangle ROI { get; set; }
        public ROISelectUserControl()
        {
            InitializeComponent();
            ROI = Rectangle.Empty;
        }

        public void OnImageCaptured(Bitmap image)
        {
            if (pictureBox1.Image != null)
                pictureBox1.Dispose();
            var cvImage=image.ToImage<Bgr, byte>();
            if (ROI != Rectangle.Empty)
                CvInvoke.Rectangle(cvImage, ROI, new MCvScalar(0, 0, 255), 3);
            pictureBox1.Image = image;
        }

    }
}
