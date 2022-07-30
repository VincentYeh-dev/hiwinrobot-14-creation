using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RASDK.Vision;
using RASDK.Vision.Positioning;
using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace PerlerBeadsHandler
{
    public static class VisualSystem
    {
        #region Config

        private static readonly Size _checkBoardSize = new Size(12, 9);
        private static readonly float _checkBoardSquareSideLength = 3;
        private static readonly double _allowablePositioningPixelError = 5.0;

        #endregion Config

        public static CameraParameter CameraCalibration(out double reprojectionError)
        {
            var images = new List<Image<Bgr, byte>>
            {
                ReadImageFromCamera()
            };

            var cc = new CameraCalibration(_checkBoardSize, _checkBoardSquareSideLength);
            reprojectionError = cc.Run(images,
                                       out var cm,
                                       out var dc,
                                       out var rv,
                                       out var tv);

            return new CameraParameter(cm, dc, rv[0], tv[0]);
        }

        public static PointF ImageToWorld(this Point pixel,
                                          PointF offset,
                                          CameraParameter cameraParam,
                                          double timeout = 1.5)
        {
            var positioner = new CCIA(cameraParam, _allowablePositioningPixelError)
            {
                WorldOffset = offset
            };

            var worldPoint = positioner.ImageToWorld(pixel, timeout);
            return worldPoint;
        }

        public static Image<Bgr, byte> ReadImageFromCamera()
        {
            var camera = new RASDK.Vision.IDS.IDSCamera();

            var ok = camera.Connect();
            if (!ok)
            {
                throw new Exception("IDS camera connect error.");
            }

            var img = camera.GetImage();

            camera.Disconnect();
            return img.ToImage<Bgr, byte>();
        }
    }
}