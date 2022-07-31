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
            _cameraParameter = VisualSystem.CameraCalibration(out var error);
            _messageHandler.Log($"Reprojection error: {error}.", LoggingLevel.Info);
        }

        private void CreatePerlerBeads()
        {
            _messageHandler.Log($"開始進行拼豆。", LoggingLevel.Trace);
            _beadsHandler.Run();
            _messageHandler.Log($"拼豆完成。", LoggingLevel.Trace);
        }
    }
}