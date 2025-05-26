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


namespace kursova_2
{
    public partial class UserForm : Form
    {

        SqlConnection con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=dblMS;Integrated Security=True");
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        public UserForm()
        {
            InitializeComponent();
            LoadUser();
        }

        public void LoadUser() 
        { 
            int i = 0;
            dgvUser.Rows.Clear();
            cm = new SqlCommand("SELECT * FROM tbUser", con);
            con.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvUser.Rows.Add(i,dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString());
            }
            dr.Close();
            con.Close();

        }

        private void dgvUser_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvUser.Columns[e.ColumnIndex].Name;
            if (colName == "Edit")
            {
                UserModuleForm userModule = new UserModuleForm();
                userModule.txtUserName.Text = dgvUser.Rows[e.RowIndex].Cells[1].Value.ToString();
                userModule.txtFullName.Text = dgvUser.Rows[e.RowIndex].Cells[2].Value.ToString();
                userModule.txtPass.Text = dgvUser.Rows[e.RowIndex].Cells[3].Value.ToString();
                userModule.txtPhone.Text = dgvUser.Rows[e.RowIndex].Cells[4].Value.ToString();

                userModule.btnSave.Enabled = false;
                userModule.btnUpdate.Enabled = true;
                userModule.txtUserName.Enabled = false;
                userModule.ShowDialog();
            }
            else if (colName == "Delete")
            {
                if (MessageBox.Show("Ви впевнені, що хочете видалити цього користувача?", "Видалення запису", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    con.Open();
                    string username = dgvUser.Rows[e.RowIndex].Cells[1].Value.ToString();
                    cm = new SqlCommand("DELETE FROM tbUser WHERE username = @username", con);
                    cm.Parameters.AddWithValue("@username", username);
                    cm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Користувача успішно видалено!");
                }
            }
            LoadUser();

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            UserModuleForm userModule = new UserModuleForm();
            userModule.btnSave.Enabled = true;
            userModule.btnUpdate.Enabled = false;
            userModule.ShowDialog();
            LoadUser();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
