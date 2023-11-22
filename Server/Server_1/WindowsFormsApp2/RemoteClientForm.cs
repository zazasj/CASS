using System;
using System.Drawing;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class RemoteClientForm : Form
    {
        bool check;
        Size csize;
        //private string Username;
        SendEventClient EventSC
        {
            get
            {
                return Controller.Singleton.SendEventClient;
            }
        }
        public RemoteClientForm()
        {
            InitializeComponent();
        }

        //public void displayset(string username)
        //{
        //    this.Username = username;
        //}

        private void RemoteClientForm_Load(object sender, EventArgs e)
        {
            Controller.Singleton.RecvedImage += Singleton_RecvedImage;
            //metroTile1.Location = new Point(1780, 15);
        }

        private void Singleton_RecvedImage(object sender, RecvImageEventArgs e)
        {
            if(check == false)
            {
                Controller.Singleton.StartEventClient();
                check = true;
                csize = e.Image.Size;
            }
            pbox_remote.Image = e.Image;
        }

        private void RemoteClientForm_KeyUp(object sender, KeyEventArgs e)
        {
            if(check == true)
            {
                EventSC.SendKeyUp(e.KeyValue);
            }
        }

        private void RemoteClientForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (check == true)
            {
                EventSC.SendKeyDown(e.KeyValue);
            }
        }

        private void pbox_remote_MouseDown(object sender, MouseEventArgs e)
        {
            if (check == true)
            {
                EventSC.SendMouseDown(e.Button);
            }
        }

        private void pbox_remote_MouseMove(object sender, MouseEventArgs e)
        {
            if (check == true)
            {
                Point pt = ConvertPoint(e.X, e.Y);
                EventSC.SendMouseMove(pt.X,pt.Y);
            }
        }

        private Point ConvertPoint(int x, int y)
        {
            int nx = csize.Width * x / pbox_remote.Width;
            int ny = csize.Height * y / pbox_remote.Height;
            return new Point(nx, ny);
        }

        private void pbox_remote_MouseUp(object sender, MouseEventArgs e)
        {
            if (check == true)
            {
                EventSC.SendMouseUp(e.Button);
            }

        }
        private void RemoteClientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            string EName = _505.SendName;//Username;
            e.Cancel = true; // 폼 닫기 이벤트 취소
            if (MyChatServer.userData.ContainsKey(EName))
            {
                TcpClient targetClient = MyChatServer.userData[EName];
                NetworkStream stream = targetClient.GetStream();
                byte[] buffer = Encoding.Default.GetBytes("**REMOTEEXIT**");// 보낼 메시지 바이트 배열로 변환
                stream.Write(buffer, 0, buffer.Length); // 메시지 전송
            }
            this.Hide(); // 폼 숨기기
        }
        private void RemoteClientForm_FormClosed(object sender, FormClosedEventArgs e)
        {

        }


        private void metroTile1_Click(object sender, EventArgs e)
        {
            
           
        }

    }
}
