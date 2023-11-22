using MetroFramework.Controls;
using Microsoft.Office.Interop.Excel;
using RCEventArgsLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp2.Properties;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using Excel = Microsoft.Office.Interop.Excel;

namespace WindowsFormsApp2
{
    public partial class _505 : Form
    {

        TcpClient client = new TcpClient();
        public string bufferedList;
        public List<string> users = new List<string>();
        public static int[] userIp = new int[60];
        public static string SendName;
        public bool isBlinking = false;
        public static string[] UserName = new string[60];
        public static string[] Pmsg = new string[60];
        public static string[] ClientIP = new string[60];
        protected string IPBatch;
        private string classname;
        public static int idx = 0;
        private string serverip;
        public int ipNum;
        public int CFN = 0;
        TcpListener server;
        string Chat_Open = "/open ";
        const string Chat_Close = "/close";
        Properties.Settings settings = Properties.Settings.Default;
        public MetroTile[] PCtiles = new MetroTile[54];
        Note note = new Note();
        IpChange ipchange = new IpChange();
        Form1 frm1 = new Form1();
        string sip;
        bool check = false;
        public static bool PeopleCheck = false;
        Color color1 = Color.FromArgb(76,76,76);
        public List<string> tileBList = new List<string>();



    public _505()
        {
            InitializeComponent();
            this.Resize += _505_Resize;
            panel1.Width = ClientSize.Width;
            label2.Location = new System.Drawing.Point(ClientSize.Width / 2 - label1.Width / 2, label1.Location.Y);
            Join.BorderStyle = BorderStyle.None;

        }

        public void SetClassname(string cname)
        {
            this.classname = cname;
            settings.ClassName = cname;
            settings.Save();
        }
        private void SetClassLabel(string name)
        {
            label2.Text = name;
        }

        private void ExitProcess()
        {
            Process[] processes = Process.GetProcessesByName("CASS");
            foreach (Process process in processes)
            {
                process.Kill();
            }
        }


        void RefreshChatters()
        //채팅 참가자 리스트를 리스트박스에 실시간으로 업데이트합니다. (1초간격) 클라이언트.
        {
            CheckForIllegalCrossThreadCalls = false;
            while (true)
            {
                bufferedList = "**userlist** ";

                if (PeopleCheck == true)
                {
                    Join.Items.Clear();
                    foreach (string user in users)
                    {

                        Join.Items.Add(user);
                        bufferedList += (user + " ");
                    }
                    Join.Sorted = true;
                    PeopleCheck = false;
                }

                Thread.Sleep(5000);
            }
        }

