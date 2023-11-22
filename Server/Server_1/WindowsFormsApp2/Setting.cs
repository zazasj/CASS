using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp2.Properties;
using Excel = Microsoft.Office.Interop.Excel;


namespace WindowsFormsApp2
{
    public partial class Setting : Form
    {
        Properties.Settings settings = Properties.Settings.Default;
        Color color1 = Color.FromArgb(38, 58, 119);
        public Setting()
        {
            InitializeComponent();
        }

        //시간표 업로드
        private void button1_Click(object sender, EventArgs e)
        {
            string connStr2 = "server="+settings.DBIP+";user=Server505;database=dbtest;password=<123456789>";
            string query2 = "TRUNCATE TABLE timetable;";
            MySqlConnection conn2 = new MySqlConnection(connStr2);
            conn2.Open();
            MySqlCommand cmd2 = new MySqlCommand(query2, conn2);
            cmd2.ExecuteNonQuery();
            conn2.Close();

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel Files|*.xlsx;*.xls;*.csv";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                ReadExcelFile(filePath, "Sheet1", "B2:F10"); //DB 타임테이블 생성
                ReadExcelFile2(filePath, "Sheet1", "A2:F10"); //시간표 girdview에 넣기
            }
        }

        //양식 다운로드 버튼
        private void button2_Click(object sender, EventArgs e)
        {
            // Excel application 생성
            Excel.Application excelApp = new Excel.Application();

            // Excel 파일 열기
            Excel.Workbook workbook = excelApp.Workbooks.Add();
            Excel.Worksheet worksheet = workbook.ActiveSheet;

            // 데이터 삽입
            worksheet.Cells[1, 1] = "교시/요일";
            worksheet.Cells[1, 2] = "월요일";
            worksheet.Cells[1, 3] = "화요일";
            worksheet.Cells[1, 4] = "수요일";
            worksheet.Cells[1, 5] = "목요일";
            worksheet.Cells[1, 6] = "금요일";

            worksheet.Cells[2, 1] = "1교시\n(9:30~10:20)";
            worksheet.Cells[3, 1] = "2교시\n(10:30~11:20)";
            worksheet.Cells[4, 1] = "3교시\n(11:30~12:20)";
            worksheet.Cells[5, 1] = "4교시\n(12:30~13:20)";
            worksheet.Cells[6, 1] = "5교시\n(13:30~14:20)";
            worksheet.Cells[7, 1] = "6교시\n(14:30~15:20)";
            worksheet.Cells[8, 1] = "7교시\n(15:30~16:20)";
            worksheet.Cells[9, 1] = "8교시\n(16:30~17:20)";
            worksheet.Cells[10, 1] = "9교시\n(17:30~18:20)";

            Excel.Range range = worksheet.Range["A1:F10"];
            range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            range.EntireColumn.ColumnWidth = 19;
            Excel.Borders borders = range.Borders;
            borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            borders.Weight = 2d;

            // Excel 파일 저장
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel Files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            saveFileDialog.Title = "Save Excel File";

            // Excel application 종료
            excelApp.Quit();
        }

