﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChineseMedicine
{
    public partial class MassageInquire : Form
    {
        public MassageInquire()
        {
            InitializeComponent();
        }

        private void 确认信息_Click(object sender, EventArgs e)
        {
            this.Close();
            //this.Hide();
            //Form MainFunction = new MainFunction();
            //MainFunction.ShowDialog();
            //Application.ExitThread();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            String connstr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
            SqlConnection conn = new SqlConnection(connstr);
            conn.Open();

            if(textBox4.Text == "" || textBox5.Text == "" || 
                textBox9.Text == "" || comboBox1.SelectedItem.ToString()=="")
            {
                MessageBox.Show("信息请填写完整");

                return;
            }

            String sq = "select IDp from Patient where IDp = '" + textBox4.Text + "'";
            SqlCommand cq = new SqlCommand(sq, conn);
            if (cq.ExecuteScalar() != null)
            {
                MessageBox.Show("患者已存在，无需录入！");
            }
            else
            {
                //获取出生日期
                String birthday = Get_Data();
                //MessageBox.Show(birthday);

                //获取Gender（M/F）
                String gender = "";
                gender = comboBox1.SelectedItem.ToString();

                String str = "";
                
                str = "insert into Patient values('" + textBox4.Text + "', '" + textBox5.Text
                    + "', '" + gender + "','" + birthday + "','" + textBox11.Text + "', '" + textBox10.Text + "')";

                SqlCommand cmd = new SqlCommand(str, conn);
                cmd.ExecuteScalar();
                //MessageBox.Show("患者信息已录入！");

                String s = Address_Deal();
                cq.CommandText = "select IDp from Address where Addres = '" + s + "' and IDp = '" + textBox4.Text + "'";
                if (cq.ExecuteScalar() != null)
                {
                    MessageBox.Show("该地址已存在，无需录入！");
                }
                else
                {
                    cq.CommandText = "select Count(*) from Address";
                    int num = Convert.ToInt32(cq.ExecuteScalar()) + 1;

                    while (true)
                    {
                        cq.CommandText = "select IDa from Address where IDa = '" + num.ToString() + "'";
                        if (cq.ExecuteScalar() != null)
                        {
                            num++;
                        }
                        else
                            break;
                    }

                    //MessageBox.Show(num.ToString());

                    cmd.CommandText = "insert into Address values('" + num.ToString() + "','" + textBox4.Text
                        + "','" + s + "')";
                    cmd.ExecuteScalar();
                    MessageBox.Show("患者信息录入成功！");
                }
            }
            conn.Close();
        }

        private string Address_Deal()
        {
            String s = "";

            String sp = textBox2.Text;

            if(sp.IndexOf(comboBox2.SelectedItem.ToString()) == -1)
            {
                sp += comboBox2.SelectedItem.ToString();
            }

            if(textBox2.Text == "")
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

        //获取出生日期
        private string Get_Data()
        {
            String s = "";
            if (textBox12.Text == "")
            {
                s += "1900-";
            }
            else
            {
                s += textBox12.Text + "-";
            }

            if (textBox1.Text == "")
            {
                s += "01-";
            }
            else
            {
                if (textBox1.Text.Length == 1)
                {
                    s += "0";
                }
                s += textBox1.Text + "-";
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
            s +=" 00:00:00";

            //MessageBox.Show(s);
            return s;
        }

        private void MassageInquire_FormClosing(object sender, FormClosingEventArgs e)
        {
            //System.Environment.Exit(0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form MassageNewAddress = new MassageNewAddress();
            MassageNewAddress.ShowDialog();
            this.Show();
        }

        private void MassageInquire_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
            comboBox4.SelectedIndex = 0;

        }
    }
}