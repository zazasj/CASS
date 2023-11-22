using System;
using System.Net;

namespace RCEventArgsLib
{/// <summary>
/// 원격 제어 요청 수신 이벤트를 정의하기 위한 대리자
/// </summary>
/// <param name="sender">이벤트 통보 개체</param>
/// <param name="e">이벤트 처리 인자</param>
    public delegate void RecvRCInfoEventHandler(object sender, RecvRCInfoEventArgs e);
    /// <summary>
    /// 원격 제어 요청 수신 이벤트 인자 클래스
    /// </summary>
    public class RecvRCInfoEventArgs : EventArgs
    {
        /// <summary>
        /// IP 단말 정보 - 가져오기
        /// </summary>
        public IPEndPoint IPEndPoint
        {
            get;
            private set;
        }
        /// <summary>
        /// IP 주소 문자열 - 가져오기
        /// </summary>
        public string IPAddressStr
        {
            get
            {
                return IPEndPoint.Address.ToString();
            }
        }
        /// <summary>
        /// 포트 - 가져오기
        /// </summary>
        public int Port
        {
            get
            {
                return IPEndPoint.Port;
            }
        }
        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="RemoteEndPoint">상대츨 단말 정보</param>
        public RecvRCInfoEventArgs(EndPoint RemoteEndPoint)
        {
            IPEndPoint = RemoteEndPoint as IPEndPoint;
        }
    }
}
