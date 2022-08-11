using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerlerBeads;
using RASDK.Arm;
using RASDK.Arm.Type;
using RASDK.Basic;
using RASDK.Basic.Message;
using Hiwin = RASDK.Arm.Hiwin;
using MotionParam = RASDK.Arm.AdditionalMotionParameters;

namespace hiwinrobot_14_creation
{
    public partial class Form1
    {
        private void TestAction()
        {
            _messageHandler.Show("Test action.");
        }

        private void MoveToCalibrate()
        {
            _arm.MoveAbsolute(_calibratePosition);
        }

        private void DoCalibrate()
        {
            _cameraParameter = VisualSystem.CameraCalibration(null, out var error);
            _messageHandler.Log($"Reprojection error: {error}.", LoggingLevel.Info);
        }

        private void Positioning()
        {
            VisualSystem.Positioning(_camera);
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
            CameraConnect();

            var p = Hiwin.Default.DescartesHomePosition;
            p[2] = 230.33;
            _arm.MoveAbsolute(p);

            double kp = (20.0 / 130.0) * 0.8; // mm per pixel * gain.
            var error = VisualSystem.VisualServoing(_arm, _camera, kp, 4, 10);
            _messageHandler.Log($"Visual servoing error: {error.X}, {error.Y}.", LoggingLevel.Info);

            var img = _camera.GetImage();
            img.Save("visual_servoing_done.jpg");

            CameraDisconnect();
        }

        private void CreatePerlerBeads()
        {
            _messageHandler.Log($"開始進行拼豆。", LoggingLevel.Trace);
            _beadsHandler.Run();
            _messageHandler.Log($"拼豆完成。", LoggingLevel.Trace);
        }
    }
}