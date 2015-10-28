using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Orleans;

namespace sscq.server
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ILogger logger = GrainClient.GrainFactory.GetGrain<ILogger>(Guid.NewGuid());
            logger.Infor("");
        }
    }
}
