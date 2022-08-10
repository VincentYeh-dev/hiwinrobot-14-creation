using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Timer = System.Timers.Timer;
using RASDK.Arm;
using RASDK.Arm.Type;
using RASDK.Arm.Hiwin;
using RoboticArm = RASDK.Arm.Hiwin.RoboticArm;
using RASDK.Vision;
using RASDK.Vision.IDS;
using RASDK.Vision.Positioning;
using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Aruco;

namespace PerlerBeads
{
    public static class VisualSystem
    {
        #region Config

        private static readonly Size _checkBoardSize = new Size(12, 9);
        private static readonly float _checkBoardSquareSideLength = 3;
        private static readonly double _allowablePositioningPixelError = 5.0;

        #endregion Config

        private static readonly PointF[] _homographyImagePoints = new PointF[]
        {
                //0-0
                new PointF(1241,738),
                //0-10
                new PointF(2042,752),
                //7-0
                new PointF(1233,1299),
                //7-10
                new PointF(2032,1311)
        };

        private static readonly PointF[] _homographyArmPoints = new PointF[]
        {
                //0-0
                new PointF(-63.775f,485.492f),
                //0-10
                new PointF(84.941f,485.115f),
                //7-0
                new PointF(-66.126f,380.732f),
                //7-10
                new PointF(84.941f,380.732f)
        };

        public static void Positioning(IDSCamera camera)
        {
            //var cc = new CameraCalibration(_checkBoardSize, _checkBoardSquareSideLength);

            //var images = new List<Image<Bgr, byte>>
            //    {
            //        new Image<Bgr, byte>(@"C:\Users\wk415\Pictures\0,400,25_5.jpg"),
            //        //new Image<Bgr, byte>(@"C:\Users\wk415\Pictures\0,400,25_4.jpg"),
            //        //new Image<Bgr, byte>(@"C:\Users\wk415\Pictures\0,400,25_3.jpg"),
            //        //new Image<Bgr, byte>(@"C:\Users\wk415\Pictures\0,400,25_2.jpg"),
            //        //new Image<Bgr, byte>(@"C:\Users\wk415\Pictures\0,400,25_1.jpg"),
            //    };

            //var reprojectionError = cc.Run(images,
            //                               out var cm,
            //                               out var dc,
            //                               out var rv,
            //                               out var tv);

            //var cp = new CameraParameter(cm, dc, rv[0], tv[0]);
            //var positioner = new CCIA(cp, 5, null, Approximation);

            var positioner = CCIA.LoadFromCsv("ccia_param.csv");

            var srcImage = ReadImageFromCamera(camera);
            //var srcImage = new Image<Bgr, byte>(@"C:\Users\wk415\Pictures\aruco_b1.jpg");
            srcImage.Save("src.jpg");
            var undistortImage = RASDK.Vision.CameraCalibration.UndistortImage(srcImage.Clone(), positioner.CameraParameter);
            undistortImage.Save("undist.jpg");

            var arucoCorners = new VectorOfVectorOfPointF();
            var arucoIds = new VectorOfInt();
            ArucoInvoke.DetectMarkers(srcImage,
                                      new Dictionary(Dictionary.PredefinedDictionaryName.Dict4X4_50),
                                      arucoCorners,
                                      arucoIds,
                                      DetectorParameters.GetDefault());

            var referenceArucoCorner = new PointF[4];
            var ids = arucoIds.ToArray();
            for (int i = 0; i < ids.Length; i++)
            {
                if (ids[i] == 0)
                {
                    var c = arucoCorners.ToArrayOfArray();
                    referenceArucoCorner[0] = c[i][0];
                    referenceArucoCorner[1] = c[i][1];
                    referenceArucoCorner[2] = c[i][2];
                    referenceArucoCorner[3] = c[i][3];
                }
            }

            var referenceArmCorner = new PointF[]
            {
                positioner.ImageToWorld(referenceArucoCorner[0]),
                positioner.ImageToWorld(referenceArucoCorner[1]),
                positioner.ImageToWorld(referenceArucoCorner[2]),
                positioner.ImageToWorld(referenceArucoCorner[3]),
            };

            var ang = Math.Atan2((double)referenceArucoCorner[0].Y - (double)referenceArucoCorner[1].Y,
                                 (double)referenceArucoCorner[1].X - (double)referenceArucoCorner[0].X);
            var deg = ang * 180.0 / Math.PI;

            var rm = new Matrix<double>(2, 3);
            CvInvoke.GetRotationMatrix2D(referenceArmCorner[0], deg, 1, rm);
            var oPoint = new Matrix<double>(3, 1);
            oPoint[0, 0] = referenceArmCorner[0].X + 10;
            oPoint[1, 0] = referenceArmCorner[0].Y - 50;
            oPoint[2, 0] = 1;

            var r = rm * oPoint;
        }

