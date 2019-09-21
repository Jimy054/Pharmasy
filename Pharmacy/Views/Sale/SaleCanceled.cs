using MySql.Data.MySqlClient;
using Pharmacy.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pharmacy.Views.Sale
{
    public partial class SaleCanceled : Form
    {
        public SaleCanceled()
        {
            InitializeComponent();
            GridFill();
        }



        public void GridFill()
        {
            try
            {
                MySqlDataAdapter sqlData = new MySqlDataAdapter("ListSalesCanceled", Connection.MakeConnection());
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

        private void SaleCanceled_Load(object sender, EventArgs e)
        {

        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            SalesView salesView = new SalesView();
            salesView.ShowDialog();
        }
    }
}
