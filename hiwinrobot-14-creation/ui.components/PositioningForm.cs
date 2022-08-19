using Emgu.CV;
using Emgu.CV.Structure;
using RASDK.Vision.IDS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace hiwinrobot_14_creation.ui.components
{
    public partial class PositioningForm : Form
    {
        public delegate Bitmap GetImage();
        private delegate void DelShow(Bitmap image);

        public Rectangle ROI { set; get; }
        public int Delay { set; get; }
        private GetImage getImage;
        private bool _run;

        public PositioningForm()
        {
            InitializeComponent();
            ROI = Rectangle.Empty;
            new Thread(start).Start();
        }

        private void start()
        {
            while (_run)
            {
                Show(getImage());
                Thread.Sleep(Delay);
            }
        }
        private void Show(Bitmap image)
        {
            if (this.InvokeRequired)
            {
                var startDel = new DelShow(Show);
                this.Invoke(startDel, image);
            }
            else
            {
                if (pictureBox1.Image != null)
                    pictureBox1.Image.Dispose();
                var cvImage=image.ToImage<Bgr, byte>();
                if (ROI != Rectangle.Empty)
                    CvInvoke.Rectangle(cvImage, ROI, new MCvScalar(0, 0, 255), 3);
                pictureBox1.Image = cvImage.ToBitmap();
                cvImage.Dispose();
            }

        }
        public void SetGetImage(GetImage getImage)
        {
            this.getImage=getImage;
        }
        public void Start()
        {
            _run = true;
        }
        public void Stop()
        {
            _run = false;
        }
             
    }
}
