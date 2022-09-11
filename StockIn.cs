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

namespace SHOPRITE
{
    public partial class StockIn : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        SqlDataReader dr;
        string stitle = "SHOPRITE";
        public StockIn()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.myConnection());
            loadSupplier();
            GetRefNo();
        }

        public void GetRefNo()
        {
            Random rnd = new Random();
            txtRefNo.Clear();
            txtRefNo.Text += rnd.Next();
        }
        public void loadSupplier()
        {
            cbSupplier.Items.Clear();
            cbSupplier.DataSource =dbcon.getTable("SELECT * FROM tbSupplier");
            cbSupplier.DisplayMember = "supplier";
        }

        public void loadStockIn()
        {
            int i = 0;  
            dgvStockIn.Rows.Clear();
            cn.Open();
            cm = new SqlCommand("SELECT * FROM vwStockIn WHERE refno LIKE '" + txtRefNo.Text + "' AND status LIKE 'Pending'", cn);
            dr= cm.ExecuteReader();
            while(dr.Read())
            {
                i++;
                dgvStockIn.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString(), dr[7].ToString());

                    
            }
            dr.Close();
            cn.Close();
        }


        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)

        {
            try 
            {
                if(dgvStockIn.Rows.Count >0)
                { 
                    if (MessageBox.Show("Are you sure you want to save these records?", stitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        for(int i= 0; i < dgvStockIn.Rows.Count; i++)
                        {
                            //updating product quantity
                            cn.Open();
                            cm = new SqlCommand("UPDATE tbProduct SET qty = qty + " + int.Parse(dgvStockIn.Rows[i].Cells[5].Value.ToString()) + "WHERE pcode LIKE '" + dgvStockIn.Rows[i].Cells[3].Value.ToString() + "'", cn);
                            cm.ExecuteNonQuery();
                            cn.Close();

                            //updtaing stockin quantity
                            cn.Open();
                            cm = new SqlCommand("UPDATE tbStockIn SET qty = qty + " + int.Parse(dgvStockIn.Rows[i].Cells[5].Value.ToString()) + ", status='Done' WHERE id LIKE '" + dgvStockIn.Rows[i].Cells[1].Value.ToString() + "'", cn);
                            cm.ExecuteNonQuery();
                            cn.Close();
                        }
                        Clear();
                        loadStockIn();
                    }
                }
            }
            catch(Exception ex)
            {

                MessageBox.Show(ex.Message, stitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        private void cbSupplier_SelectedIndexChanged(object sender, EventArgs e)
        {
            cn.Open();
            cm = new SqlCommand("SELECT * FROM tbSupplier WHERE supplier LIKE'" + cbSupplier.Text +"'", cn);
            dr = cm.ExecuteReader();
            dr.Read();
            if(dr.HasRows)
            {
                lblId.Text = dr["id"].ToString();
                txtConPerson.Text = dr["contactperson"].ToString();
                txtAddress.Text = dr["address"].ToString();
            }
            dr.Close();
            cn.Close();
        }

        private void cbSupplier_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void LinGenerate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            GetRefNo();
        }

        private void LinProduct_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProductStockIn productStock = new ProductStockIn(this);
            productStock.ShowDialog();
        }

        public void Clear ()
        {
            txtRefNo.Clear();
            txtStockInBy.Clear();
            dtStockIn.Value  = DateTime.Now;
        }

        private void dgvStockIn_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvStockIn.Columns[e.ColumnIndex].Name;
            if (colName == "Delete")
            {
                if (MessageBox.Show("Remove this item", stitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("DELETE FROM tbStockIn WHERE id='" + dgvStockIn.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", cn);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Item has been removed successfully", stitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadStockIn();
                }
            }
        }
    }

}
