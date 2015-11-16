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
                    this.button1.Text = "断开";
                    this.textBox1.Enabled = false;
                    this.timer1.Enabled = true;
                    break;
                case "disConnect":
                    client.DisConnect();
                    this.button1.Tag = "connect";
                    this.button1.Text = "连接";
                    this.textBox1.Enabled = true;
                    this.timer1.Enabled = false;
                    break;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (client.KeepAlived().Result)
            {
                if (this.notifyIcon1.Tag.ToString() == "red")
                {
                    this.notifyIcon1.Icon = Properties.Resources.Spiderman_Green48;
                    this.notifyIcon1.Tag = "green";
                    return;
                }
            }
            this.notifyIcon1.Icon = Properties.Resources.Spiderman48;
            this.notifyIcon1.Tag = "red";
        }
    }
}
