using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerlerBeads;
using RASDK.Arm;
using RASDK.Arm.Type;
using RASDK.Basic;
using RASDK.Vision;
using RASDK.Vision.Positioning;
using RASDK.Basic.Message;
using Hiwin = RASDK.Arm.Hiwin;
using MotionParam = RASDK.Arm.AdditionalMotionParameters;
using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.Drawing;

namespace hiwinrobot_14_creation
{
    public partial class Form1
    {
        /// <summary>
        /// 組織動作流程。
        /// </summary>
        private void OrganizeActions()
        {
            _actions.Add("Test", () => _messageHandler.Show("Test action."));

            _actions.Add("Homing", () => _arm.Homing());
            _actions.Add("Open Camera", CameraConnect);
            _actions.Add("移動到相機標定的位置", MoveToCapture);
            _actions.Add("Visual Servoing", VisualServoing);
            _actions.Add("移動到針板原點", MoveToPegboardOrigin);
            _actions.Add("Down", MoveDown);
            _actions.Add("ABORT", () => { /* Do nothing. */ });
            _actions.Add("Close Camera", CameraDisconnect);
            _actions.Add("進行拼豆", CreatePerlerBeads);

            _actions.Add("End", () => _messageHandler.Show("所有動作已結束。"));
        }

        #region Variables

        private readonly string _armIp = @"192.168.0.1";
        private readonly float _distanceOfCameraAndEndEffector = 110;
        private readonly int _armInitSpeed = 20;
        private PointF _worldOffset = new PointF(0, -50);

        #region Positions

        private double[] _pegboardOriginPosition = new double[6];

        private double[] _testPosition => new double[] { };

        private double[] _capturePosition
        {
            get
            {
                var pos = Hiwin.Default.DescartesHomePosition.Clone() as double[];
                pos[2] = 230.336; // Z.
                return pos;
            }
        }

        #endregion Positions

        #endregion Variables

        private void MoveToCapture()
        {
            _arm.MoveAbsolute(_capturePosition);
        }

        private void MoveToPegboardOrigin()
        {
            _arm.MoveAbsolute(_pegboardOriginPosition);
        }

        private void MoveDown()
        {
            var pos = _pegboardOriginPosition.Clone() as double[];
            pos[2] = 5;
            _arm.MoveAbsolute(pos, new MotionParam { MotionType = RASDK.Arm.Type.MotionType.Linear });
        }

        private void CameraConnect()
        {
            var ok = _camera.Connect();
            if (!ok)
            {
                _messageHandler.Show("Can't connect to camera.");
                return;
            }
            _camera.LoadParameterFromEEPROM();
        }

        private void CameraDisconnect()
        {
            _camera.Disconnect();
        }

        private void VisualServoing()
        {
            var arucoId = 1;
            var timeout = 20;
            var allowableError = 4;
            var kp = (20.0 / 130.0) * 0.8; // mm per pixel * gain.

            var error = VisualSystem.VisualServoing(_arm, _camera, kp, allowableError, timeout, arucoId);

            var image = _camera.GetImage();
            image.Save("visual_servoing_done.jpg");

            var arucoCorners = VisualSystem.FindArucoCorners(image.ToImage<Bgr, byte>(), arucoId);
            var arucoAngle = VisualSystem.CalcArucoAngle(arucoCorners);

            var presentPosition = _arm.GetNowPosition();
            var presentPoint = new PointF((float)presentPosition[0], (float)presentPosition[1]);
            var centerPoint = new PointF(presentPoint.X, presentPoint.Y + _distanceOfCameraAndEndEffector);

            var pegboardOrigin = VisualSystem.CalcPositionWithOffset(-arucoAngle, _worldOffset, presentPoint, centerPoint);
            _pegboardOriginPosition = _capturePosition;
            _pegboardOriginPosition[0] = pegboardOrigin.X;
            _pegboardOriginPosition[1] = pegboardOrigin.Y;

            _messageHandler.Log($"Visual servoing error: {error.X}, {error.Y}; " +
                                $"Aruco point: {presentPoint.X}, {presentPoint.Y}; " +
                                $"Aruco angle deg: {arucoAngle}; " +
                                $"Pegboard origin: {pegboardOrigin.X}, {pegboardOrigin.Y}.",
                                LoggingLevel.Info);
        }

        private void CreatePerlerBeads()
        {
            _messageHandler.Log($"開始進行拼豆。", LoggingLevel.Trace);
            _beadsHandler.Run();
            _messageHandler.Log($"拼豆完成。", LoggingLevel.Trace);
        }
    }
}