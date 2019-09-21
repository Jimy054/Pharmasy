using MySql.Data.MySqlClient;
using Pharmacy.DB;
using Pharmacy.Views.Sale;
using Pharmacy.Views.Sales;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pharmacy.Views
{
    public partial class SalesView : Form
    {
        public SalesView()
        {
            InitializeComponent();
            GridFill();
        }
        string id;

        private void button1_Click(object sender, EventArgs e)
        {

        //    Connection.MakeConnection().Open();
                 MySqlCommand command = new MySqlCommand("GenerateCodeSales", Connection.MakeConnection());
                command.Parameters.AddWithValue("_SalesDate", null);
                command.Parameters.AddWithValue("_SalesTotal", null);
                command.Parameters.AddWithValue("_SalesReference", null);
                command.Parameters.AddWithValue("_ClientID", null);
                command.CommandType = CommandType.StoredProcedure;
                command.ExecuteNonQuery();
            //    Connection.MakeConnection().Close();
              //  Connection.MakeConnection().Open();
                MySqlCommand mySqlCommand = new MySqlCommand("select * from Sales", Connection.MakeConnection());
                MySqlDataReader myReader;
                myReader = mySqlCommand.ExecuteReader();
                while (myReader.Read())
                {
                     id = myReader.GetString("SalesID");
                }
                int salesID = int.Parse(id);
                AddSale addPurchase = new AddSale(salesID);
                addPurchase.ShowDialog();
                GridFill();                   
        }



        public void GridFill()
        {
            try
            {
                MySqlDataAdapter sqlData = new MySqlDataAdapter("ListSales", Connection.MakeConnection());
                sqlData.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dtProduct = new DataTable();
                sqlData.Fill(dtProduct);


                dataGridView1.DataSource = dtProduct;
                DataGridViewColumn column = dataGridView1.Columns[0];
                column.Visible = false;

                for (int i = 0; i < 5; i++)
                {
                    DataGridViewColumn column1 = dataGridView1.Columns[i];
                    column1.Width = 188;
                }
            }
            finally
            {
                Connection.MakeConnection().Close();       
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {

            var f2 = new SaleCanceled();
            f2.StartPosition = FormStartPosition.Manual;
            f2.Location = this.Location;
            f2.Size = this.Size;
            f2.Show();

        }
    }



}
