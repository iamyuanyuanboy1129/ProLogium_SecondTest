using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProLogium_SecondTest
{
    public partial class Form1 : Form
    {
        int func = 1;
        String cntStr = @"Data Source=(LocalDB)\MSSQLLocalDB;" +
          @"AttachDBFilename = C:\Users\BOSS-20210208\Desktop\vscode_windform\ProLogium_SecondTest\Student.mdf"; //資料庫路徑

        public Form1()
        {
            InitializeComponent();
        }

        bool connectDB(ref SqlConnection sqlDb) //連接資料庫
        {
            bool fg;
          try
            {
                sqlDb = new SqlConnection(cntStr);
                sqlDb.Open();
                fg = true;
            }
            catch (Exception ex)
            {
                fg = false;
            }
            return fg; 
        }

        void queryScore(string name, SqlConnection sqlDb) //查詢成績功能
        {
            string sqlSrt, str;

            sqlSrt = String.Format("Select ScoreDB.學號, 姓名, " +
                "國文, 英文, 數學, 社會 from studentDB inner join " +
                "ScoreDB ON studentDB.學號 = ScoreDB.學號 " +
                $"where 姓名 = N'{name}'");
            SqlCommand sqlCmd = new SqlCommand(sqlSrt, sqlDb);
            SqlDataReader sqlDr = sqlCmd.ExecuteReader();

            if (sqlDr.HasRows)
            {
                sqlDr.Read();
                str = String.Format($"學號:{sqlDr.GetInt32(0)} " +
                    $"姓名:{sqlDr.GetString(1)} " +
                    $"國文:{sqlDr.GetInt32(2)} " +
                    $"英文:{sqlDr.GetInt32(3)} " +
                    $"數學:{sqlDr.GetInt32(4)} " +
                    $"社會:{sqlDr.GetInt32(5)} ");
                sqlDr.Close();
            }
            else
            {
                str = "查無此學生成績";
            }
            label2.Text = str;
        }

        private void button1_Click(object sender, EventArgs e) //查詢成績按鈕
        {
            SqlConnection sqlDb = null;
            String name;

            if(!connectDB(ref sqlDb))
            {
                MessageBox.Show("無法開啟資料庫");
                return;
            }
            if(textBox1.Text == "")
            {
                label2.Text = "成績";
                return;
            }
            name = textBox1.Text;
            queryScore(name, sqlDb);

            sqlDb.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        void showData() //顯示資料功能
        {
            SqlConnection sqlDb;
            SqlDataReader rd;
            string showStr;
            string showSqlStr = "SELECT * FROM studentDB ";

          using (sqlDb = new SqlConnection(cntStr))
            {
                textBox6.Clear(); //顯示資料視窗
                try
                {
                    SqlCommand showCmd = new SqlCommand(showSqlStr, sqlDb);
                    sqlDb.Open();
                    textBox6.AppendText("學號          姓名          " +
                        "電話                    地址\r\n");
                    textBox6.AppendText("---------------------------" +
                        "---------------------------\r\n");
                    using (rd = showCmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            showStr = $"{rd.GetInt32(0)}     " +
                                $"{rd.GetString(1).Trim()}     " +
                                $"{rd.GetString(2)}          {rd.GetString(3)}\r\n";
                            textBox6.AppendText(showStr);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        void addData(int studentNo, string name, string phone, string addr) //新增功能
        {
            SqlConnection sqlDb;
            string sqlStr = "insert studentDB( 學號, 姓名, 電話, 住址 )" +
                $"values({studentNo}, N'{name}', N'{phone}', N'{addr}')";
            using (sqlDb = new SqlConnection(cntStr))
            {
                try
                {
                    sqlDb.Open();
                    using (SqlCommand showCmd = new SqlCommand(sqlStr))
                    {
                        showCmd.Connection = sqlDb;
                        showCmd.ExecuteNonQuery();
                        MessageBox.Show("資料已新增");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        void updateData(int studentNo, string name, string phone, string addr) //修改功能
        {
            SqlConnection sqlDb;
            string sqlStr = $"update studentDB set 姓名=N'{name}', " +
                $"電話=N'{phone}', 住址=N'{addr}' " +
                $"where 學號 = {studentNo}";
            using (sqlDb = new SqlConnection(cntStr))
            {
                try
                {
                    using (SqlCommand showCmd = new SqlCommand(sqlStr))
                    {
                        showCmd.Connection= sqlDb;
                        showCmd.Connection.Open();
                        showCmd.ExecuteNonQuery();
                        MessageBox.Show("資料已更新");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        void deleteData(int studentNo) //刪除功能
        {
            SqlConnection sqlDb;
            string[] tables = { "studentDB", "scoreDB", "healthDB" };
            string sqlStr;
            using (sqlDb = new SqlConnection(cntStr))
            {
                sqlDb.Open();
                foreach (var table in tables)
                {
                    try
                    {
                        sqlStr = $"delete from {table} " +
                            $"where 學號 = {studentNo}";
                        using (SqlCommand showcmd = new SqlCommand(sqlStr, sqlDb))
                        {
                            showcmd.ExecuteNonQuery();
                            MessageBox.Show("資料已從資料表"+table+"刪除");
                        }    
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }    
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e) //新增資料
        {
            func = 1;
            textBox3.Enabled = true; //電話
            textBox4.Enabled = true; //姓名
            textBox5.Enabled = true; //住址
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e) //修改資料
        {
            func = 2;
            textBox3.Enabled = true; //電話
            textBox4.Enabled = true; //姓名
            textBox5.Enabled = true; //住址
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e) //刪除資料
        {
            func = 3;
            textBox3.Enabled = false; //電話
            textBox4.Enabled = false; //姓名
            textBox5.Enabled = false; //住址
        }

        private void button2_Click(object sender, EventArgs e) //確定按鈕(缺少學號重複防呆)
        {
            switch(func)
            {
                case 1: //新增(學號、姓名、電話、住址)
                    if (textBox2.Text == "" || textBox3.Text == "" ||
                        textBox4.Text == "" || textBox5.Text == "")
                        //MessageBox.Show("所有欄位都要KEY");
                        break;
                    addData(Convert.ToInt32(textBox2.Text),
                            textBox4.Text, textBox3.Text, textBox5.Text);
                    break;
                case 2: //修改(學號、姓名、電話、住址)
                    if (textBox2.Text == "" || textBox3.Text == "" ||
                        textBox4.Text == "" || textBox5.Text == "")
                        //MessageBox.Show("所有欄位都要KEY");
                        break;
                    updateData(Convert.ToInt32(textBox2.Text),
                            textBox4.Text, textBox3.Text, textBox5.Text);
                    break;
                case 3: //刪除(學號)
                    if (textBox2.Text == "")
                        break;
                    deleteData(Convert.ToInt32(textBox2.Text));
                    break;
            }
        }

        private void button3_Click(object sender, EventArgs e) //顯示資料按鈕
        {
            showData();
        }
    }
}
