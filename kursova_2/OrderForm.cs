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
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace kursova_2
{
    public partial class OrderForm : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=dblMS;Integrated Security=True");
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        public OrderForm()
        {
            InitializeComponent();
            LoadOrder();
        }
        
        public void LoadOrder()
        {
            double total = 0;
            int i = 0;
            dgvOrder.Rows.Clear();
            cm = new SqlCommand("SELECT orderid, odate, O.pid, P.pname, O.cid, C.cname, qty, price, total  FROM tbOrder AS O JOIN tbCustomer AS C ON O.cid=C.cid JOIN tbProduct AS P ON O.pid=P.pid WHERE CONCAT ( orderid, odate, O.pid, P.pname, O.cid, C.cname, qty, price) LIKE '%"+txtSearch.Text+"%'", con);
            con.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvOrder.Rows.Add(i, dr[0].ToString(),Convert.ToDateTime(dr[1].ToString()).ToString("dd/MM/yyyy"), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString(), dr[7].ToString(), dr[8].ToString());
                total += Convert.ToInt32(dr[8].ToString());
            }
            dr.Close();
            con.Close();

            lblQty.Text = i.ToString();
            lblTotal.Text = total.ToString();
        }
        private void dgvOrder_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvOrder.Columns[e.ColumnIndex].Name;

            if (colName == "Delete")
            {
                if (MessageBox.Show("Ви впевнені, що хочете видалити цей запис?", "Видалення запису", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    con.Open();
                    cm = new SqlCommand("DELETE FROM tbOrder WHERE orderid LIKE'" + dgvOrder.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", con);
                    cm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Запис успішно видалено!", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    cm = new SqlCommand("UPDATE tbProduct SET pqty=(pqty+@pqty) WHERE pid LIKE'" + dgvOrder.Rows[e.RowIndex].Cells[3].Value.ToString() + "'", con);
                    cm.Parameters.AddWithValue("@pqty", Convert.ToInt16(dgvOrder.Rows[e.RowIndex].Cells[5].Value.ToString()));

                    con.Open();
                    cm.ExecuteNonQuery();
                    con.Close();
                }
            }
            else if (colName == "Download")
            {
                string orderId = dgvOrder.Rows[e.RowIndex].Cells[1].Value.ToString();
                string orderDate = dgvOrder.Rows[e.RowIndex].Cells[2].Value.ToString();
                string productId = dgvOrder.Rows[e.RowIndex].Cells[3].Value.ToString();
                string productName = dgvOrder.Rows[e.RowIndex].Cells[4].Value.ToString();
                string customerId = dgvOrder.Rows[e.RowIndex].Cells[5].Value.ToString();
                string customerName = dgvOrder.Rows[e.RowIndex].Cells[6].Value.ToString();
                string qty = dgvOrder.Rows[e.RowIndex].Cells[7].Value.ToString();
                string price = dgvOrder.Rows[e.RowIndex].Cells[8].Value.ToString();
                string total = dgvOrder.Rows[e.RowIndex].Cells[9].Value.ToString();

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "PDF Files (*.pdf)|*.pdf";
                sfd.FileName = $"Фактура_{orderId}.pdf";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    Document doc = new Document(PageSize.A4);
                    PdfWriter.GetInstance(doc, new FileStream(sfd.FileName, FileMode.Create));
                    doc.Open();

                    string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
                    BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont, 12);
                    iTextSharp.text.Font titleFont = new iTextSharp.text.Font(baseFont, 18, iTextSharp.text.Font.BOLD);

                    Paragraph title = new Paragraph("ФАКТУРА", titleFont);
                    title.Alignment = Element.ALIGN_CENTER;
                    doc.Add(title);
                    doc.Add(new Paragraph("\n", font));

                    PdfPTable table = new PdfPTable(2);
                    table.WidthPercentage = 100;

                    table.AddCell(new PdfPCell(new Phrase("Номер замовлення:", font)));
                    table.AddCell(new PdfPCell(new Phrase(orderId, font)));
                    table.AddCell(new PdfPCell(new Phrase("Дата:", font)));
                    table.AddCell(new PdfPCell(new Phrase(orderDate, font)));
                    table.AddCell(new PdfPCell(new Phrase("Клієнт:", font)));
                    table.AddCell(new PdfPCell(new Phrase($"{customerName} (ID: {customerId})", font)));
                    table.AddCell(new PdfPCell(new Phrase("Товар:", font)));
                    table.AddCell(new PdfPCell(new Phrase($"{productName} (ID: {productId})", font)));
                    table.AddCell(new PdfPCell(new Phrase("Кількість:", font)));
                    table.AddCell(new PdfPCell(new Phrase(qty, font)));
                    table.AddCell(new PdfPCell(new Phrase("Ціна за одиницю:", font)));
                    table.AddCell(new PdfPCell(new Phrase($"{price} грн", font)));
                    table.AddCell(new PdfPCell(new Phrase("Загальна сума:", font)));
                    table.AddCell(new PdfPCell(new Phrase($"{total} грн", font)));

                    doc.Add(table);
                    doc.Add(new Paragraph("\nПідпис менеджера: ________________________", font));

                    doc.Close();

                    MessageBox.Show("Фактура збережена у форматі PDF!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    System.Diagnostics.Process.Start(sfd.FileName);
                }
            }

            LoadOrder();
        }





        private void btnAdd_Click(object sender, EventArgs e)
        {
            OrderModuleForm moduleForm = new OrderModuleForm();
            moduleForm.ShowDialog();
            LoadOrder();
        }


        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadOrder();
        }
    }
}
