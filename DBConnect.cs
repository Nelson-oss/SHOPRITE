using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SHOPRITE
{
    class DBConnect
    {

        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        private string con;
        public string myConnection()
        {
            con = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\ProgramData\DATABASESHOPRITE.mdf;Integrated Security=True;Connect Timeout=30";
            return con;
        }
        public DataTable getTable(string qury)
        {
          cn.ConnectionString = myConnection();
          cm = new SqlCommand(qury, cn);
            SqlDataAdapter adapter = new SqlDataAdapter(cm);
          DataTable table = new DataTable();
          adapter.Fill(table);
          return table;



        }

        public void ExecuteQuery(String sql)
        {
            try
            {
                cn.ConnectionString = myConnection();
                cn.Open();
                cm = new SqlCommand(sql, cn);
                cm.ExecuteNonQuery();
                cn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
          
        }
    }
}
