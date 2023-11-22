using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        private TcpListener server;
        private string fname;
        Color color1 = Color.FromArgb(38, 58, 119);
        public Form1()
        {
            InitializeComponent();
            //flowLayoutPanel1.Location = new Point(3, 3);
            //flowLayoutPanel2.Location = new Point(5, 5);
            //this.Controls.SetChildIndex(flowLayoutPanel1, 10);
        }

        
        private async void ProcessClient(TcpClient client)
        {
            try
            {
                using (client)
                using (var stream = client.GetStream())
                {
                    byte[] fileNameLengthBytes = new byte[sizeof(int)];
                    await stream.ReadAsync(fileNameLengthBytes, 0, fileNameLengthBytes.Length);
                    int fileNameLength = BitConverter.ToInt32(fileNameLengthBytes, 0);

                    byte[] fileNameBytes = new byte[fileNameLength];
                    await stream.ReadAsync(fileNameBytes, 0, fileNameBytes.Length);
                    string fileName = Encoding.UTF8.GetString(fileNameBytes);

                    using (var fileStream = File.Create(fname + fileName))
                    {
                        await stream.CopyToAsync(fileStream);
                    }
                }
                MessageBox.Show("파일 전송이 완료되었습니다.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("파일 전송에 실패했습니다. 오류 메시지: " + ex.Message);
            }
        }


        private void metroTile6_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void metroTile5_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private Point mouseOffset;
        private void flowLayoutPanel2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseOffset = new Point(-e.X, -e.Y);
            }
        }

        private void flowLayoutPanel2_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point mousePos = Control.MousePosition;
                mousePos.Offset(mouseOffset.X, mouseOffset.Y);
                Location = mousePos;
            }
        }

        private void flowLayoutPanel2_MouseUp(object sender, MouseEventArgs e)
        {
            mouseOffset = Point.Empty;
        }

        private void button_MouseEnter(object sender, EventArgs e)
        {
            ButtonColorChange(sender);
        }

        private void button_MouseLeave(object sender, EventArgs e)
        {
            ButtonColorUnChange(sender);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //파일 저장 위치 설정
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                fname = folderBrowserDialog.SelectedPath + "\\";
                if (fname.Length > 40)
                {
                    string newText = fname.Substring(0, 40) + "\n" + fname.Substring(40);
                    label2.Text = newText;
                }
                else { label2.Text = fname; }
            }
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

        private async void button2_Click(object sender, EventArgs e)
        {
            server = new TcpListener(IPAddress.Any, 10204); //!! 포트번호 확인
            server.Start();
            MessageBox.Show("서버가 시작되었습니다.");
            label4.Text = "파일 다운로드 가능";
            try
            {

                while (true)
                {

                    TcpClient client = await server.AcceptTcpClientAsync();
                    ProcessClient(client);

                }
            }
            catch (Exception ex)
            {
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (server != null)
            {
                server.Stop();
                server = null;
                MessageBox.Show("서버가 정지되었습니다.");
                label4.Text = "파일 다운로드 불가능";
            }
        }
    }
}
