using System;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace WindowsFormsApp2
{   /// <summary>
    /// 원격 제어 이벤트 종류 열거형
    /// </summary>
    public enum MsgType
    {
        MT_KDOWN = 1, MT_KEYUP, MT_M_LEFTDOWN,
        MT_M_LEFTUP, MT_M_RIGHTUP, MT_M_RIGHTDOWN,
        MT_M_MIDDLEDOWN, MT_M_MIDDLEUP, MT_M_MOVE
    }

    /// <summary>
    /// 원격 제어 이벤트 전송 클라이언트 클래스
    /// </summary>
    public class SendEventClient
    {
        IPEndPoint ep;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="ip">원격지 호스트 IP</param>
        /// <param name="port">포트</param>
        public SendEventClient(string ip, int port)
        {
            //원격 제어 호스트 IP 단말 개체 생성
            ep = new IPEndPoint(IPAddress.Parse(ip), port);
        }

        /// <summary>
        /// 키 누름 이벤트 전송 메서드
        /// </summary>
        /// <param name="key">누른 키</param>
        public void SendKeyDown(int key)
        {
            byte[] data = new byte[9];
            data[0] = (byte)MsgType.MT_KDOWN;//키 누름 이벤트 설정
            Array.Copy(BitConverter.GetBytes(key), 0, data, 1, 4);//누른 키를 버퍼에 복사
            SendData(data);//버퍼 전송
        }

        private void SendData(byte[] data)
        {
            //소켓 생성
            Socket sock = new Socket(AddressFamily.InterNetwork,
                                     SocketType.Stream,
                                     ProtocolType.Tcp);
            sock.Connect(ep);//원격 제어 호스트에 연결
            sock.Send(data); //이벤트 전송
            sock.Close(); //소켓 닫기
        }

        /// <summary>
        /// 키 뗌 이벤트 전송 메서드
        /// </summary>
        /// <param name="key">뗀 키</param>
        public void SendKeyUp(int key)
        {
            byte[] data = new byte[9];
            data[0] = (byte)MsgType.MT_KEYUP;//키 뗌 이벤트 설정
            Array.Copy(BitConverter.GetBytes(key), 0, data, 1, 4); //뗀 키를 버퍼에 복사
            SendData(data);//버퍼 전송
        }

        /// <summary>
        /// 마우스 누름 이벤트 전송 메서드
        /// </summary>
        /// <param name="mouseButtons">누른 마우스 버튼</param>
        public void SendMouseDown(MouseButtons mouseButtons)
        {
            byte[] data = new byte[9];
            switch (mouseButtons)//마우스 버튼에 따라 메시지 종류 결정
            {
                case MouseButtons.Left: data[0] = (byte)MsgType.MT_M_LEFTDOWN; break;
                case MouseButtons.Right: data[0] = (byte)MsgType.MT_M_RIGHTDOWN; break;
                case MouseButtons.Middle: data[0] = (byte)MsgType.MT_M_MIDDLEDOWN; break;
            }
            SendData(data);//마우스 누름 이벤트 전송
        }

        /// <summary>
        /// 마우스 뗌 이벤트 전송 메서드
        /// </summary>
        /// <param name="mouseButtons">뗀 마우스 버튼</param>
        public void SendMouseUp(MouseButtons mouseButtons)
        {
            byte[] data = new byte[9];
            switch (mouseButtons) //마우스 버튼에 따라 메시지 종류 결정
            {
                case MouseButtons.Left: data[0] = (byte)MsgType.MT_M_LEFTUP; break;
                case MouseButtons.Right: data[0] = (byte)MsgType.MT_M_RIGHTUP; break;
                case MouseButtons.Middle: data[0] = (byte)MsgType.MT_M_MIDDLEUP; break;
            }
            SendData(data);//마우스 뗌 이벤트 전송
        }

        /// <summary>
        /// 마우스 이동 메서드
        /// </summary>
        /// <param name="x">마우스 x좌표</param>
        /// <param name="y">마우스 y좌표</param>
        public void SendMouseMove(int x, int y)
        {
            byte[] data = new byte[9];
            data[0] = (byte)MsgType.MT_M_MOVE;//마우스 이동 이벤트 설정
            Array.Copy(BitConverter.GetBytes(x), 0, data, 1, 4); //x좌표 복사
            Array.Copy(BitConverter.GetBytes(y), 0, data, 5, 4); //y좌표 복사
            SendData(data); //마우스 이동 이벤트 전송
        }
    }
}   
