using System.Net;
using System.Net.Sockets;

namespace WindowsFormsApp2
{
    public class Controller
    {
        static Controller singleton;
        public static Controller Singleton
        {
            get
            {
                return singleton;
            }
        }
        static Controller()
        {
            singleton = new Controller();
        }
        private Controller()
        {

        }
        ImageServer img_sever = null;
        SendEventClient sce = null;
        public event RecvImageEventHandler RecvedImage = null;
        string host_ip;
        public SendEventClient SendEventClient
        {
            get
            {
                return sce;
            }
        }

        public string MyIP
        {
            get
            {
                string host_name = Dns.GetHostName();
                IPHostEntry host_entry = Dns.GetHostEntry(host_name);

                foreach (IPAddress ipaddr in host_entry.AddressList)
                {
                    if (ipaddr.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return ipaddr.ToString();
                    }
                }
                return string.Empty;
            }
        }


        public void Start(string host_ip)
        {
            this.host_ip = host_ip;
            img_sever = new ImageServer(MyIP, NetworkInfo.ImgPort);
            img_sever.RecvedImage += img_sever_RecvedImage;
            SetupClient.Setup(host_ip, NetworkInfo.SetupPort);
        }
        void img_sever_RecvedImage(object sender, RecvImageEventArgs e)
        {
            if (RecvedImage != null)
            {
                RecvedImage(this, e);
            }
        }
        public void StartEventClient()
        {
            sce = new SendEventClient(host_ip, NetworkInfo.EventPort);
        }
        public void Stop()
        {
            if (img_sever != null)
            {
                img_sever.Close();
                img_sever = null;
            }
        }
    }
}