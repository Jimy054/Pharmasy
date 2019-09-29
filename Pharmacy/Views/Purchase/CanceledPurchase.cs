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

namespace Pharmacy.Views.Purchase
{
    public partial class CanceledPurchase : Form
    {
        public CanceledPurchase()
        {
            InitializeComponent();
            GridFill();
        }

        private void CanceledPurchase_Load(object sender, EventArgs e)
        {

        }


        public void GridFill()
        {
            MySqlDataAdapter sqlData = new MySqlDataAdapter("ListPurchasesCanceled", Connection.MakeConnection());
            sqlData.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dtProduct = new DataTable();
            sqlData.Fill(dtProduct);


            dataGridView1.DataSource = dtProduct;
            DataGridViewColumn column = dataGridView1.Columns[0];
            column.Visible = false;

            for (int i = 0; i < 5; i++)
            {
                DataGridViewColumn column1 = dataGridView1.Columns[i];
                column1.Width = 157;
            }
        }



    }
}
