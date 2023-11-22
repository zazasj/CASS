using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class IpChange : Form
    {

        Color color1 = Color.FromArgb(38, 58, 119);
        Properties.Settings settings = Properties.Settings.Default;

        public IpChange()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {

            settings.ServerIP = metroTextBox2.Text;
            settings.ClientFirstNum = metroTextBox1.Text;
            settings.Save();
            this.Hide();
            _505 classroom = new _505();
            classroom.Show();
        }


        private void IpChange_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                settings.Reload();
                iplabel.Text = settings.ServerIP;
                metroTextBox2.Text = settings.ServerIP;
                firstseatlabel.Text = settings.ClientFirstNum;
                metroTextBox1.Text = settings.ClientFirstNum;

            }
        }


        private void button_MouseEnter(object sender, EventArgs e)
        {
            ButtonColorChange(sender);
        }

        private void button_MouseLeave(object sender, EventArgs e)
        {
            ButtonColorUnChange(sender);
        }

        private void ButtonColorChange(object sender)
        {
            ((Button)sender).FlatAppearance.BorderSize = 0;
            ((Button)sender).BackColor = color1;
            ((Button)sender).ForeColor = Color.White;

        }
        private void ButtonColorUnChange(object sender)
        {
            ((Button)sender).FlatAppearance.BorderSize = 1;
            ((Button)sender).BackColor = Color.White;
            ((Button)sender).ForeColor = Color.Black;

        }

        private void IpChange_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            _505 classroom = new _505();
            classroom.Show();
        }
    }
}
