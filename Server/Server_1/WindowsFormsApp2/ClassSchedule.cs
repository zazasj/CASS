using Microsoft.Office.Interop.Excel;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class ClassSchedule : Form
    {


        Properties.Settings settings = Properties.Settings.Default;


        Color color1 = Color.FromArgb(38, 58, 119);
        public ClassSchedule()
        {
            InitializeComponent();
        }

        private void ClassSchedule_Load(object sender, EventArgs e)
        {
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 1)
            {
                DataGridViewCell selectedCell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                string selectedCellValue = selectedCell.Value.ToString();
                DialogResult result = MessageBox.Show("수업 명: "+selectedCellValue +" 으로 입장 하시겠습니까?", "수업 입장", MessageBoxButtons.OKCancel);

                if (result == DialogResult.OK)
                {
                    // OK 버튼이 클릭된 경우 실행되는 코드
                    _505 c505 = new _505();
                    c505.SetClassname(selectedCellValue);
                    this.Hide();
                    c505.Show();

                }
                else if (result == DialogResult.Cancel)
                {
                    // Cancel 버튼이 클릭된 경우 실행되는 코드
                }
            }
        }


        private void ClassSchedule_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible) {

                //string connectionString = "server= localhost;"+"user=root;database=dbtest;password=0000";
                string connectionString = "server="+ settings.DBIP+";user=Server505;database=dbtest;password=<123456789>";

                // MySQL 데이터베이스 연결
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    // 데이터베이스 연결 열기
                    connection.Open();

                    // 쿼리문 생성
                    string query = "SELECT * FROM timetable";

                    // 쿼리문 실행
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        // 데이터 어댑터 생성
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            // 데이터셋 생성
                            DataSet dataSet = new DataSet();

                            // 데이터셋에 데이터 어댑터로 데이터 채우기
                            try
                            {
                                adapter.Fill(dataSet);
                                // DataGridView에 데이터셋을 바인딩
                                dataGridView1.DataSource = dataSet.Tables[0];

                                dataGridView1.AutoGenerateColumns = false;
                                dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("시간표가 설정되어있지 않으니 초기설정 바랍니다.");
                            }
                        }
                    }

                    //dataGridView1.AutoGenerateColumns = true;
                    //    System.Data.DataTable dt = new System.Data.DataTable();

                    //    for (int i = 0; i < 9; i++)
                    //    {
                    //        dt.Rows.Add();
                    //        if (i < 6)
                    //        {
                    //            dataGridView1.Rows[i].Height = 60;
                    //        }
                    //        else dataGridView1.Rows[i].Height = 60;
                    //    }

                    //    dataGridView1.AutoGenerateColumns = false;
                    //    dataGridView1.DataSource = dt;

                    //    for (int i = 0; i < 9; i++) // 2번째 행부터 4번째 행까지
                    //    {
                    //        for (int j = 1; j < 6; j++) // 3번째 Column부터 5번째 Column까지
                    //        {
                    //            dataGridView1.Rows[i].Cells[j].Style.Font = new System.Drawing.Font("Arial", 14);
                    //        }
                    //    }
                    //    dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    //    dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    //    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    //    int totalRowHeight = dataGridView1.Rows.GetRowsHeight(DataGridViewElementStates.Visible);
                    //    dataGridView1.Height = totalRowHeight + dataGridView1.ColumnHeadersHeight;

                        //System.Data.DataTable dt = new System.Data.DataTable();
                        //dataGridView1.DataSource = dt.DefaultView;
                        //for (int i = 0; i < 9; i++)
                        //{
                        //    if (i < 6)
                        //    {
                        //        dataGridView1.Columns[i].Width = 170;
                        //        dataGridView1.Rows[i].Height = 60;
                        //    }
                        //    else dataGridView1.Rows[i].Height = 60;
                        //}

                        //for (int i = 0; i < 9; i++) // 2번째 행부터 4번째 행까지
                        //{
                        //    for (int j = 1; j < 6; j++) // 3번째 Column부터 5번째 Column까지
                        //    {
                        //        dataGridView1.Rows[i].Cells[j].Style.Font = new System.Drawing.Font("Arial", 14);
                        //    }
                        //}
                        //dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        //dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        //dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        //int totalRowHeight = dataGridView1.Rows.GetRowsHeight(DataGridViewElementStates.Visible);
                        //dataGridView1.Height = totalRowHeight + dataGridView1.ColumnHeadersHeight;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("초기 설정을 하시겠습니까??", "알림", MessageBoxButtons.OKCancel);

            if (result == DialogResult.OK)
            {
                // OK 버튼이 클릭되었을 때 수행할 작업
                this.Hide();
                Setting set = new Setting();
                set.Show();
            }
            else if (result == DialogResult.Cancel)
            {
                // Cancel 버튼이 클릭되었을 때 수행할 작업

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
            ((System.Windows.Forms.Button)sender).FlatAppearance.BorderSize = 0;
            ((System.Windows.Forms.Button)sender).BackColor = color1;
            ((System.Windows.Forms.Button)sender).ForeColor = Color.MistyRose;

        }
        private void ButtonColorUnChange(object sender)
        {
            ((System.Windows.Forms.Button)sender).FlatAppearance.BorderSize = 1;
            ((System.Windows.Forms.Button)sender).BackColor = Color.MistyRose;
            ((System.Windows.Forms.Button)sender).ForeColor = Color.Black;

        }
    }
}
