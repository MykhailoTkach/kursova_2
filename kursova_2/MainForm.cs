using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kursova_2
{
    public partial class MainForm : Form
    {
        private string currentUser;
        private string currentUsername;
        private string currentRole;

        public MainForm(string fullName, string username, string role)
        {
            InitializeComponent();

            currentUser = fullName;
            currentUsername = username;
            currentRole = role;

            lblWelcome.Text = $"Вітаю, {currentUser} ({currentRole})";

            ConfigureUIByRole(currentRole);
        }

        public MainForm()
        {
            InitializeComponent();
        }

        private void ConfigureUIByRole(string role)
        {
            if (role == "Користувач")
            {
                btnCategory.Visible = false;
            }
            else if (role == "Адміністратор")
            {
                btnUser.Visible = true;
                btnCategory.Visible = true;
            }
        }

        private Form activeForm = null;

        private void openChildForm(Form childForm)
        {
            if (activeForm != null)
                activeForm.Close();
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelMain.Controls.Add(childForm);
            panelMain.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            openChildForm(new UserForm());
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            openChildForm(new CustomerForm());
        }

        private void btnCategory_Click(object sender, EventArgs e)
        {
            openChildForm(new CategoryForm());
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            openChildForm(new OrderForm());
        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            openChildForm(new ProductForm());
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panelMain_Paint(object sender, PaintEventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
