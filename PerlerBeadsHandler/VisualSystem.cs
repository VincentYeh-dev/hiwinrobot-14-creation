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
        public static PointF CalcPositionWithOffset(double angleDeg,
                                                    PointF offset,
                                                    PointF oriPoint,
                                                    PointF? centerPoint = null)
        {
            var rotationMatrix = new RotationMatrix2D(centerPoint ?? oriPoint, angleDeg, 1);
            var points = new[]
            {
                new PointF(oriPoint.X + offset.X,
                           oriPoint.Y + offset.Y)
            };
            rotationMatrix.RotatePoints(points);

            return points[0];
        }

        public static PointF VisualServoing(RoboticArm arm,
                                            IDSCamera camera,
                                            double kp,
                                            double allowableError,
                                            double timeout,
                                            int arucoId)
        {
            var sampleImage = ReadImageFromCamera(camera);

            Func<PointF> getPixelFunc =
                VisualServo.MakeBasicArucoGetCurrentPixelFunc(camera,
                                                              arucoId,
                                                              new Dictionary(
                                                                  Dictionary.PredefinedDictionaryName.Dict4X4_50),
                                                              DetectorParameters.GetDefault());
            Action<PointF> armMoveFunc = VisualServo.MakeBasicArmMoveFunc(arm, kp);

            var error = VisualServo.Tracking(sampleImage.Size,
                                             timeout,
                                             getPixelFunc,
                                             armMoveFunc,
                                             allowableError);

            return error;
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

        public static double CalcArucoAngle(PointF[] corners, bool inDegree = true)
        {
            var angleRad = Math.Atan2((double)corners[0].Y - (double)corners[1].Y,
                                      (double)corners[1].X - (double)corners[0].X);
            return inDegree ? (angleRad * 180.0 / Math.PI) : angleRad;
        }

        public static PointF[] FindArucoCorners(Image<Bgr, byte> image, int id)
        {
            var corners = new VectorOfVectorOfPointF();
            var ids = new VectorOfInt();
            ArucoInvoke.DetectMarkers(image,
                                      new Dictionary(Dictionary.PredefinedDictionaryName.Dict4X4_50),
                                      corners,
                                      ids,
                                      DetectorParameters.GetDefault());

            var idsArray = ids.ToArray();
            for (int i = 0; i < idsArray.Length; i++)
            {
                if (ids[i] == id)
                {
                    var c = corners.ToArrayOfArray();
                    var goalCorner = new PointF[]
                    {
                        c[i][0],
                        c[i][1],
                        c[i][2],
                        c[i][3]
                    };

                    return goalCorner;
                }
            }

            // Not found.
            return null;
        }
    }
}