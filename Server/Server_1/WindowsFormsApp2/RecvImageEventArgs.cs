using System;
using System.Drawing;
using System.Net;

namespace WindowsFormsApp2
{
    /// <summary>
    /// 이미지 수신 처리를 위한 대리자
    /// </summary>
    /// <param name="sender">이벤트 통보 개체(게시자)</param>
    /// <param name="e">이벤트 처리 인자</param>
    public delegate void RecvImageEventHandler(object sender, RecvImageEventArgs e);
    /// <summary>
    /// 이미지 수신 이벤트 인자 클래스
    /// </summary>
    public class RecvImageEventArgs:EventArgs
    {/// <summary>
     /// IP 단말 - 가져오기
     /// </summary>
        public IPEndPoint IPEndPoint
        {
            get;
            private set;
        }

        /// <summary>
        /// IP 주소 - 가져오기
        /// </summary>
        public IPAddress IPAddress
        {
            get
            {
                return IPEndPoint.Address;
            }
        }

        /// <summary>
        /// IP 주소 문자열 - 가져오기
        /// </summary>
        public string IPAddressStr
        {
            get
            {
                return IPAddress.ToString();
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
        /// 이미지 - 가져오기
        /// </summary>
        public Image Image
        {
            get;
            private set;
        }

        /// <summary>
        /// 이미지 크기 - 가져오기
        /// </summary>
        public Size Size
        {
            get
            {
                return Image.Size;
            }
        }

        /// <summary>
        /// 이미지 폭 - 가져오기
        /// </summary>
        public int Width
        {
            get
            {
                return Image.Width;
            }
        }

        /// <summary>
        /// 이미지 높이 - 가져오기
        /// </summary>
        public int Height
        {
            get
            {
                return Image.Height;
            }
        }

        internal RecvImageEventArgs(IPEndPoint remote_iep, Image image)
        {
            IPEndPoint = remote_iep;
            Image = image;
        }

        /// <summary>
        /// ToString 메서드
        /// </summary>
        /// <returns>IP주소 및 이미지 크기를 문자열로 반환</returns>
        public override string ToString()
        {
            return string.Format("IP:{0} width:{1} height:{1}",
                                  IPAddressStr, Width, Height);
        }
    }


}