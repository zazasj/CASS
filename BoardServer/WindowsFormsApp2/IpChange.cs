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



        private void IpChange_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                settings.Reload();
                label3.Text = settings.ServerIP;
                metroTextBox2.Text = settings.ServerIP;
            }
        }

        private void button_MouseEnter(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                ButtonColorChange(button);
            }

        }

        private void button_MouseLeave(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                ButtonColorUnChange(button);
            }
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

        private void button1_Click(object sender, EventArgs e)
        {

            settings.ServerIP = metroTextBox2.Text;
            settings.Save();
            this.Hide();
            Info info = new Info();
            info.Show();
        }

        private void IpChange_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel= true;
            this.Hide();
        }
    }
}
