using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace ChineseMedicine
{
    public partial class selectMassage : Form
    {
        public selectMassage()
        {
            InitializeComponent();
        }

        private void 确认信息_Click(object sender, EventArgs e)
        {
            this.Close();
            //Form MainFunction = new MainFunction();
            //MainFunction.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String connstr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
            SqlConnection conn = new SqlConnection(connstr);
            conn.Open();

            String sq = "select Name from Patient where IDp = '" + textBox4.Text + "'";
            SqlCommand cq = new SqlCommand(sq, conn);
            if (cq.ExecuteScalar() != null)
            {
                textBox5.Text = cq.ExecuteScalar().ToString();

                cq.CommandText = "select Phone from Patient where IDp = '" + textBox4.Text + "'";
                textBox6.Text = cq.ExecuteScalar().ToString();

                cq.CommandText = "select HomePhone from Patient where IDp = '" + textBox4.Text + "'";
                textBox7.Text = cq.ExecuteScalar().ToString();

                //数据库查询结果全部返回形成下拉框comboBox
                DataTable dt = new DataTable();
                cq.CommandText = "select Addres from Address where IDp = '" + textBox4.Text + "'";
                if(cq.ExecuteScalar() == null)
                {
                    MessageBox.Show("未查询到地址，请添加地址");
                }

                SqlDataAdapter sda = new SqlDataAdapter(cq);
                sda.Fill(dt);
                this.comboBox1.DataSource = (from x in dt.Rows.Cast<DataRow>().ToList() select x[0]).ToList();

                MessageBox.Show("查询成功！");
            }
            else
            {
                MessageBox.Show("无该患者记录！");
            }
            conn.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String connstr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
            SqlConnection conn = new SqlConnection(connstr);
            conn.Open();

            string sq = "";
            SqlCommand cq = new SqlCommand(sq, conn);
            cq.CommandText = "select Count(*) from Information";
            String num = Convert.ToString(Convert.ToInt32(cq.ExecuteScalar()) + 1);
            
            while(true)
            {
                cq.CommandText = "select IDnum from Information where IDnum = '" + num + "'";
                if(cq.ExecuteScalar() == null)
                    break;
                num = Convert.ToString(Convert.ToInt32(num) + 1);
            }

            //MessageBox.Show(num);
            String s = Get_Data();

            if(textBox1.Text == textBox2.Text && textBox2.Text == textBox3.Text && textBox3.Text == "")
            {
                s = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                
            }
            //MessageBox.Show(s);
            cq.CommandText = "insert into Information values('" + num + "','" + textBox4.Text + "','" 
                + this.comboBox1.Text + "','" + s + "')";
            cq.ExecuteNonQuery();

            conn.Close();
            
            All.Yao = checkedListBox1.Text;
            All.Lei = checkedListBox2.Text;
            All.Num = textBox8.Text + "t" + textBox9.Text + "x" + textBox10.Text;
            All.Bei = textBox11.Text;
            All.Id = num;
            All.Phone = textBox6.Text;
            All.Home_phone = textBox7.Text;
            All.Address = comboBox1.SelectedItem.ToString();

            //MessageBox.Show(All.Lei);

            this.Hide();
            Form Massageform = new Massageform();
            Massageform.ShowDialog();
            this.Show();
        }

        private string Get_Data()
        {
            String s = "";
            if (textBox1.Text == "")
            {
                s += "1900-";
            }
            else
            {
                s += textBox1.Text + "-";
            }

            if (textBox2.Text == "")
            {
                s += "01-";
            }
            else
            {
                if (textBox2.Text.Length == 1)
                {
                    s += "0";
                }
                s += textBox2.Text + "-";
            }

            if (textBox3.Text == "")
            {
                s += "01";
            }
            else
            {
                if (textBox3.Text.Length == 1)
                {
                    s += "0";
                }
                s += textBox3.Text;
            }
            s += " " + DateTime.Now.ToLongTimeString().ToString();

            //MessageBox.Show(s);
            return s;
        }

        private void Massageform_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void selectMassage_Load(object sender, EventArgs e)
        {
            textBox1.Text = DateTime.Now.Year.ToString();
            textBox2.Text = DateTime.Now.Month.ToString();
            textBox3.Text = DateTime.Now.Day.ToString();
        }
    }
}