        public static CameraParameter CameraCalibration(List<Image<Bgr, byte>> images, out double reprojectionError)
        {
            if (images == null || images.Count == 0)
            {
                throw new ArgumentException(nameof(images), "Should not be null or count <= 0.");
            }

            var cc = new CameraCalibration(_checkBoardSize, _checkBoardSquareSideLength);
            var cp = cc.CalCameraParameter(images, out _, out _, out _, out _, out reprojectionError);

            return cp;
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

            var worldPoint = positioner.ImageToWorld(pixel);
            return worldPoint;
        }

        public static PointF VisualServoing(RoboticArm arm, IDSCamera camera, double kp, double allowableError, double timeout)
        {
            var error = new PointF(float.NaN, float.NaN);

            var timerCount = 0.0;
            var timer = new Timer(100);

            if (timeout >= 0)
            {
                timer.Stop();
                timer.Elapsed += (s, e) => { timerCount += 0.1; };
                timer.Start();
            }
            while (timerCount < timeout || timeout < 0)
            {
                try
                {
                    error = VisualServoingInterative(arm, camera, kp);
                }
                catch
                { }

                if (Math.Abs(error.X) <= allowableError &&
                    Math.Abs(error.Y) <= allowableError &&
                    timeout >= 0)
                {
                    break;
                }
            }
            timer.Stop();

            return error;
        }

        public static PointF VisualServoingInterative(RoboticArm arm, IDSCamera camera, double kp)
        {
            var image = ReadImageFromCamera(camera);
            var arucoCorners = FindArucoCorners(image, 0);
            if (arucoCorners == null)
            {
                throw new Exception($"Con't find ArUco id: {0}.");
            }

            // Calc error in pixel.
            var imageCenter = new Point((image.Size.Width / 2) - 1, (image.Size.Height / 2) - 1);
            var errorX = arucoCorners[0].X - imageCenter.X;
            var errorY = arucoCorners[0].Y - imageCenter.Y;

            // P control.
            var position = new double[]
            {
                kp * errorX,
                kp * -errorY,
                0,

                0,
                0,
                0
            };
            arm.MoveRelative(position,
                             new AdditionalMotionParameters
                             {
                                 MotionType = RASDK.Arm.Type.MotionType.Linear,
                                 NeedWait = true
                             });

            return new PointF(errorX, errorY);
        }

        public static Image<Bgr, byte> ReadImageFromCamera(IDSCamera camera)
        {
            if (!camera.Connected)
            {
                throw new Exception("Camera not connected.");
            }

            var img = camera.GetImage();
            return img.ToImage<Bgr, byte>();
        }

        private static double CalcArucoAngle(PointF[] corners, bool inDegree = true)
        {
            var angleRad = Math.Atan2((double)corners[0].Y - (double)corners[1].Y,
                                      (double)corners[1].X - (double)corners[0].X);
            return inDegree ? (angleRad * 180.0 / Math.PI) : angleRad;
        }

        private static PointF[] FindArucoCorners(Image<Bgr, byte> image, int id)
        {
            var corners = new VectorOfVectorOfPointF();
            var ids = new VectorOfInt();
            ArucoInvoke.DetectMarkers(image,
                                      new Dictionary(Dictionary.PredefinedDictionaryName.Dict4X4_50),
                                      corners,
                                      ids,
                                      DetectorParameters.GetDefault());

            var goalCorner = new PointF[4];
            var idsArray = ids.ToArray();
            for (int i = 0; i < idsArray.Length; i++)
            {
                if (ids[i] == id)
                {
                    var c = corners.ToArrayOfArray();
                    goalCorner[0] = c[i][0];
                    goalCorner[1] = c[i][1];
                    goalCorner[2] = c[i][2];
                    goalCorner[3] = c[i][3];

                    return goalCorner;
                }
            }

            return null;
        }

        private static void Approximation(double errorX, double errorY, ref double vX, ref double vY)
        {
            var kp = 0.02;
            vX += errorX * kp;
            vY += errorY * kp;
        }
    }
}