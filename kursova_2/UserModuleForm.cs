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
                if (MessageBox.Show("Are you sure you want to save this user?", "Saving Record",MessageBoxButtons.YesNo,MessageBoxIcon.Question)== DialogResult.Yes)
                {
                    cm = new SqlCommand("INSERT INTO tbUser(username, fullname, password, phone) VALUES (@username, @fullname, @password, @phone)", con);
                    cm.Parameters.AddWithValue("@username", txtUserName.Text);
                    cm.Parameters.AddWithValue("@fullname", txtFullName.Text);
                    cm.Parameters.AddWithValue("@password", txtPass.Text);
                    cm.Parameters.AddWithValue("@phone", txtPhone.Text);
                    con.Open();
                    cm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Дані успішно збережено!");
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
        }

        public void Clear()
        {
            txtUserName.Clear();
            txtFullName.Clear();
            txtPass.Clear();
            txtPhone.Clear();
        }
    }
}
