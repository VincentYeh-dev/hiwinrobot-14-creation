using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using RASDK.Basic;
using RASDK.Basic.Message;
using RASDK.Arm.Type;
using Hiwin = RASDK.Arm.Hiwin;
using MotionParam = RASDK.Arm.AdditionalMotionParameters;
using Motion = RASDK.Arm.RoboticArmMotion;

namespace PerlerBeads
{
    public class PerlerBeadsHandler
    {
        /// <summary>
        /// 樣板。
        /// </summary>
        private readonly Pegboard _modelBoard;

        /// <summary>
        /// 儲存。
        /// </summary>
        private readonly Pegboard _storeBoard;

        /// <summary>
        /// 目標。
        /// </summary>
        private readonly Pegboard _goalBoard;

        private readonly Hiwin.RoboticArm _arm;
        private readonly double _pickUpperZ = -40;
        private readonly double _pickLowerZ=-55.5;

        private readonly double _putDownZ = -10;

        private readonly MessageHandler _messageHanlder;

        private readonly MotionParam _defauleMotionParam = new MotionParam()
        {
            CoordinateType = CoordinateType.Descartes,
            NeedWait = true
        };

        public PerlerBeadsHandler(Hiwin.RoboticArm arm, MessageHandler messageHandler)
        {
            _modelBoard = PegBoardFactory.Make("pegboard.csv", out var info);
            Console.WriteLine(info);

            _goalBoard = new Pegboard
            {
                Position = new PointF(-131.480f, 600.619f)
            };

            _storeBoard = PegBoardFactory.Make("storeboard.csv",out _);
            _storeBoard.Position = _goalBoard.Position;
            _storeBoard.Position.X += Pegboard.X_GridLength* (_goalBoard.Size.Width);

            _arm = arm;
            _messageHanlder = messageHandler;
        }

        public void Run()
        {
            var dict=_modelBoard.BeadColorCount();
            CloseGripper();
            OpenGripper();

            for (int x = 0; x < _modelBoard.Size.Width; x++)
            {
                for (int y = 0; y < _modelBoard.Size.Height; y++)
                {
                    var skip = HandleTheBead(new Point(x, y));
                    if (skip)
                    {
                        _messageHanlder.Log($"Bead {x}, {y} skip.", LoggingLevel.Info);
                    }
                }
            }
            DisposGripper();
        }

        public void CheckGoalYGridLength()
        {
            CloseGripper();
            for (int y = 0; y < _goalBoard.Size.Height; y++)
            {

                var position = _goalBoard.GetRealPosition(new Point(0, y));
                var goalPosition = Hiwin.Default.DescartesHomePosition.Clone() as double[];
                goalPosition[0] = position.X;
                goalPosition[1] = position.Y;
                goalPosition[2] = -55;
                _arm.MoveAbsolute(goalPosition, _defauleMotionParam);
                Thread.Sleep(1000);
            }
        }

        public void CheckGoalXGridLength()
        {
            CloseGripper();
            for (int x = 0; x < _goalBoard.Size.Width; x++)
            {
                var position = _goalBoard.GetRealPosition(new Point(x, 0));
                var goalPosition = Hiwin.Default.DescartesHomePosition.Clone() as double[];
                goalPosition[0] = position.X;
                goalPosition[1] = position.Y;
                goalPosition[2] = -55;
                _arm.MoveAbsolute(goalPosition, _defauleMotionParam);
                Thread.Sleep(1000);
            }
        }
        public void CheckStoreYGridLength()
        {
            CloseGripper();
            for (int y = 0; y < _storeBoard.Size.Height; y++)
            {

                var position = _storeBoard.GetRealPosition(new Point(0, y));
                var goalPosition = Hiwin.Default.DescartesHomePosition.Clone() as double[];
                goalPosition[0] = position.X;
                goalPosition[1] = position.Y;
                goalPosition[2] = -55;
                _arm.MoveAbsolute(goalPosition, _defauleMotionParam);
                Thread.Sleep(1000);
            }
        }

        public void CheckStoreXGridLength()
        {
            CloseGripper();
            for (int x = 0; x < _storeBoard.Size.Width; x++)
            {
                var position = _storeBoard.GetRealPosition(new Point(x, 0));
                var goalPosition = Hiwin.Default.DescartesHomePosition.Clone() as double[];
                goalPosition[0] = position.X;
                goalPosition[1] = position.Y;
                goalPosition[2] = -55;
                _arm.MoveAbsolute(goalPosition, _defauleMotionParam);
                Thread.Sleep(1000);
            }
        }

