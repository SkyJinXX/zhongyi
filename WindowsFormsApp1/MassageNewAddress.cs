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
    public partial class MassageNewAddress : Form
    {
        public MassageNewAddress()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //保存新地址
        private void button2_Click(object sender, EventArgs e)
        {
            String connstr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
            SqlConnection conn = new SqlConnection(connstr);
            conn.Open();
            
            SqlCommand cq = new SqlCommand("", conn);
            SqlCommand cmd = new SqlCommand("", conn);

            cq.CommandText = "select Count(*) from Address";
            String num = (Convert.ToInt32(cq.ExecuteScalar()) + 1).ToString();

            Boolean flag = true;
            while (flag)
            {
                cq.CommandText = "select IDnum from Information where IDnum = '" + num + "'";
                if (cq.ExecuteScalar() != null)
                {
                    num = Convert.ToString(Convert.ToInt32(num) + 1);
                }
                else
                {
                    flag = false;
                }
            }

            String s = Address_Deal();

            cmd.CommandText = "insert into Address values('" + num.ToString() + "','" + textBox4.Text
                + "','" + s + "')";
            cmd.ExecuteScalar();
            MessageBox.Show("患者地址录入成功！");
        }

        private string Address_Deal()
        {
            String s = "";

            String sp = textBox2.Text;

            if (sp.IndexOf(comboBox2.SelectedItem.ToString()) == -1)
            {
                sp += comboBox2.SelectedItem.ToString();
            }

            if (textBox2.Text == "")
            {
                sp = "";
            }

            String ss = textBox6.Text;
            if (!ss.EndsWith("市"))
            {
                ss += "市";
            }

            if (textBox6.Text == "")
            {
                ss = "";
            }

            String sq = textBox8.Text;
            if (sq.IndexOf(comboBox3.SelectedItem.ToString()) == -1)
            {
                sq += comboBox3.SelectedItem.ToString();
            }

            if (textBox8.Text == "")
            {
                sq = "";
            }

            String sj = textBox7.Text;
            if (sj.IndexOf(comboBox4.SelectedItem.ToString()) == -1)
            {
                sj += comboBox4.SelectedItem.ToString();
            }

            if (textBox7.Text == "")
            {
                sj = "";
            }

            s = sp + ss + sq + sj + textBox9.Text;

            return s;
            throw new NotImplementedException();
        }

        //查询
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
                
                //数据库查询结果全部返回形成下拉框comboBox
                DataTable dt = new DataTable();
                cq.CommandText = "select Addres from Address where IDp = '" + textBox4.Text + "'";
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

        private void MassageNewAddress_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
            comboBox4.SelectedIndex = 0;
        }
    }
}