        void OpenServer(object s)
        //
        {
            string serverIP = s.ToString().Substring("/open ".Length);
            
            const int Port = 10203;
            IPEndPoint serverAddr = new IPEndPoint(IPAddress.Parse(serverIP), Port);
            server = new TcpListener(serverAddr);
            try
            {
                server.Start();
            }
            catch (SocketException e)
            {
                //MessageBox.Show("서버ip가 일치하지않습니다.");
                MessageBox.Show("서버ip가 일치하지않습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                // 사용자가 확인 버튼을 클릭한 후 폼을 활성화
                this.WindowState = FormWindowState.Normal;
                this.Activate();
                return;
            }
            //AddText(String.Format("Server Opened. [{0}]\r\n", serverAddr.ToString()));
            while (true)
            {
                try
                {
                    client = server.AcceptTcpClient();
                    NetworkStream stream = client.GetStream();
                    byte[] buffer = new byte[1024];
                    int bytes = stream.Read(buffer, 0, buffer.Length);
                    string userID = Encoding.Default.GetString(buffer, 0, bytes);

                    //클라이언트 접속시 DB와 비교하기 위해 수업명 전송
                    //byte[] buffer2 = Encoding.Default.GetBytes("*/" + classname);// 보낼 메시지 바이트 배열로 변환
                    //stream.Write(buffer2, 0, buffer2.Length); // 메시지 전송
                    //stream.Flush();

                    users.Add(userID);
                    PeopleCheck= true;
                    string[] StudentName = userID.Split('_');
                    MyChatServer mychat = new MyChatServer(client, userID, this);
                    Thread serverThread = new Thread(new ThreadStart(mychat.Listen));
                    serverThread.IsBackground = true;
                    serverThread.Start();
                    //밑 3개줄로 ip 변수로 관련해서 상호작용 가능
                    Socket c = client.Client;
                    IPEndPoint ip_point = (IPEndPoint)c.RemoteEndPoint;
                    //ex) ip : 172.18.4.40  ex) split_ip[0] : 172 / [1] : 18 / [2] : 4 / [3](=ipNum) : 40
                    string ip = ip_point.Address.ToString();
                    string[] split_ip = ip.Split('.');
                    ipNum = Convert.ToInt32(split_ip[3]);
                    string ip505 = split_ip[0]+split_ip[1]+split_ip[2]; 

                    //505호 IP 설정
                    //!!우리가 쓸땐 40, 505호에서는 152
                    if (ipNum >= CFN & ip505.Equals("172184")) 
                    {
                        Console.WriteLine("****접속*****");
                        ClientIP[ipNum- CFN] = ip_point.Address.ToString(); //userIp[5] = 172.18.4.45 
                        PCtiles[ipNum - CFN].Text = StudentName[1];
                        ColorChange(PCtiles[ipNum - CFN]);
                        UserName[ipNum- CFN] = userID;
                        mychat.SendClassName(settings.ClassName);

                    }
                    
                }
                catch (Exception ex) { break; }
            }
            client.Close();
            server.Stop();
        }
        void CloseServer()
        {
            Controller1(Chat_Close);

        }
        void Controller1(string s)
        {
            Thread open = new Thread(OpenServer);
            open.IsBackground = true;
            if (s.Equals(string.Empty)) return;
            
            else if (s.StartsWith("/open "))
            {
                open.Start(s);
            }
            else if (s.Equals("/close"))
            {
                open.Abort();
                //AddText("[Server] Closed.\r\n");
            }
        }
        

        private void PC_Click(object sender, EventArgs e)
        {
            string k,n;
            int m;
            n = ((MetroTile)sender).Name; // PC6
            m = Convert.ToInt32(n.Substring(2));//6
            Info info = new Info();
            if (UserName[m-1] != null) 
            {
                k = UserName[m-1];//학번_이름
                SendName = k;
                isBlinking = false;
                info.infoset(ClientIP[m - 1]);
                info.label1.Text = k;
                //  45--> pmsg[5] ; 40 --> pmsg[0] pc6누르면 pmsg[5] pc1 누르면 pmsg[0]
                if (k.Equals(UserName[m - 1]))
                {
                    info.richTextBox1.Text = Pmsg[m - 1];
                }
                //특정 클라이언트에 메세지 보내기
                
            }
            else
            {
                info.infoset(ClientIP[m - 1]);
                info.label1.Text = ((MetroTile)sender).Text;
            }


            info.Show();


        }
        private void ColorChange(object sender)
        {
            ((MetroTile)sender).BackColor = Color.SpringGreen;
            ((MetroTile)sender).ForeColor= Color.Black;
        }
        public void UnColorChange(object sender)
        {
            ((MetroTile)sender).BackColor = Color.FromArgb(47, 49, 62);
            ((MetroTile)sender).ForeColor = Color.White;
        }
        public void HandupColor(object sender)
        {
            if (isBlinking) return;
            Color originalBackColor = ((MetroTile)sender).BackColor;
            Color blinkBackColor = Color.FromArgb(245, 192, 86); // 깜빡이게 될 색깔 설정
            int duration = 1000; // 깜빡이는 시간 설정 (밀리초 단위)
            isBlinking = true;

            // 새로운 스레드에서 깜빡이는 작업을 수행합니다.
            Task.Run(() =>
            {
                while (isBlinking)
                {
                    ((MetroTile)sender).BackColor = blinkBackColor;
                    Thread.Sleep(duration / 2);
                    ((MetroTile)sender).BackColor = originalBackColor;
                    Thread.Sleep(duration / 2);

                }
            });
        }

        private void ListSave_Click(object sender, EventArgs e)
        {
            // 현재 시간을 가져옵니다.
            DateTime now = DateTime.Now;
            // 메시지 박스에 현재 시간을 표시합니다.
            string today = classname + now.ToString(" M월 dd일 ");
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel Files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            saveFileDialog.Title = "Save Excel File";
            saveFileDialog.FileName = today + "출석부.xlsx";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Excel.Application excelApp = new Excel.Application();
                Excel.Workbook workbook = excelApp.Workbooks.Add(Type.Missing);
                Excel.Worksheet worksheet = null;
                worksheet = workbook.Sheets["Sheet1"];
                worksheet = workbook.ActiveSheet;
                worksheet.Columns[2].ColumnWidth = 30;
                worksheet.Range["B:B"].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                
                

                int row = 2;
                worksheet.Cells[row, 2] = today.ToString();
                Excel.Range range = worksheet.Cells[row, 2];
                Excel.Borders firstborder = range.Borders;
                firstborder.LineStyle = Excel.XlLineStyle.xlContinuous;
                firstborder.Weight = 2d;
                row++;

                foreach (var item in Join.Items)
                {
                    
                    

                    Excel.Range range2 = worksheet.Cells[row, 2];
                    range2.Value = item.ToString();
                    //테두리 추가
                    Excel.Borders borders = range2.Borders;
                    borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                    borders.Weight = 2d;
                    row++;
                }
                workbook.SaveAs(saveFileDialog.FileName);
                workbook.Close();
                excelApp.Quit();
                MessageBox.Show("저장이 완료되었습니다.");
            }

        }

