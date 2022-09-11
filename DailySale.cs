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
    public partial class DailySale : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        SqlDataReader dr;
        public DailySale()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.myConnection());
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        public void loadCashier()
            {
            cboCashier.Items.Clear();
            cboCashier.Items.Add("All Cashier");
            cn.Open();
            cm = new SqlCommand("SELECT * FROM tbUser WHERE role LIKE 'Cashier'", cn);
            dr = cm.ExecuteReader();
            while(dr.Read())
            {
                cboCashier.Items.Add(dr["username"].ToString());
            }
            dr.Close();
            cn.Close();
               
            }
        public void loadSold()
        {
            int i = 0;
            dgvSold.Rows.Clear();
            cn.Open();
            if(cboCashier.Text=="All Cashier")
            {
                cm = new SqlCommand("select c.id, c.transno, c.pcode, p.pdesc, c.price, c.qty, c.disc, c.total from tbCart as c inner join tbProduct as p on c.pcode = p.pcode where status like 'Sold' between '" + dtFrom.Value + "' and '" + dtTo.Value + "'", cn);
            }
            else
            {
                cm = new SqlCommand("select c.id, c.transno, c.pcode, p.pdesc, c.price, c.qty, c.disc, c.total from tbCart as c inner join tbProduct as p on c.pcode = p.pcode where status like 'Sold' between '" + dtFrom.Value + "' and '" + dtTo.Value + "' and cashier like '" + cboCashier.Text + "'", cn);


            }
            dr = cm.ExecuteReader();
            while(dr.Read())
            {
                i++;
                dgvSold.Rows.Add(i, dr["id"].ToString(), dr["transno"].ToString(), dr["pcode"].ToString(), dr["pdesc"].ToString(), dr["price"].ToString(), dr["qty"].ToString(), dr["disc"].ToString(), dr["total"].ToString());
            }
            dr.Close();
            cn.Close();
        }

        private void cboCashier_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadSold();
        }

        private void dtFrom_ValueChanged(object sender, EventArgs e)
        {
            loadSold();
        }

        private void dtTo_ValueChanged(object sender, EventArgs e)
        {
            loadSold();
        }
    }
}
