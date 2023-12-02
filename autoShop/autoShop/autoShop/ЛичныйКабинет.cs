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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace autoShop
{
    public partial class ЛичныйКабинет : Form
    {
        RegisteredUser CurrentUser;
        public ЛичныйКабинет(RegisteredUser currentUser)
        {
            InitializeComponent();
            CurrentUser = currentUser;

            SqlConnection sqlConnect = new SqlConnection("Data Source=localhost;Initial Catalog = autoShop; Integrated Security = True");

            try
            {
                sqlConnect.Open();
                SqlCommand da = new SqlCommand("select * from Клиент where UserID = @login", sqlConnect);
                da.Parameters.AddWithValue("@login", CurrentUser.UserID);
                try
                {
                    using (SqlDataReader reader = da.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string lastName = reader["LastName"].ToString();
                            string name = reader["Name"].ToString();
                            string patronimic = reader["Patronimic"].ToString();
                            string phone = reader["Phone"].ToString();
                            string sign = reader["Sign"].ToString();
                            string bank = reader["Bank"].ToString();
                            string account = reader["Account"].ToString();

                            // Отображение изображения на форме
                            textBox1.Text = lastName;
                            textBox2.Text = name;
                            textBox3.Text = patronimic;
                            textBox4.Text = phone;
                            textBox5.Text = bank;
                            textBox6.Text = account;
                            if(sign == "1")
                            {
                                textBox7.Text = "Физическое Лицо";
                            }
                            else
                            {
                                textBox7.Text = "Юридическое Лицо";
                            }
                        }
                        else
                        {
                            MessageBox.Show("Изображение не найдено.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Опибка при входе: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            МенюПокупателя brForm = new МенюПокупателя(CurrentUser);
            brForm.Show();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Заказы orders = new Заказы(CurrentUser);
            orders.Show();
            this.Close();
        }
    }
}