        private bool HandleTheBead(Point modelGrid)
        {
            var modelBead = _modelBoard.GetBead(modelGrid);
            if (modelBead == null)
            {
                // 不用放。Skip
                return true;
            }

            var find = _storeBoard.SearchBy(modelBead.Color, out var storeGrid);
            if (!find)
            {
                // 找不到目標顏色的珠子。Skip
                return true;
            }

            //var r = _messageHanlder.Show($"將要抓放珠子-{modelBead.Color.Name}。", "Info", MessageBoxButtons.OKCancel, MessageBoxIcon.None, LoggingLevel.Info);
            //if (r != DialogResult.OK)
            //{
            //    OpenGripper();
            //    DisposGripper();
            //    _arm.Disconnect();
            //    throw new Exception("Abort.");
            //}

            _messageHanlder.Log($"執行：{modelGrid.X},{modelGrid.Y};{modelBead.Color.Name}。", LoggingLevel.Trace);

            //TakeBeadTest(); // For testing.
            TakeBead(storeGrid);
            _storeBoard.RemoveBead(storeGrid);

            PutBead(modelGrid);
            _goalBoard.PutBead(modelGrid, modelBead, true);

            _messageHanlder.Log($"完成：{modelGrid.X},{modelGrid.Y};{modelBead.Color.Name}。", LoggingLevel.Trace);
            return false;
        }

        private void TakeBeadTest()
        {
            OpenGripper();
            var goalPosition = Hiwin.Default.DescartesHomePosition;
            goalPosition[0] = 154.246; 
            goalPosition[1] = 600.619; 
            goalPosition[2] = _pickUpperZ+40; 
            _arm.MoveAbsolute(goalPosition, _defauleMotionParam);

            //_messageHanlder.Show("Pick a bead A.");

            goalPosition[2] = -57; 
            _arm.MoveAbsolute(goalPosition, _defauleMotionParam);
            CloseGripper();
            goalPosition[2] = _pickUpperZ+40; 
            _arm.MoveAbsolute(goalPosition, _defauleMotionParam);
            //_messageHanlder.Show("Pick a bead B.");
        }

        private void TakeBead(Point grid)
        {
            var beadRealPos = _storeBoard.GetRealPosition(grid);

            var goalPosition = (double[])Hiwin.Default.DescartesHomePosition.Clone();
            goalPosition[0] = beadRealPos.X;
            goalPosition[1] = beadRealPos.Y;
            goalPosition[2] = _pickUpperZ;

            // Move to upper.
            OpenGripper();
            _arm.MoveAbsolute(goalPosition, _defauleMotionParam);

            goalPosition[2] = _pickLowerZ;
            _arm.MoveAbsolute(goalPosition, _defauleMotionParam);
            CloseGripper();

            goalPosition[2] = _pickUpperZ;
            // Back to upper.
            _arm.MoveAbsolute(goalPosition, _defauleMotionParam);
        }

        private void PutBead(Point grid)
        {
            var beadRealPos = _goalBoard.GetRealPosition(grid);

            var goalPosition = (double[])Hiwin.Default.DescartesHomePosition.Clone();
            goalPosition[0] = beadRealPos.X;
            goalPosition[1] = beadRealPos.Y;
            goalPosition[2] = _pickUpperZ;

            // Move to upper.
            _arm.MoveAbsolute(goalPosition, _defauleMotionParam);

            // Move to lower.
            //_messageHanlder.Show("Downing.");
            var mp = new MotionParam
            {
                CoordinateType = CoordinateType.Descartes,
                MotionType = MotionType.Linear,
                NeedWait = true
            };
            _arm.MoveRelative(0, 0, _putDownZ, 0, 0, 0, mp);
            OpenGripper();

            // Back to upper.
            //_messageHanlder.Show("Upping.");
            _arm.MoveRelative(0, 0, -_putDownZ, 0, 0, 0, mp);
        }

        private void CloseGripper()
        {
            // R1:on, R2:on => close.
            _arm.SetRobotOutput(1, true);
            _arm.SetRobotOutput(2, true);

            Thread.Sleep(100);
            //while (!_arm.GetRobotInput(1))
            { /* Wait for action is starting. */ }

            while (_arm.GetRobotInput(1))
            { /* Wait for action is completion. */ }
        }

        private void OpenGripper()
        {
            // R1:on, R2:off => open.
            _arm.SetRobotOutput(1, true);
            _arm.SetRobotOutput(2, false);

            Thread.Sleep(100);
            //while (!_arm.GetRobotInput(1))
            { /* Wait for action is starting. */ }

            while (_arm.GetRobotInput(1))
            { /* Wait for action is completion. */ }
        }

        private void DisposGripper()
        {
            _arm.SetRobotOutput(1, false);
            _arm.SetRobotOutput(2, false);
        }
    }
}