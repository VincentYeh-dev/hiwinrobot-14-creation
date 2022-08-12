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
using RASDK.Arm.Hiwin;
using RASDK.Arm.Type;
using RASDK.Basic;
using RASDK.Basic.Message;

using Hiwin = RASDK.Arm.Hiwin;

using MotionParam = RASDK.Arm.AdditionalMotionParameters;

namespace hiwinrobot_14_creation
{
    public partial class Form1 : Form
    {
        #region Actions

        private readonly Dictionary<string, Action> _actions = new Dictionary<string, Action>();

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

        private bool DoAction(int index)
        {
            var item = listViewActions.Items[index];
            var action = item.Tag as Action;
            var name = item.SubItems[1].Text;

            _messageHandler.Log($"Do action: {name}, index: {index}.", LoggingLevel.Trace);

            if (name == "ABORT")
            {
                return true;
            }

            action();

            _messageHandler.Log($"Action done: {name}, index: {index}.", LoggingLevel.Trace);
            return false;
        }

        private bool DoAction(int startIndex, int endIndex)
        {
            for (int i = startIndex; i <= endIndex; i++)
            {
                var abort = DoAction(i);
                if (abort)
                {
                    return true;
                }
            }
            return false;
        }

        private void buttonActionRunAll_Click(object sender, EventArgs e)
        {
            var actionsCount = listViewActions.Items.Count;
            DoAction(0, actionsCount - 1);
        }

        private void buttonActionRunSelected_Click(object sender, EventArgs e)
        {
            var selectedItem = listViewActions.SelectedItems;
            foreach (ListViewItem item in selectedItem)
            {
                var abort = DoAction(item.Index);
                if (abort)
                {
                    break;
                }
            }
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

        private readonly RASDK.Vision.IDS.IDSCamera _camera;

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
            _camera = new RASDK.Vision.IDS.IDSCamera(_messageHandler);
            _beadsHandler = new PerlerBeads.PerlerBeadsHandler(_arm, _messageHandler);

            // Init Actions.
            _actions.Clear();
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