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
    public partial class CustomerModuleForm : Form
    {
        public CustomerModuleForm()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=dblMS;Integrated Security=True");
        SqlCommand cm = new SqlCommand();
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to save this customer?", "Saving Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    con.Open();
                    cm = new SqlCommand("INSERT INTO tbCustomer(cname, cphone) VALUES (@cname, @cphone)", con);
                    cm.Parameters.AddWithValue("@cname", txtCName.Text);
                    cm.Parameters.AddWithValue("@cphone", txtCPhone.Text);
                    con.Open();
                    cm.ExecuteNonQuery(); 
                    con.Close();
                    MessageBox.Show("Customer has been successfully saved!");
                    Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CustomerModuleForm_Load(object sender, EventArgs e)
        {

        }
        private void Clear()
        {
            txtCName.Clear();
            txtCPhone.Clear();
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void txtCName_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to update this Customer?", "Update Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cm = new SqlCommand("Update tbCustomer SET cname=@cname, cphone=@cphone Where cid LIKE'" + lblCId.Text + "' ", con);
                    cm.Parameters.AddWithValue("@cname", txtCName.Text);
                    cm.Parameters.AddWithValue("@cphone", txtCPhone.Text);
                    con.Open();
                    cm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Customer has been successfully updated!");
                    this.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void lblCId_Click(object sender, EventArgs e)
        {

        }
    }
}
