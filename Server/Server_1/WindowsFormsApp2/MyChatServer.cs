using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Interop;

namespace WindowsFormsApp2
{
    class MyChatServer
    {
        public static Dictionary<string, TcpClient> userData = new Dictionary<string, TcpClient>();
        TcpClient client;
        NetworkStream stream = default(NetworkStream);
        string userID;
        _505 serverForm;
        public MyChatServer(TcpClient client, string userID, _505 serverForm)
        {
            this.userID = userID;
            this.client = client;
            this.serverForm = serverForm;
            userData.Add(userID, client);
            Broadcast(string.Format("{0} joined to server", userID), "Notice");
            Thread refresher = new Thread(Refresh);
            refresher.IsBackground = true;
            refresher.Start();
        }
        void Refresh()
        {
            while (true)
            {
                if (!client.Connected) break;
                if (serverForm.bufferedList != null)
                {
                    byte[] bytes = Encoding.Default.GetBytes(serverForm.bufferedList);
                    stream.Write(bytes, 0, bytes.Length);
                    stream.Flush();
                    Thread.Sleep(5000);
                }
            }

        }
        void Broadcast(string msg, string ID)
        {
            foreach (var user in userData)
            {
                TcpClient client = user.Value as TcpClient;
                stream = client.GetStream();
                byte[] buffer = Encoding.Default.GetBytes(string.Format("[{0}] {1}", ID, msg));
                stream.Write(buffer, 0, buffer.Length);
                stream.Flush();
            }
        }

        public void SendClassName(string classname)
        {
            foreach (var user in MyChatServer.userData)
            {
                TcpClient client = user.Value as TcpClient;
                NetworkStream stream1 = default(NetworkStream);
                stream1 = client.GetStream();
                byte[] buffer = Encoding.Default.GetBytes(string.Format("***/[{0}]{1}", "", "?"+classname));
                string str = Encoding.Default.GetString(buffer);
                Console.WriteLine("111"+str);
                stream1.Write(buffer, 0, buffer.Length);
                stream1.Flush();
            }
        }
        bool Controller(string str)
        {
            bool isControlMsg = true;
            if (str.Equals("/exit"))
            {
                for (int i = 0; i < _505.UserName.Length; i++)
                {
                    if (userID.Equals(_505.UserName[i]))
                    {
                        _505.idx = i;
                        serverForm.PCtiles[i].Text = "PC" + (i + 1);
                        serverForm.UnColorChange(serverForm.PCtiles[i]);
                    }
                }
                serverForm.users.Remove(userID);
                _505.PeopleCheck = true;
                Broadcast(string.Format($"{userID} exited"), "Notice");
                userData.Remove(userID);
            }
            else if (str.Contains(":"))
            {
                string[] ary = str.Split(':');
                int a = Convert.ToInt32(ary[0])-serverForm.CFN; // !!505호에선 152 우리끼린 40
                _505.Pmsg[a] = ary[1]; //45 --> pmsg[5] 프로세스 들어가고 
            }else if (str.Equals("교수님"))
            {
                for (int i = 0; i < _505.UserName.Length; i++)
                {
                    if (userID.Equals(_505.UserName[i]))
                    {                        
                        _505.idx = i;
                        serverForm.tileBList.Add(serverForm.PCtiles[i].Name); //list에 blink 된 학생의 타일 이름을 저장 ex)PC10 
                        serverForm.Handboard.AppendText(serverForm.PCtiles[i].Text + " 학생이 손들었습니다.\n");
                        serverForm.HandupColor(serverForm.PCtiles[i]);

                    }
                }
                //serverForm.users.Remove(userID);
                //_505.PeopleCheck = true;
                //Broadcast(string.Format($"{userID} exited"), "Notice");
                //userData.Remove(userID);
            }
            else
            {
                isControlMsg = false;
            }
            return isControlMsg;
        }
        public void Listen()
        {
            NetworkStream stream = client.GetStream();
            try
            {
                while (true)
                {
                    int bufLength;
                    byte[] buffer = new byte[1024];
                    string str = string.Empty;
                    if (!client.Connected) break;
                    try
                    {
                        while ((bufLength = stream.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            str = Encoding.Default.GetString(buffer, 0, bufLength);
                            if (!Controller(str)) Broadcast(str, userID);
                        }
                    }
                    catch (IOException e)
                    {
                    }

                }
            }
            catch (SocketException e)
            {
                stream.Close();
                client.Close();
            }
            finally
            {
                stream.Close();
                client.Close();
            }


        }

    }
}
