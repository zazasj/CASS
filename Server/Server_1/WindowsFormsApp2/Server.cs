using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace WindowsFormsApp2
{
    class Server
    {
        private TcpListener server;

        public Server(int port)
        {
            // 서버 소켓 생성
            server = new TcpListener(IPAddress.Any, port);
        }

        public void Start()
        {
            // 클라이언트 연결 대기
            server.Start();
            Console.WriteLine($"서버가 {((IPEndPoint)server.LocalEndpoint).Port}번 포트에서 대기 중입니다.");

            while (true)
            {
                // 클라이언트 연결 수락
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine($"클라이언트 {client.Client.RemoteEndPoint.ToString()}가 연결되었습니다.");

                // 클라이언트로부터 파일 데이터를 읽어들임
                Thread t = new Thread(() => {
                    using (NetworkStream ns = client.GetStream())
                    {
                        byte[] buffer = new byte[1024];
                        int bytesRead;
                        string filePath = "C:\\received.txt";
                        using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                        {
                            while ((bytesRead = ns.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                fs.Write(buffer, 0, bytesRead);
                            }
                        }
                    }

                    // 클라이언트 소켓 닫기
                    client.Close();
                    Console.WriteLine($"클라이언트 {client.Client.RemoteEndPoint.ToString()}와의 연결이 종료되었습니다.");
                });
                t.Start();
            }
        }

        public void Stop()
        {
            // 서버 소켓 닫기
            server.Stop();
        }
    }
}
