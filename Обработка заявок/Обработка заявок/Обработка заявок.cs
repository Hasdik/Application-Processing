using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Обработка_заявок
{
    public partial class Form1 : Form
    {
        SqlConnection conn = new SqlConnection(@"Data Source=АЙЗИЛЯ-ПК\SQLEXPRESS;Initial Catalog='Обработка заявок';Integrated Security=True");
        public Form1()
        {
            InitializeComponent();

        }
        string writePath = @"D:\1.txt";
        int s;
        private void button2_Click(object sender, EventArgs e)
        {
            if ((radioButton1.Checked == false) && (radioButton2.Checked == false))
            {
                MessageBox.Show("Не выбрали действие!");
            }
            if (radioButton1.Checked == true)
            {
                    try
                    {
                        conn.Open();
                        s = Convert.ToInt32(textBox4.Text);
                        string s1 = textBox1.Text;
                        string s2 = textBox2.Text;
                        string s3 = textBox3.Text;
                        string s4 = dateTimePicker1.Value.Date.ToShortDateString();
                        string s5 = textBox6.Text;
                        string s6 = "Ожидание";
                        string sql = "insert into Заявки ([Номер заявки],ФИО,Почта, [Номер телефона], [Дата создания], Описание, Status) values ('" + s + "','" + s1 + "','" + s2 + "','" + s3 + "','" + s4 + "','" + s5 + "','" + s6 + "')";
                        SqlCommand command = new SqlCommand(sql, conn);
                        command.ExecuteNonQuery();
                        conn.Close();
                        printtable();
                        printtable2();
                        MessageBox.Show("Заявка: " + s + " добавлена!");
                        textBox1.Clear();
                        textBox2.Clear();
                        textBox3.Clear();
                        textBox4.Clear();
                        textBox6.Clear();
                    }
                    catch (FormatException ex)
                    {
                        MessageBox.Show("Заполните все поля! В поле 'Номер заяки' можно вводить только цифры!", ex.Message);
                        conn.Close();
                    }
            }
            else if (radioButton2.Checked==true)
            {
                conn.Open();
                int s = Convert.ToInt32(textBox4.Text);
                string s1 = textBox1.Text;
                string s2 = textBox2.Text;
                string s3 = textBox3.Text;
                string s4 = dateTimePicker1.Value.Date.ToShortDateString();
                string s5 = textBox6.Text;
                string s6 = "Ожидание";
                string sql = "Update Заявки set ФИО='" + s1 + "',Почта='" + s2 + "', [Номер телефона]='" + s3 + "',[Дата создания]='" + s4 + "', Описание='" + s5 + "', Status='" + s6 + "' where [Номер заявки]='" + s + "'";
                SqlCommand command = new SqlCommand(sql, conn);
                command.ExecuteNonQuery();
                conn.Close();
                printtable();
                printtable2();
                MessageBox.Show("Заявка: " + s + " изменена!");
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                textBox6.Clear();
            }
        }
        void printtable()
        {
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter("select *from Заявки", conn);
            SqlCommandBuilder cb = new SqlCommandBuilder(da);
            DataSet ds = new DataSet();
            da.Fill(ds, "Заявки");
            dataGridView2.DataSource = ds.Tables[0];
            conn.Close();
        }
        void printtable2()
        {
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter("select [Номер заявки], [Дата создания],Status from Заявки", conn);
            SqlCommandBuilder cb = new SqlCommandBuilder(da);
            DataSet ds = new DataSet();
            da.Fill(ds, "Заявки");

            dataGridView4.DataSource = ds.Tables[0];
            conn.Close();
        }
        void monitorzaivok()
        {
            string lines = File.ReadAllText(writePath, System.Text.Encoding.Default);
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter("select * from USERS", conn);

            SqlCommandBuilder cb = new SqlCommandBuilder(da);
            DataSet ds = new DataSet();
            da.Fill(ds, "USERS");
            dataGridView3.DataSource = ds.Tables[0];
            conn.Close();
            for (int i = 0; i < dataGridView3.RowCount; i++)
            {
                if (lines == dataGridView3.Rows[i].Cells[1].Value.ToString())
                {
                    textBox14.Text = dataGridView3.Rows[i].Cells[0].Value.ToString();
                    textBox15.Text = dataGridView3.Rows[i].Cells[1].Value.ToString();
                    textBox16.Text = dataGridView3.Rows[i].Cells[2].Value.ToString();
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            printtablegotovzaiavki();
            groupBox2.Visible = false;
            printtable3();
            printtable2();
            printtable();
            dataGridView4.Columns[2].HeaderText = "Статус";
            dataGridView2.Columns[6].HeaderText = "Статус";
            dataGridView6.Columns[6].HeaderText = "Статус";
            dataGridView5.Columns[1].HeaderText = "ИД Менеджера";
            dataGridView5.Columns[2].HeaderText = "ФИО Менеджера";
            dataGridView5.Columns[3].HeaderText = "Логин Менеджера";
            textBox14.Enabled = false;
            textBox15.Enabled = false;
            textBox16.Enabled = false;
            monitorzaivok();
        }
        void monitorzaivokinsert()
        {

            int s = Convert.ToInt32(dataGridView5.RowCount);
            string s1 = textBox15.Text;
            string s2 = textBox16.Text;
            string s3 = textBox10.Text;

            conn.Open();
            string sql = "insert into [Монитор заявок] (ИД,Логин,ФИО, [Номер заявки]) values ('" + s + "','" + s1 + "','" + s2 + "','" + s3 + "')";
            SqlCommand command = new SqlCommand(sql, conn);
            command.ExecuteNonQuery();
            conn.Close();
            printtable3();
        }
        public void printtable3()
        {
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter("select [Номер заявки], ИД, ФИО, Логин from [Монитор заявок]", conn);
            SqlCommandBuilder cb = new SqlCommandBuilder(da);
            DataSet ds = new DataSet();
            da.Fill(ds, "[Монитор заявок]");
            dataGridView5.DataSource = ds.Tables[0];
            conn.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {       
            int count2 = 0;
            string k1 = "";
                k1 = dataGridView4.CurrentCell.Value.ToString();
                for (int i = 0; i < dataGridView5.RowCount - 1; i++)
                {

                    if (k1 == dataGridView5.Rows[i].Cells[0].Value.ToString())
                    {

                        count2++;
                    }
                }
            if(count2>0)
            {
                MessageBox.Show("Данная заявка уже в обработке!");
            }
            if (count2 == 0)
            {
                groupBox2.Visible = true;
                tabControl1.SelectTab(1);
                textBox5.Clear();
                textBox10.Clear();
                textBox11.Clear();
                textBox12.Clear();
                textBox13.Clear();
                textBox8.Clear();
                string k = dataGridView4.CurrentRow.Index.ToString();
                int index = Convert.ToInt32(k);
                string s = dataGridView2.Rows[index].Cells[4].Value.ToString();
                string s1 = dataGridView2.Rows[index].Cells[0].Value.ToString();
                string s2 = dataGridView2.Rows[index].Cells[1].Value.ToString();
                string s4 = dataGridView2.Rows[index].Cells[2].Value.ToString();
                string s5 = dataGridView2.Rows[index].Cells[3].Value.ToString();
                string s6 = dataGridView2.Rows[index].Cells[5].Value.ToString();
                textBox5.Text = s;
                textBox10.Text = s1;
                textBox11.Text = s2;
                textBox12.Text = s4;
                textBox13.Text = s5;
                textBox8.Text = s6;
                conn.Open();
                string ob = "В обработке";
                string sql = "Update Заявки set Status='" + ob + "' where [Номер заявки]='" + textBox10.Text + "'";
                SqlCommand command = new SqlCommand(sql, conn);
                command.ExecuteNonQuery();
                conn.Close();
                printtable();
                printtable2();
                monitorzaivokinsert();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            conn.Open();
            string ob = "Заявка отложена";
            string sql = "Update Заявки set Status='" + ob + "' where [Номер заявки]='" + textBox10.Text + "'";
            SqlCommand command = new SqlCommand(sql, conn);
            command.ExecuteNonQuery();
            conn.Close();
            printtable();
            printtable2();
            delotlojzaivaok();
            MessageBox.Show("Заявка: "+textBox10.Text+" отложена на 24 часа");
            textBox5.Clear();
            textBox10.Clear();
            textBox11.Clear();
            textBox12.Clear();
            textBox13.Clear();
            textBox8.Clear();
            groupBox2.Visible = false;
        }
        void delotlojzaivaok()
        {
            conn.Open();
            string s = textBox10.Text;
            string sql = "Delete from [Монитор заявок] where [Номер заявки]='" + s + "'";
            SqlCommand command = new SqlCommand(sql, conn);
            command.ExecuteNonQuery();
            conn.Close();
            printtable();
            printtable3();
            printtable2();
        }
        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {         
            string s = dataGridView5.CurrentCell.Value.ToString();
            conn.Open();
            string sql = "Delete from [Монитор заявок] where [Номер заявки]='" + s + "'";
            SqlCommand command = new SqlCommand(sql, conn);
            command.ExecuteNonQuery();
            conn.Close();
            printtable3();
            StatusojodDelzaivky(s);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            conn.Open();
            string s = dataGridView2.CurrentCell.Value.ToString();
            string sql = "Delete from Заявки where [Номер заявки]='" + s + "'";
            SqlCommand command = new SqlCommand(sql, conn);
            command.ExecuteNonQuery();
            conn.Close();
            printtable();
            printtable2();

        }
        public void StatusojodDelzaivky(string s)
        {
            conn.Open();
            string ob = "Ожидание";
            string sql = "Update Заявки set Status='" + ob + "' where [Номер заявки]='" + s + "'";
            SqlCommand command = new SqlCommand(sql, conn);
            command.ExecuteNonQuery();
            conn.Close();
            printtable();
            printtable2();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox7.Text == String.Empty)
            {
                MessageBox.Show("Введите номер заявки для поиска!!!");
            }
            else
            {
                string search = textBox7.Text;
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter("select *from Заявки where [Номер заявки] like '%" + search + "%'", conn);
                SqlCommandBuilder cb = new SqlCommandBuilder(da);
                DataSet ds = new DataSet();
                da.Fill(ds, "[Монитор заявок]");
                dataGridView1.DataSource = ds.Tables[0];
                conn.Close();
            }
        }
        void zaivprint()
        {
            conn.Open();
            int s = Convert.ToInt32(textBox10.Text);
            string s1 = textBox11.Text;
            string s2 = textBox12.Text;
            string s3 = textBox13.Text;
            string s4 = textBox5.Text;
            string s5 = textBox8.Text;
            string s6 = "Готова";
            string sql = "insert into [Готовые заявки] ([Номер заявки],ФИО,Почта, [Номер телефона], [Дата создания], Описание, Status) values ('" + s + "','" + s1 + "','" + s2 + "','" + s3 + "','" + s4 + "','" + s5 + "','" + s6 + "')";
            SqlCommand command = new SqlCommand(sql, conn);
            command.ExecuteNonQuery();
            conn.Close();
            printtable();
            printtable2();
            printtablegotovzaiavki();
        }
        void delgotovzakaz()
        {
            conn.Open();
            string s = textBox10.Text;
            string sql = "Delete from Заявки where [Номер заявки]='" + s + "'";
            SqlCommand command = new SqlCommand(sql, conn);
            command.ExecuteNonQuery();
            conn.Close();
            printtable();
            printtable2();
        }
        void delzakazojidanue()
        {
            conn.Open();
            string s = textBox10.Text;
            string sql = "Delete from [Монитор заявок] where [Номер заявки]='" + s + "'";
            SqlCommand command = new SqlCommand(sql, conn);
            command.ExecuteNonQuery();
            conn.Close();
            printtable3();
            printtable2();
        }
        public void printtablegotovzaiavki()
        {
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter("select *from [Готовые заявки]", conn);
            SqlCommandBuilder cb = new SqlCommandBuilder(da);
            DataSet ds = new DataSet();
            da.Fill(ds, "[Готовые заявки]");
            dataGridView6.DataSource = ds.Tables[0];
            conn.Close();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            MailMessage mail = new MailMessage("danir.nizamoff@yandex.ru", textBox12.Text, "Ваша заявка принята", textBox9.Text);
            SmtpClient client = new SmtpClient("smtp.yandex.ru");
            client.Port = 587;
            client.Credentials = new System.Net.NetworkCredential("danir.nizamoff@yandex.ru", "ybzplfybh12345");
            client.EnableSsl = true;
            client.Send(mail);
            conn.Open();
            string ob = "Принята";
            string sql = "Update Заявки set Status='" + ob + "' where [Номер заявки]='" + textBox10.Text + "'";
            SqlCommand command = new SqlCommand(sql, conn);
            command.ExecuteNonQuery();
            conn.Close();
            printtable();
            printtable2();
            zaivprint();
            delgotovzakaz();
            //delzakazojidanue();
            MessageBox.Show("Сообщение отправлено клиенту: "+textBox11.Text+"", "Success", MessageBoxButtons.OK);
            textBox5.Clear();
            textBox10.Clear();
            textBox11.Clear();
            textBox12.Clear();
            textBox13.Clear();
            textBox8.Clear();
            textBox9.Clear();
            groupBox2.Visible = false;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            conn.Open();
            string s = dataGridView6.CurrentCell.Value.ToString();
            string sql = "Delete from [Готовые заявки] where [Номер заявки]='" + s + "'";
            SqlCommand command = new SqlCommand(sql, conn);
            command.ExecuteNonQuery();
            conn.Close();
            printtablegotovzaiavki();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox6.Clear();
            string k1 = "";
            k1 = dataGridView4.CurrentCell.Value.ToString();
            for (int i = 0; i < dataGridView2.RowCount - 1; i++)
            {

                if (k1 == dataGridView2.Rows[i].Cells[0].Value.ToString())
                {
                    textBox4.Text = dataGridView2.Rows[i].Cells[0].Value.ToString();
                    textBox1.Text = dataGridView2.Rows[i].Cells[1].Value.ToString();
                    textBox2.Text = dataGridView2.Rows[i].Cells[2].Value.ToString();
                    textBox3.Text = dataGridView2.Rows[i].Cells[3].Value.ToString();
                    textBox6.Text = dataGridView2.Rows[i].Cells[5].Value.ToString();
                }
            }
        }
    }
}
