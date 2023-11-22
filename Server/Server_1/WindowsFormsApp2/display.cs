﻿using MetroFramework.Controls;
using System;
using System.Drawing;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class display : Form
    {
        bool check;
        Size csize;
        SendEventClient EventSC
        {
            get
            {
                return Controller.Singleton.SendEventClient;
            }
        }
        public display()
        {
            InitializeComponent();
        }

        private void RemoteClientForm_Load(object sender, EventArgs e)
        {
            Controller.Singleton.RecvedImage += Singleton_RecvedImage;
            //metroTile1.Location = new Point(1780, 15);
        }

        private void Singleton_RecvedImage(object sender, RecvImageEventArgs e)
        {
            if (check == false)
            {
                Controller.Singleton.StartEventClient();
                check = true;
                csize = e.Image.Size;
            }
            pbox_remote.Image = e.Image;
        }

        

        private Point ConvertPoint(int x, int y)
        {
            int nx = csize.Width * x / pbox_remote.Width;
            int ny = csize.Height * y / pbox_remote.Height;
            return new Point(nx, ny);
        }


        private void RemoteClientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            string EName = _505.SendName;//Username;
            e.Cancel = true;
            if (MyChatServer.userData.ContainsKey(EName))
            {
                TcpClient targetClient = MyChatServer.userData[EName];
                NetworkStream stream = targetClient.GetStream();
                byte[] buffer = Encoding.Default.GetBytes("**DISPLAYEXIT**");// 보낼 메시지 바이트 배열로 변환
                stream.Write(buffer, 0, buffer.Length); // 메시지 전송
            }
            this.Hide();
        }

        private void metroTile2_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {

                this.WindowState = FormWindowState.Normal;

            }
            else
            {

                this.WindowState = FormWindowState.Maximized;


            }
        }
    }
}

