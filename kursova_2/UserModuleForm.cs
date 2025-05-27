using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace kursova_2
{
    public partial class UserModuleForm : Form
    {
        public UserModuleForm()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=dblMS;Integrated Security=True");
        SqlCommand cm = new SqlCommand();
        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtPass.Text != txtRepass.Text)
                {
                    MessageBox.Show("Паролі не співпадають!", "Попередження", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (MessageBox.Show("Ви дійсно хочете зберегти цього користувача?", "Збереження запису", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string hashedPassword = PasswordHelper.HashPassword(txtPass.Text);

                    cm = new SqlCommand("INSERT INTO tbUser(username, fullname, password, phone, role) VALUES (@username, @fullname, @password, @phone, @role)", con);
                    cm.Parameters.AddWithValue("@username", txtUserName.Text);
                    cm.Parameters.AddWithValue("@fullname", txtFullName.Text);
                    cm.Parameters.AddWithValue("@password", hashedPassword);
                    cm.Parameters.AddWithValue("@phone", txtPhone.Text);
                    cm.Parameters.AddWithValue("@role", cbRole.SelectedItem.ToString());

                    con.Open();
                    cm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Користувач успішно збережений!");
                    Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
        }

        public void Clear()
        {
            txtUserName.Clear();
            txtFullName.Clear();
            txtPass.Clear();
            txtRepass.Clear();
            txtPhone.Clear();
        }
        public string oldPasswordHash = "";
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtPass.Text != txtRepass.Text)
                {
                    MessageBox.Show("Паролі не співпадають!", "Попередження", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show("Ви дійсно хочете оновити дані користувача?", "Оновлення запису", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string passwordToSave;

                    if (string.IsNullOrWhiteSpace(txtPass.Text))
                    {
                        passwordToSave = oldPasswordHash;
                    }
                    else
                    {
                        passwordToSave = PasswordHelper.HashPassword(txtPass.Text);
                    }

                    cm = new SqlCommand("UPDATE tbUser SET fullname=@fullname, password=@password, phone=@phone, role=@role WHERE username=@username", con);
                    cm.Parameters.AddWithValue("@fullname", txtFullName.Text);
                    cm.Parameters.AddWithValue("@password", passwordToSave);
                    cm.Parameters.AddWithValue("@phone", txtPhone.Text);
                    cm.Parameters.AddWithValue("@role", cbRole.SelectedItem.ToString());
                    cm.Parameters.AddWithValue("@username", txtUserName.Text);

                    con.Open();
                    cm.ExecuteNonQuery();
                    con.Close();

                    MessageBox.Show("Користувача успішно оновлено!");
                    this.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка: " + ex.Message);
            }
        }


        private void UserModuleForm_Load(object sender, EventArgs e)
        {
            cbRole.Items.Clear();
            cbRole.Items.Add("Адміністратор");
            cbRole.Items.Add("Користувач");
            cbRole.SelectedIndex = 1;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtPhone_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
