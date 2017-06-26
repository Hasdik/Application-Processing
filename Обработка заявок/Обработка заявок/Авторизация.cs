using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Обработка_заявок
{
    public partial class Авторизация : Form
    {
        SqlConnection connection = new SqlConnection(@"Data Source=АЙЗИЛЯ-ПК\SQLEXPRESS;Initial Catalog='Обработка заявок';Integrated Security=True");
        public Авторизация()
        {
            InitializeComponent();
        }
        string[] str;
        string[] mas=new string[50];
        string writePath = @"D:\1.txt";
        private void button1_Click(object sender, EventArgs e)
        {
            int count = 0;

            using (SqlCommand command = new SqlCommand("select * from USERS WHERE LOGIN = '" + textBox1.Text + "' AND PASSWORD = '" + textBox2.Text + "'", connection))
            {
                command.Parameters.AddWithValue("par1", textBox1.Text);
                command.Parameters.AddWithValue("par2", textBox2.Text);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        count += 1;
                }
                connection.Close();
            }
            if (count == 0)
            {
                MessageBox.Show("Пароль неверен!");
                return;
            }

            File.WriteAllText(writePath, textBox1.Text, System.Text.Encoding.Default);

            this.Hide();
            Form1 f1 = new Form1();
            f1.ShowDialog();

        }
        
        private void Авторизация_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "обработка_заявокDataSet.USERS". При необходимости она может быть перемещена или удалена.
            this.uSERSTableAdapter.Fill(this.обработка_заявокDataSet.USERS);
            string lines = File.ReadAllText(writePath, System.Text.Encoding.Default);
            str = lines.Split('\n');

        }
    }
}
