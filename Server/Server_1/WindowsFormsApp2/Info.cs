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
        private string ClientIp;
        int sport;
        RemoteClientForm rcf = null;
        display dpy= null;
        
        public Info()
        {
            InitializeComponent();
        }
        
        public void infoset(string clientip)
        {
            this.ClientIp = clientip;
            
        }
        private void Info_Load(object sender, EventArgs e)
        {
            rcf = new RemoteClientForm();
            dpy = new display();
            Remote.Singleton.RecvedRCInfo += Singleton_RecvedRCInfo;
            richTextBox1.ReadOnly= true;
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

        private void timer_send_img_Tick(object sender, EventArgs e)
        {
            Rectangle rect = Remote.Singleton.Rect;
            Bitmap bitmap = new Bitmap(rect.Width, rect.Height);
            Graphics graphics = Graphics.FromImage(bitmap);

            Size size2 = new Size(rect.Width, rect.Height);
            graphics.CopyFromScreen(new Point(0, 0), new Point(0, 0), size2);
            graphics.Dispose();

            try
            {
                ImageClient ic = new ImageClient();
                ic.Connect(sip, NetworkInfo.ImgPort);
                ic.SendImageAsync(bitmap, null);
            }
            catch
            {
                timer_send_img.Stop();
                MessageBox.Show("서버에 문제가 있는 것 같이요");
                this.Close();
            }
        }

        private void noti_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
        }

        private void btn_setting_Click(object sender, EventArgs e)
        {
            try
            {
                string host_ip = ClientIp;
                Rectangle rect = Remote.Singleton.Rect;
                Controller.Singleton.Start(host_ip);
                rcf.ClientSize = new Size(rect.Width - 40, rect.Height - 80);
                rcf.Show();
            }
            catch(Exception ex)
            {
                MessageBox.Show("접속중인 PC가 아닙니다.");
            }
        }

        private void OnlyDisplay_Click(object sender, EventArgs e)
        {
            try
            {
                string host_ip = ClientIp;
                Console.WriteLine(host_ip);
                Rectangle rect = Remote.Singleton.Rect;
                Controller.Singleton.Start(host_ip);
                dpy.ClientSize = new Size(rect.Width - 40, rect.Height - 80);
                dpy.Show();
            }catch(Exception ex)
            {
                MessageBox.Show("접속중인 PC가 아닙니다.");
            }
        }

        private void Info_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            rcf.Close();
            richTextBox1.Clear();
            this.Hide();
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
            ((Button)sender).FlatAppearance.BorderSize = 0;
            ((Button)sender).BackColor = color1;
            ((Button)sender).ForeColor = Color.White;

        }
        private void ButtonColorUnChange(object sender)
        {
            ((Button)sender).FlatAppearance.BorderSize = 1;
            ((Button)sender).BackColor = Color.White;
            ((Button)sender).ForeColor = Color.Black;

        }
    }
}
