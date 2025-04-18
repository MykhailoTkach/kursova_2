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

        }

        private void customerButton1_Click(object sender, EventArgs e)
        {
            UserModuleForm userModule = new UserModuleForm();
            userModule.btnSave.Enabled = true;
            userModule.btnUpdate.Enabled = false;
            userModule.ShowDialog();
        }
    }
}
