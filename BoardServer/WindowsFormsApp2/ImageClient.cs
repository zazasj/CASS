using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace WindowsFormsApp2
{
    public class ImageClient
    {
        Socket sock;
        public void Connect(string ip, int port)
        {

            sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);


            IPAddress ipaddr = IPAddress.Parse(ip);
            IPEndPoint ep = new IPEndPoint(ipaddr, port);
            sock.Connect(ep);
        }

        public bool SendImage(Image img)
        {
            if(sock == null)
            {
                return false;
            }
            MemoryStream ms = new MemoryStream();
            img.Save(ms, ImageFormat.Jpeg);
            byte[] data = ms.GetBuffer();
            try
            {
                int trans = 0;
                byte[] lbuf = BitConverter.GetBytes(data.Length);
                sock.Send(lbuf); //전송할 이미지 길이를 먼저 전송

                while(trans < data.Length)
                {
                    trans += sock.Send(data, trans, data.Length - trans, SocketFlags.None);
                }
                sock.Close();
                sock = null;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public void SendImageAsync(Image img, AsyncCallback callback)
        {
            SendImageDele dele = SendImage;
            dele.BeginInvoke(img, callback, this);
        }
        public void Close()
        {
            if(sock != null)
            {
                sock.Close();
                sock = null;
            }
        }
    }
    public delegate bool SendImageDele(Image img);
}
