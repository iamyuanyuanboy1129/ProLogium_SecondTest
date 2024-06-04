using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        public Form1()
        {
            InitializeComponent();
        }

        bool connectDB(ref SqlConnection sqlDb) //連接資料庫
        {
            bool fg;
            String cntStr = @"Data Source=(LocalDB)\MSSQLLocalDB;" +
                            @"AttachDBFilename = C:\Users\BOSS-20210208\Desktop\vscode_windform\ProLogium_SecondTest\Student.mdf"; //資料庫路徑
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

        void queryScore(string name, SqlConnection sqlDb)
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
                str = "查無此學生";
            }
            label2.Text = str;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
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
    }
}
