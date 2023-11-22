using MetroFramework.Controls;
using MetroFramework.Drawing.Html;
using RCEventArgsLib;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Info : Form
    {
        Color color1 = Color.FromArgb(38, 58, 119);
        string sip;
        private string ClientIp = "172.18.4.50";
        int sport;
        display dpy= null;
        Properties.Settings settings = new Properties.Settings();
        
        public Info()
        {
            InitializeComponent();
        }

        private void Info_Load(object sender, EventArgs e)
        {
            dpy = new display();
            Remote.Singleton.RecvedRCInfo += Singleton_RecvedRCInfo;
        }
        delegate void Remote_Dele(object sender, RecvRCInfoEventArgs e);
        private void Singleton_RecvedRCInfo(object sender, RecvRCInfoEventArgs e)
        {
            if (this.InvokeRequired)
            {
                object[] objs = new object[2] { sender, e };
                this.Invoke(new Remote_Dele(Singleton_RecvedRCInfo), objs);
            }
            else
            {
                //tbox_controller_ip.Text = e.IPAddressStr;
                sip = e.IPAddressStr;
                sport = e.Port;
                //btn_ok.Enabled = true;
            }
        }

        private void SetupServer_RecvedRCInfoEventHandler(object sender, RCEventArgsLib.RecvRCInfoEventArgs e)
        {

        }

        private void metroTile2_Click(object sender, EventArgs e)
        {
            string host_ip = ClientIp;
            Rectangle rect = Remote.Singleton.Rect;
            Controller.Singleton.Start(host_ip);
            dpy.ClientSize = new Size(rect.Width - 40, rect.Height - 80);
            dpy.Show();

            
        }

        private void Info_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                settings.Reload();
                ClientIp = settings.ServerIP;
            }
        }
        private void button_MouseEnter(object sender, EventArgs e)
        {
            ButtonColorChange(sender);
        }

        private void button_MouseLeave(object sender, EventArgs e)
        {
            ButtonColorUnChange(sender);
        }

        private void ButtonColorChange(object sender)
        {
            ((System.Windows.Forms.Button)sender).FlatAppearance.BorderSize = 0;
            ((System.Windows.Forms.Button)sender).BackColor = color1;
            ((System.Windows.Forms.Button)sender).ForeColor = Color.White;

        }
        private void ButtonColorUnChange(object sender)
        {
            ((System.Windows.Forms.Button)sender).FlatAppearance.BorderSize = 1;
            ((System.Windows.Forms.Button)sender).BackColor = Color.White;
            ((System.Windows.Forms.Button)sender).ForeColor = Color.Black;

        }

        private void Info_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel= true;
            this.WindowState = FormWindowState.Minimized;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            IpChange ipc = new IpChange();
            ipc.Show();
        }
    }
}