        //학생 명단 업로드 
        private void button3_Click(object sender, EventArgs e)
        {
            HashSet<string> cellValues1 = new HashSet<string>();
            HashSet<string> cellValues2 = new HashSet<string>();
            List<string> l1 = new List<string>();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                string[] Fn = new string[2];
                string Classname;

                foreach (string file in openFileDialog.FileNames)
                {
                    string realfilename = Path.GetFileName(file);

                    if (realfilename.Contains("["))
                    {
                        Fn = realfilename.Split('[');

                    }
                    else if (realfilename.Contains("("))
                    {
                        Fn = realfilename.Split('(');
                    }

                    MessageBox.Show(Fn[0]);
                    Classname = Fn[0]; //과목명
                    Excel.Application excelApp = new Excel.Application();
                    Excel.Workbook workbook = excelApp.Workbooks.Open(file);
                    Excel.Worksheet worksheet = workbook.Sheets[1];

                    Excel.Range searchRange = worksheet.UsedRange;
                    Excel.Range foundCell1 = searchRange.Find("학과(전공)");
                    Excel.Range foundCell2 = searchRange.Find("학번");
                    Excel.Range foundCell3 = searchRange.Find("이름");

                    MySqlConnection connection = new MySqlConnection("server="+settings.DBIP+";user=Server505;database=dbtest;password=<123456789>");
                    connection.Open();

                    string connStr = "server=" + settings.DBIP + ";user=Server505;database=dbtest;password=<123456789>";
                    string query = "TRUNCATE TABLE account_info;";
                    MySqlConnection conn = new MySqlConnection(connStr);
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    string connStr2 = "server=" + settings.DBIP + ";user=Server505;database=dbtest;password=<123456789>";
                    string query2 = "TRUNCATE TABLE totaltable;";
                    MySqlConnection conn2 = new MySqlConnection(connStr2);
                    conn2.Open();
                    MySqlCommand cmd2 = new MySqlCommand(query2, conn2);
                    cmd2.ExecuteNonQuery();
                    conn2.Close();

                    if (foundCell1 != null && foundCell2 != null)
                    {
                        Excel.Range offsetCell1 = foundCell1.Offset[1, 0]; // 같은 열의 다음 셀
                        Excel.Range offsetCell2 = foundCell2.Offset[1, 0]; // 같은 열의 다음 셀
                        Excel.Range offsetCell3 = foundCell3.Offset[1, 0]; // 같은 열의 다음 셀
                        while (offsetCell1 != null && offsetCell2 != null && offsetCell3 != null && offsetCell1.Value2 != null && offsetCell2.Value2 != null && offsetCell3.Value2 != null)
                        {
                            string cellValue1 = offsetCell1.Value2.ToString();
                            string cellValue2 = offsetCell2.Value2.ToString();
                            string cellValue3 = offsetCell3.Value2.ToString();
                            string insertQuery = "INSERT INTO " + Classname + " (학번,이름,학과) VALUES ('" + cellValue2 + "','" + cellValue3 + "','" + cellValue1 + "');";
                            MySqlCommand insertCommand = new MySqlCommand(insertQuery, connection);
                            if (insertCommand.ExecuteNonQuery() == 1)
                            {
                            }
                            else
                            {
                            }
                            offsetCell1 = offsetCell1.Offset[1, 0];
                            offsetCell2 = offsetCell2.Offset[1, 0];// 다음 셀로 이동
                            offsetCell3 = offsetCell3.Offset[1, 0];

                        }
                    }
                    connection.Close();

                    // "찾을 글자1"이 포함된 열에서 값을 HashSet<T>에 추가
                    int column1 = 0;
                    int row1 = 0;
                    Excel.Range searchRange1 = worksheet.Cells;
                    Excel.Range resultRange1 = searchRange1.Find("학번", System.Type.Missing, Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByColumns, Excel.XlSearchDirection.xlNext, false, false, System.Type.Missing);
                    if (resultRange1 != null)
                    {
                        column1 = resultRange1.Column;
                        row1 = resultRange1.Row;
                        // 해당 열의 값을 HashSet<T>에 추가
                        for (int i = row1 + 1; i <= worksheet.UsedRange.Rows.Count; i++)
                        {
                            // Value 속성의 반환 형식을 명시적으로 지정
                            string cellValue1 = (worksheet.Cells[i, column1].Value2 ?? "").ToString();
                            cellValues1.Add(cellValue1);
                        }
                    }
                    // "찾을 글자2"가 포함된 열에서 값을 HashSet<T>에 추가
                    int column2 = 0;
                    int row2 = 0;
                    Excel.Range searchRange2 = worksheet.Cells;
                    Excel.Range resultRange2 = searchRange2.Find("이름", System.Type.Missing,
                        Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByColumns,
                        Excel.XlSearchDirection.xlNext, false, false, System.Type.Missing);
                    if (resultRange2 != null)
                    {
                        column2 = resultRange2.Column;
                        row2 = resultRange2.Row;
                        // 해당 열의 값을 HashSet<T>에 추가
                        for (int i = row2 + 1; i <= worksheet.UsedRange.Rows.Count; i++)
                        {
                            // Value 속성의 반환 형식을 명시적으로 지정
                            string cellValue2 = (worksheet.Cells[i, column2].Value2 ?? "").ToString();
                            cellValues2.Add(cellValue2);
                        }
                    }
                    workbook.Close(false);
                    excelApp.Quit();
                }
                // HashSet<T>에 저장된 값을 출력
                int index1 = 0; // cellValues1에서 값을 가져오는 인덱스 변수
                int index2 = 0; // cellValues2에서 값을 가져오는 인덱스 변수
                int count = cellValues1.Count + cellValues2.Count; // 출력할 값의 총 개수
                string StNum = "";
                string StName = "";
                string NumName = "";
                for (int i = 0; i < count; i++)
                {
                    if (i % 2 == 0) // 짝수 인덱스일 경우, cellValues1에서 값을 가져옴
                    {
                        if (index1 < cellValues1.Count) // 가져올 값이 남아있는 경우에만 출력
                        {
                            StNum = cellValues1.ElementAt(index1);
                            index1++; // 인덱스 증가
                        }
                    }
                    else // 홀수 인덱스일 경우, cellValues2에서 값을 가져옴
                    {
                        if (index2 < cellValues2.Count) // 가져올 값이 남아있는 경우에만 출력
                        {
                            StName = cellValues2.ElementAt(index2);
                            index2++; // 인덱스 증가
                        }
                        NumName = StNum + StName;
                        MySqlConnection connection2 = new MySqlConnection("server=" + settings.DBIP + ";user=Server505;database=dbtest;password=<123456789>");
                        connection2.Open();
                        string insertQuery2 = "INSERT INTO totaltable (학번,이름,비번) VALUES ('" + StNum + "','" + StName + "','0000');";
                        MySqlCommand insertCommand2 = new MySqlCommand(insertQuery2, connection2);
                        if (insertCommand2.ExecuteNonQuery() == 1) { }
                        else { }
                        connection2.Close();
                    }
                }
            }
        }

        //수업명단(개인테이블)
        private void button4_Click(object sender, EventArgs e)
        {
            // OpenFileDialog를 사용하여 엑셀 파일 선택
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
            openFileDialog1.Multiselect = true;
            List<string> filelist = new List<string>();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                foreach (string file in openFileDialog1.FileNames)
                {
                    filelist.Add(file);
                }
            }
            for (int i = 0; i < filelist.Count; i++)
            {
                // 파일 경로
                List<string> list1 = new List<string>(); //엑셀 데이터
                List<string> dblist = new List<string>(); //DB 데이터
                string filePath = filelist[i];
                Excel.Application excelApp = new Excel.Application();
                Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
                Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Sheets[1]; // 첫 번째 워크시트
                Excel.Range searchRange = worksheet.UsedRange;
                string[] Fn = new string[2];
                string Classname;
                string realfilename = Path.GetFileName(filePath);
                if (realfilename.Contains("["))
                {
                    Fn = realfilename.Split('[');

                }
                else if (realfilename.Contains("("))
                {
                    Fn = realfilename.Split('(');
                }
                Classname = Fn[0]; //과목명
                MessageBox.Show(Classname);
                Excel.Range foundCell1 = searchRange.Find("학번");
                Excel.Range foundCell2 = searchRange.Find("이름");
                Excel.Range foundCell3 = searchRange.Find("학과(전공)");
                MySqlConnection connection = new MySqlConnection("server="+ settings.DBIP+ ";user=Server505;database=dbtest;password=<123456789>");
                connection.Open();

                string query2 = "SELECT 학번 FROM " + Classname;
                var adapter2 = new MySqlDataAdapter(query2, connection);
                var dataTable2 = new DataTable();
                adapter2.Fill(dataTable2);
                // 리스트에 없는 값을 가진 행을 삭제합니다.
                foreach (DataRow row in dataTable2.Rows)
                {
                    string dbValue = row["학번"].ToString();
                    dblist.Add(dbValue);
                }
                // DataTable의 각 행에서 값을 추출하여 변수에 저장합니다.
                if (foundCell1 != null && foundCell2 != null && foundCell3 != null) //range에서 cell1으로 변경
                {
                    Excel.Range offsetCell1 = foundCell1.Offset[1, 0];
                    Excel.Range offsetCell2 = foundCell2.Offset[1, 0];
                    Excel.Range offsetCell3 = foundCell3.Offset[1, 0];
                    while (offsetCell1 != null && offsetCell2 != null && offsetCell1.Value2 != null && offsetCell2.Value2 != null)
                    {
                        string cellValue1 = offsetCell1.Value2.ToString();
                        string cellValue2 = offsetCell2.Value2.ToString();
                        string cellValue3 = offsetCell3.Value2.ToString();
                        list1.Add(cellValue1);
                        string query1 = "SELECT * FROM " + Classname + " WHERE 학번=@학번 AND 학과=@학과 AND 이름 = @이름";
                        MySqlCommand command = new MySqlCommand(query1, connection);
                        MySqlDataAdapter adapter1 = new MySqlDataAdapter(command);
                        command.Parameters.AddWithValue("@학번", cellValue1);
                        command.Parameters.AddWithValue("@이름", cellValue2);
                        command.Parameters.AddWithValue("@학과", cellValue3);
                        System.Data.DataTable dataTable1 = new System.Data.DataTable();
                        adapter1.Fill(dataTable1);
                        // 중복 데이터가 없는 경우 MySQL에 데이터 추가
                        if (dataTable1.Rows.Count == 0)
                        {
                            query1 = "INSERT INTO " + Classname + " (학번, 이름, 학과) VALUES (@학번, @이름, @학과)";
                            command = new MySqlCommand(query1, connection);
                            command.Parameters.AddWithValue("@학번", cellValue1);
                            command.Parameters.AddWithValue("@이름", cellValue2);
                            command.Parameters.AddWithValue("@학과", cellValue3);
                            command.ExecuteNonQuery();
                        }
                        offsetCell1 = offsetCell1.Offset[1, 0]; //다음셀로 이동
                        offsetCell2 = offsetCell2.Offset[1, 0];
                        offsetCell3 = offsetCell3.Offset[1, 0];
                    }
                    // DB 연결 설정
                }
                // 리스트에 없는 값을 가진 행을 삭제합니다.
                foreach (DataRow row in dataTable2.Rows)
                {
                    string dbValue = row["학번"].ToString();
                    if (!list1.Contains(dbValue))
                    {
                        // 삭제 쿼리 작성
                        string deleteQuery = "DELETE FROM " + Classname + " WHERE 학번='" + dbValue + "'";
                        MySqlCommand cmd = new MySqlCommand(deleteQuery, connection);
                        cmd.ExecuteNonQuery();
                    }
                }
                list1.Clear();
                workbook.Close();
                excelApp.Quit();
                MessageBox.Show("확인");
                connection.Close();
            }
        }

        //로그인명단(통합테이블)
        private void button5_Click(object sender, EventArgs e)
        {
            //엑셀에는 있는데 DB에없으면 추가.
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
            openFileDialog1.Title = "Select an Excel File";
            openFileDialog1.Multiselect = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {


                foreach (string file in openFileDialog1.FileNames)
                {
                    string filePath = openFileDialog1.FileName;



                    Excel.Application excelApp = new Excel.Application();
                    Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
                    Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Sheets[1]; // 첫 번째 워크시트

                    Excel.Range searchRange = worksheet.UsedRange;
                    Excel.Range foundCell1 = searchRange.Find("학번");
                    Excel.Range foundCell2 = searchRange.Find("이름");

                    MySqlConnection connection = new MySqlConnection("server=" + settings.DBIP + ";user=Server505;database=dbtest;password=<123456789>");
                    connection.Open();

                    if (foundCell1 != null && foundCell2 != null)
                    {
                        Excel.Range offsetCell1 = foundCell1.Offset[1, 0]; // 같은 열의 다음 셀
                        Excel.Range offsetCell2 = foundCell2.Offset[1, 0]; // 같은 열의 다음 셀
                        while (offsetCell1 != null && offsetCell2 != null && offsetCell1.Value2 != null && offsetCell2.Value2 != null)
                        {
                            string cellValue1 = offsetCell1.Value2.ToString();//학번
                            string cellValue2 = offsetCell2.Value2.ToString();

                            string query = "SELECT * FROM totaltable WHERE 학번=@학번 AND 이름=@이름";
                            MySqlCommand command = new MySqlCommand(query, connection);
                            command.Parameters.AddWithValue("@학번", cellValue1);
                            command.Parameters.AddWithValue("@이름", cellValue2);
                            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                            System.Data.DataTable dataTable = new System.Data.DataTable();
                            adapter.Fill(dataTable);

                            // 중복 데이터가 없는 경우 MySQL에 데이터 추가
                            if (dataTable.Rows.Count == 0)
                            {
                                query = "INSERT INTO totaltable (학번, 이름, 비번) VALUES (@학번, @이름, '0000')";
                                command = new MySqlCommand(query, connection);
                                command.Parameters.AddWithValue("@학번", cellValue1);
                                command.Parameters.AddWithValue("@이름", cellValue2);
                                command.ExecuteNonQuery();
                            }
                            offsetCell1 = offsetCell1.Offset[1, 0];
                            offsetCell2 = offsetCell2.Offset[1, 0];// 다음 셀로 이동

                        }
                    }

                    // Excel 객체 모두 해제하기
                    workbook.Close();
                    excelApp.Quit();

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);

                    worksheet = null;
                    workbook = null;
                    excelApp = null;
                    GC.Collect();
                    connection.Close();
                }



            }
        }







        private void ReadExcelFile(string filePath, string sheetName, string range)
        {
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook excelWorkbook = excelApp.Workbooks.Open(filePath);
            Excel._Worksheet excelWorksheet = excelWorkbook.Sheets[sheetName];
            Excel.Range excelRange = excelWorksheet.Range[range];
            // Get values from Excel and remove duplicates
            string[] myArray = new string[] { "학번", "이름", "학과" };
            List<object> cellValues = new List<object>();
            foreach (Excel.Range cell in excelRange.Cells)
            {
                cellValues.Add(cell.Value);
            }
            cellValues = cellValues.Distinct().ToList();
            // Convert to array
            object[] uniqueValues = cellValues.ToArray();
            // MySQL 데이터베이스 연결 문자열
            string connectionString = "server="+settings.DBIP+";user=Server505;database=dbtest;password=<123456789>"; ;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                // 데이터베이스 연결 열기
                connection.Open();
                for (int i = 0; i < uniqueValues.Length; i++)
                {
                    if (uniqueValues[i] != null)
                    {


                        string query = $"CREATE TABLE {uniqueValues[i]} ({myArray[0]} VARCHAR(45), {myArray[1]} VARCHAR(45), {myArray[2]} VARCHAR(45))";
                        using (MySqlCommand command = new MySqlCommand(query, connection))
                        {

                            try
                            {
                                command.ExecuteNonQuery();
                            }
                            catch (MySqlException ex)
                            {
                                // 테이블이 이미 존재하는 경우, 예외 무시
                                if (ex.Number != 1050)
                                {
                                    // 다른 예외인 경우에는 다시 예외를 던지도록 함
                                }
                            }
                        }
                    }
                }
                string grantQuery = "GRANT ALL PRIVILEGES ON dbtest.* TO 'Tester1'@'%';";
                string grantQuery2 = "GRANT ALL PRIVILEGES ON dbtest.* TO 'Server505'@'172.18.4.40' WITH GRANT OPTION;";
                string grantQuery3 = "GRANT ALL PRIVILEGES ON dbtest.* TO 'Server505PC'@'172.18.4.151' WITH GRANT OPTION;";
                using (MySqlCommand grantCommand = new MySqlCommand(grantQuery, connection))
                {

                    try
                    {
                        grantCommand.ExecuteNonQuery();

                    }
                    catch (MySqlException ex)
                    {
                        // 테이블이 이미 존재하는 경우, 예외 무시
                        if (ex.Number != 1050)
                        {
                            // 다른 예외인 경우에는 다시 예외를 던지도록 함
                        }
                    }
                }
                using (MySqlCommand grantCommand2 = new MySqlCommand(grantQuery2, connection))
                {

                    try
                    {
                        grantCommand2.ExecuteNonQuery();

                    }
                    catch (MySqlException ex)
                    {
                        // 테이블이 이미 존재하는 경우, 예외 무시
                        if (ex.Number != 1050)
                        {
                            // 다른 예외인 경우에는 다시 예외를 던지도록 함
                        }
                    }
                }
                //505호 IP 
                using (MySqlCommand grantCommand3 = new MySqlCommand(grantQuery3, connection))
                {

                    try
                    {
                        grantCommand3.ExecuteNonQuery();

                    }
                    catch (MySqlException ex)
                    {
                        // 테이블이 이미 존재하는 경우, 예외 무시
                        if (ex.Number != 1050)
                        {
                            // 다른 예외인 경우에는 다시 예외를 던지도록 함
                        }
                    }
                }
                string flushQuery = "FLUSH PRIVILEGES;";
                using (MySqlCommand flushCommand = new MySqlCommand(flushQuery, connection))
                {
                    flushCommand.ExecuteNonQuery();
                }

                // 쿼리문 실행
                connection.Close();
            }
        }

        private void ReadExcelFile2(string filePath, string sheetName, string range)
        {
            MySqlConnection connection = new MySqlConnection("server="+settings.DBIP+";user=Server505;database=dbtest;password=<123456789>");
            connection.Open();
            string A = "";
            string B = "";
            string C = "";
            string D = "";
            string E = "";
            string F = "";
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook excelWorkbook = excelApp.Workbooks.Open(filePath);
            Excel._Worksheet excelWorksheet = excelWorkbook.Sheets[sheetName];
            Excel.Range excelRange = excelWorksheet.Range[range];
            // Get values from Excel without removing duplicates
            object[,] cellValues2 = excelRange.Value; //cellValues2 -> 시간표 전체
            // Store values in a variable
            List<object> cellValueList = new List<object>();
            for (int row = 1; row <= cellValues2.GetLength(0); row++)
            {
                for (int col = 1; col <= cellValues2.GetLength(1); col++)
                {
                    cellValueList.Add(cellValues2[row, col]);
                }
            }
            object[] cellValueArray = cellValueList.ToArray();
            for (int i = 0; i < cellValueArray.Length; i++)
            {
                if ((i % 6) == 0)
                {
                    A = (string)cellValueArray[i];

                }
                else if ((i % 6) == 1)
                {
                    B = (string)cellValueArray[i];
                }
                else if ((i % 6) == 2)
                {
                    C = (string)cellValueArray[i];
                }
                else if ((i % 6) == 3)
                {
                    D = (string)cellValueArray[i];
                }
                else if ((i % 6) == 4)
                {
                    E = (string)cellValueArray[i];
                }
                else
                {
                    F = (string)cellValueArray[i];
                    string insertQuery = "INSERT INTO timetable (교시,월요일,화요일,수요일,목요일,금요일) VALUES ('" + A + "','" + B + "','" + C + "','" + D + "','" + E + "','" + F + "');";
                    MySqlCommand insertCommand = new MySqlCommand(insertQuery, connection);
                    if (insertCommand.ExecuteNonQuery() == 1)
                    {

                    }
                    else
                    {

                    }

                }
                
            }
            connection.Close();
            // Close Excel objects
            excelWorkbook.Close();
            excelApp.Quit();
        }

        private void Setting_FormClosed(object sender, FormClosedEventArgs e)
        {
            ClassSchedule CS = new ClassSchedule();
            CS.Show();
        }

        //버튼 상호작용들
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

        private void button6_Click(object sender, EventArgs e)
        {
            settings.DBIP = textBox1.Text;
            settings.Save();
        }

        private void Setting_Load(object sender, EventArgs e)
        {
            textBox1.Text = settings.DBIP;
        }
    }
}
