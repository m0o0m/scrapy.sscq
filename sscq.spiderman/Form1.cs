using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sscq.spiderman
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.tss_host.Text = Dns.GetHostName();
        }

        private IClient client { get; set; }

        private void Form1_Load(object sender, EventArgs e)
        {           
            client = OrleansClient.Instance;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            switch (this.button1.Tag.ToString())
            {
                case "connect":
                    client.Connect();
                    this.button1.Tag = "disConnect";
                    break;
                case "disConnect":
                    client.DisConnect();
                    this.button1.Tag = "connect";
                    break;
            }
        }
    }
}
