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
            button4.Visible = false;
            button5.Visible = false;
            dataGridView1.Visible = false;
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
                            textBox4.Text = ("+7" + phone);
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
                            reader.Close();

                            SqlCommand dataAdapterCommand = new SqlCommand("select * from Клиент where UserID = @login", sqlConnect);
                            dataAdapterCommand.Parameters.AddWithValue("@login", CurrentUser.UserID);

                            SqlDataAdapter adapter = new SqlDataAdapter(dataAdapterCommand);
                            DataTable dataTable = new DataTable();

                            // Заполняем таблицу данными из базы данных
                            adapter.Fill(dataTable);

                            // Связываем DataTable с DataGridView
                            dataGridView1.DataSource = dataTable;
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

        private void button3_Click(object sender, EventArgs e)
        {
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            button4.Visible = true;
            button5.Visible = true;

            groupBox1.Visible = false;
            groupBox2.Visible = false;
            dataGridView1.Visible = true;

            this.Size = new Size(671, 213);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            
            button1.Visible = true;
            button2.Visible = true;
            button3.Visible = true;
            button4.Visible = false;
            button5.Visible = false;

            groupBox1.Visible = true;
            groupBox2.Visible = true;
            dataGridView1.Visible = false;

            this.Size = new Size(445, 213);
        }

        private void ЛичныйКабинет_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "autoShopDataSet.Клиент". При необходимости она может быть перемещена или удалена.
            this.клиентTableAdapter.Fill(this.autoShopDataSet.Клиент);

        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                Validate();
                клиентBindingSource.EndEdit();
                клиентTableAdapter.Update(autoShopDataSet);
                autoShopDataSet.AcceptChanges();
                MessageBox.Show("Данные успешно обновлены");
            }
            catch (SystemException ex)
            {
                MessageBox.Show("Не удалось обновить данные" + ex.ToString());
                return;
            }
        }
    }
}
