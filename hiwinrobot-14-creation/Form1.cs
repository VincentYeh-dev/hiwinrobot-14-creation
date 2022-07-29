using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PerlerBeadsHandler;
using RASDK.Arm;
using RASDK.Basic;
using RASDK.Basic.Message;

namespace hiwinrobot_14_creation
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            var lh = new GeneralLogHandler();
            var mh = new GeneralMessageHandler(lh);
            var arm = new RASDK.Arm.Hiwin.RoboticArm(mh, "192.168.0.1");
            arm.Connect();
            //arm.Homing();
            var ph = new PerlerBeadsHandler.PerlerBeadsHandler(arm, mh);
            ph.Run();
            arm.SetRobotOutput(1, false);
            arm.SetRobotOutput(2, false);
            arm.Disconnect();
        }
    }
}