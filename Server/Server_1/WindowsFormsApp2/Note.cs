using System;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Note : Form
    {
        private string Smsg;
        DateTime now = DateTime.Now;
        // 메시지 박스에 현재 시간을 표시합니다.
        
        public Note()
        {
            InitializeComponent();
            this.ControlInput.KeyDown += this.ControlInput_KeyDown;
        }
        public void AddText(String text) // textbox 내용-> richbox
        {
            string time = now.ToString("(HH시 mm분)");
            myConsole.AppendText(time+" [공지사항] " + "\n" + text+"\n\n");
            myConsole.Select(myConsole.Text.Length, 0);
            myConsole.ScrollToCaret();
        }
        void ControlEnter() // 보냈으면  Smsg에 저장하고, clear
        {
            Smsg = ControlInput.Text;
            ControlInput.Text = string.Empty;
            ControlInput.Focus();

        }
        void SendSTC()
        {
            string time = now.ToString("(HH시 mm분)");
            foreach (var user in MyChatServer.userData)
            {
                TcpClient client = user.Value as TcpClient;
                NetworkStream stream1 = default(NetworkStream);
                stream1 = client.GetStream();
                byte[] buffer = Encoding.Default.GetBytes(string.Format("({2}) [{0}] {1}", "공지사항", Smsg,time));
                stream1.Write(buffer, 0, buffer.Length);
                stream1.Flush();
            }
        }
        private void metroTile1_Click(object sender, EventArgs e)
        {
        }

        private void metroTile2_Click(object sender, EventArgs e)
        {
            ControlEnter();
            AddText(Smsg);
            SendSTC();
        }

        private void ControlInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return) metroTile2_Click(sender, e);
        }

        private void metroTile3_Click(object sender, EventArgs e)
        {
            myConsole.Clear();
        }

        private void Note_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel= true;
            this.Hide();
        }
    }
}
