using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp2
{
    public static class SetupClient
    {
        public static event EventHandler ConnectedEventHandler = null;
        public static event EventHandler ConnectFailedEventHandler = null;
        static Socket sock;
        public static void Setup(string ip, int port)
        {
            IPAddress ipaddr = IPAddress.Parse(ip);
            IPEndPoint ep = new IPEndPoint(ipaddr, port);
            sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sock.BeginConnect(ep, DoConnect, null);
        }
        static void DoConnect(IAsyncResult result)
        {
            AsyncResult ar = result as AsyncResult;
            try
            {
                sock.EndConnect(result);
                if (ConnectedEventHandler != null)
                {
                    ConnectedEventHandler(null, new EventArgs());
                }
            }
            catch
            {
                if(ConnectFailedEventHandler != null)
                {
                    ConnectFailedEventHandler(null, new EventArgs());
                }
            }
            sock.Close();
        }
        public static void Close()
        {
            if(sock != null)
            {
                sock.Close();
                sock= null;
            }
        }
    }
}
