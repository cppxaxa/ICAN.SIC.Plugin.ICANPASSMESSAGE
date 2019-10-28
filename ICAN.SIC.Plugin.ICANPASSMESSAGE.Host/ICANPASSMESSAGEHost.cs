using ICAN.SIC.Abstractions.ConcreteClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ICAN.SIC.Plugin.ICANPASSMESSAGE.Host
{
    public partial class ICANPASSMESSAGEHost : Form
    {
        ICANPASSMESSAGE controller;

        public ICANPASSMESSAGEHost()
        {
            InitializeComponent();

            controller = new ICANPASSMESSAGE();
        }

        private void btnPostMachine_Click(object sender, EventArgs e)
        {
            controller.Hub.Publish(new MachineMessage(txtMessage.Text));
        }
    }
}
