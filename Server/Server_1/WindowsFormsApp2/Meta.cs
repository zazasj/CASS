using System.Drawing;

namespace WindowsFormsApp2
{
    /// <summary>
    /// 원격 제어 이벤트 수신 정보를 변환한 클래스
    /// </summary>
    public class Meta
    {
        /// <summary>
        /// 원격 제어 이벤트 종류 - 가져오기
        /// </summary>
        public MsgType Mt
        {
            get;
            private set;
        }

        /// <summary>
        /// 누르거나 뗀 키 - 가져오기
        /// </summary>
        public int Key
        {
            get;
            private set;
        }

        /// <summary>
        /// 마우스 좌표 - 가져오기
        /// </summary>
        public Point Now
        {
            get;
            private set;
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="data">수신한 원격 제어 이벤트 </param>
        public Meta(byte[] data)
        {
            Mt = (MsgType)data[0];//메시지 종류 설정

            switch (Mt)//메시지 종류에 따라
            {
                case MsgType.MT_KDOWN:
                case MsgType.MT_KEYUP:
                    MakingKey(data); break;//데이터를 키로 변환
                case MsgType.MT_M_MOVE:
                    MakingPoint(data); break;//데이터를 좌표로 변환
            }
        }

        private void MakingPoint(byte[] data)
        {
            //data를 좌표로 변환
            Point now = new Point(0, 0);
            now.X = (data[4] << 24) + (data[3] << 16) + (data[2] << 8) + (data[1]);
            now.Y = (data[8] << 24) + (data[7] << 16) + (data[6] << 8) + (data[5]);
            Now = now;
        }

        private void MakingKey(byte[] data)
        {
            //data를 키로 변환
            Key = (data[4] << 24) + (data[3] << 16) + (data[2] << 8) + (data[1]);
        }
    }
}
