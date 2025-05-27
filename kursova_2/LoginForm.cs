using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace kursova_2
{
    public partial class LoginForm : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=dbLMS;Integrated Security=True");
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;

        public LoginForm()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void checkBoxPass_CheckedChanged(object sender, EventArgs e)
        {
            txtPass.UseSystemPasswordChar = !checkBoxPass.Checked;
        }

        private void lblClear_Click(object sender, EventArgs e)
        {
            txtName.Clear();
            txtPass.Clear();
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вийти з програми?", "Підтвердження", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                cm = new SqlCommand("SELECT fullname, password, role FROM tbUser WHERE username = @username", con);
                cm.Parameters.AddWithValue("@username", txtName.Text);
                dr = cm.ExecuteReader();

                if (dr.Read())
                {
                    string storedHash = dr["password"].ToString();
                    string fullName = dr["fullname"].ToString();
                    string role = dr["role"].ToString();

                    bool isPasswordValid = PasswordHelper.VerifyPassword(txtPass.Text, storedHash);

                    if (isPasswordValid)
                    {
                        MessageBox.Show($"Ласкаво просимо, {fullName}! Ваша роль: {role}", "Доступ дозволено", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Hide();
                        MainForm main = new MainForm(fullName, txtName.Text, role);  
                        main.ShowDialog();
                        this.Close(); 
                    }
                    else
                    {
                        MessageBox.Show("Невірний пароль!", "Доступ заборонено", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Користувача не знайдено!", "Доступ заборонено", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                dr.Close();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
            }
        }
        private void txtPass_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