        //공지사항전달
        private void NoteSend_Click(object sender, EventArgs e)
        {
            note.ShowDialog();
        }

        private void _505_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                settings.Reload();
                SetClassLabel(settings.ClassName);
               serverip = settings.ServerIP;
                CFN = Convert.ToInt32(settings.ClientFirstNum);
                Thread refresher = new Thread(RefreshChatters);
                refresher.IsBackground = true;
                refresher.Start();
                if (serverip != "") {
                    Controller1(Chat_Open + serverip);
                }
                Remote.Singleton.RecvedRCInfo += Singleton_RecvedRCInfo;
                for (int i = 0; i < PCtiles.Length; i++)
                {
                    PCtiles[i] = this.Controls.Find("PC" + (i + 1), true).FirstOrDefault() as MetroTile;

                }

            }
        }


        private void metroTile3_Click(object sender, EventArgs e)
        {
            this.Hide();
            client.Close();
            CloseServer();
            server.Stop();
            //Start start = new Start();
            //start.Show();
            ExitProcess();
        }

        private void metroTile5_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
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
                check = true;
                if (check == true)
                {
                    Remote.Singleton.RecvEventStart();
                    timer_send_img.Start();
                }
            }
        }

        private void timer_send_img_Tick(object sender, EventArgs e)
        {
            System.Drawing.Rectangle rect = Remote.Singleton.Rect;
            Bitmap bitmap = new Bitmap(rect.Width, rect.Height);
            Graphics graphics = Graphics.FromImage(bitmap);

            Size size2 = new Size(rect.Width, rect.Height);
            graphics.CopyFromScreen(new System.Drawing.Point(0, 0), new System.Drawing.Point(0, 0), size2);
            graphics.Dispose();

            try
            {
                ImageClient ic = new ImageClient();
                ic.Connect(sip, NetworkInfo.ImgPort);
                ic.SendImageAsync(bitmap, null);
            }
            catch
            {
                //timer_send_img.Enabled = false;
                SetupClient.Close();
                timer_send_img.Stop();
                check= false;
            }
        }

        private void _505_FormClosing(object sender, FormClosingEventArgs e)
        {
            ExitProcess();
        }


        private System.Drawing.Point mouseOffset;
        private void _505_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseOffset = new System.Drawing.Point(-e.X, -e.Y);
            }
        }

        private void _505_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                System.Drawing.Point mousePos = Control.MousePosition;
                mousePos.Offset(mouseOffset.X, mouseOffset.Y);
                Location = mousePos;
            }
        }

        private void _505_MouseUp(object sender, MouseEventArgs e)
        {
            mouseOffset = System.Drawing.Point.Empty;
        }


        //파일전송버튼
        private void FTP_Click(object sender, EventArgs e)
        {
            frm1.Show();
        }

        private void Join_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedname;
            selectedname = Join.SelectedItem.ToString(); ; // 20180663_전세진

            for (int i = 0; i < PCtiles.Length; i++)
            {
                if (UserName[i] == selectedname)
                { 
                    PCtiles[i].PerformClick();
                }
            }
        }
        private void IPchange_Click(object sender, EventArgs e)
        {
            this.Hide();
            server.Stop();
            ipchange.Show();
        }

        private void _505_Resize(object sender, EventArgs e)
        {
            //최대화 레이아웃 변경 코드
            if (WindowState == FormWindowState.Maximized)
            {
                // TextBox 가로 길이를 폼에 꽉 차게 설정
                panel1.Width = ClientSize.Width;

                // Label 위치 조정
                label2.Location = new System.Drawing.Point(ClientSize.Width / 2 - label1.Width / 2, label1.Location.Y);
            }
            else if (WindowState == FormWindowState.Normal)
            { 
            panel1.Width = ClientSize.Width;
            label2.Location = new System.Drawing.Point(ClientSize.Width / 2 - label1.Width / 2, label1.Location.Y);
            }
        }

        private void AllCheck_Click(object sender, EventArgs e)
        {
            Handboard.Clear();
            foreach (string tile in tileBList)
            {
                MetroTile Btile = this.Controls.Find(tile, true).FirstOrDefault() as MetroTile; //Btile 이라는데에 blink되는 타일 이름저장 ex)PC1
                ColorChange(Btile);
            }
            tileBList.Clear();
            isBlinking = false;

            //foreach (MetroTile tile in tileList)
            //{
            //    Console.WriteLine(tile.Name);
            //}

        }
    }
}
