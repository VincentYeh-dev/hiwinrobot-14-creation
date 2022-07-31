using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PerlerBeads;
using RASDK.Arm;
using RASDK.Arm.Type;
using RASDK.Basic;
using RASDK.Basic.Message;
using Hiwin = RASDK.Arm.Hiwin;
using MotionParam = RASDK.Arm.AdditionalMotionParameters;

namespace hiwinrobot_14_creation
{
    public partial class Form1 : Form
    {
        #region config

        private readonly string _armIp = "192.168.0.1";

        #endregion config

        #region Positions

        private double[] _testPosition => new double[] { };
        private double[] _calibratePosition => new double[] { };

        #endregion Positions

        #region Actions

        private readonly Dictionary<string, Action> _actions = new Dictionary<string, Action>();

        private void OrganizeActions()
        {
            _actions.Clear();
            _actions.Add("Test", TestAction);
            _actions.Add("移動到相機標定的位置", MoveToCalibrate);
            _actions.Add("進行相機標定", DoCalibrate);
            _actions.Add("進行拼豆", CreatePerlerBeads);
            _actions.Add("End", () => { _messageHandler.Show("所有動作已結束。"); });
        }

        private void UpdateActionsListView()
        {
            var names = _actions.Keys.ToList();
            var actions = _actions.Values.ToList();

            listViewActions.Items.Clear();
            for (int i = 0; i < _actions.Count; i++)
            {
                var id = i + 1;
                var item = new ListViewItem(id.ToString());
                item.SubItems.Add(names[i]);
                item.Tag = actions[i];

                listViewActions.Items.Add(item);
            }
        }

        private void DoAction(int index)
        {
            var action = listViewActions.Items[index].Tag as Action;
            action();
        }

        private void DoAction(int startIndex, int endIndex)
        {
            for (int i = startIndex; i <= endIndex; i++)
            {
                DoAction(i);
            }
        }

        private void buttonActionRunAll_Click(object sender, EventArgs e)
        {
            var actionsCount = listViewActions.Items.Count;
            DoAction(0, actionsCount - 1);
        }

        private void buttonActionRunFromHere_Click(object sender, EventArgs e)
        {
            var startIndex = listViewActions.SelectedItems[0].Index;
            var actionsCount = listViewActions.Items.Count;
            DoAction(startIndex, actionsCount - 1);
        }

        private void buttonActionDoOnce_Click(object sender, EventArgs e)
        {
            var selectedIndex = listViewActions.SelectedItems[0].Index;
            DoAction(selectedIndex);
        }

        #endregion Actions

        #region Connection

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            _arm.Connect();
        }

        private void buttonDisconnect_Click(object sender, EventArgs e)
        {
            _arm.Disconnect();
        }

        #endregion Connection

        #region Form

        private readonly Hiwin.RoboticArm _arm;

        private readonly MessageHandler _messageHandler;

        private readonly PerlerBeadsHandler _beadsHandler;

        private RASDK.Vision.CameraParameter _cameraParameter;

        public Form1()
        {
            InitializeComponent();

            // 實體化。
            var logHandler = new GeneralLogHandler();
            _messageHandler = new GeneralMessageHandler(logHandler);
            _arm = new Hiwin.RoboticArm(_messageHandler, _armIp);
            _beadsHandler = new PerlerBeads.PerlerBeadsHandler(_arm, _messageHandler);

            // Init Actions.
            OrganizeActions();
            UpdateActionsListView();
        }

        ~Form1()
        {
            try
            {
                _arm.Disconnect();
            }
            catch
            { /* Do nothing. */ }
        }

        #endregion Form
    }
}