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

namespace Pharmacy.Views.Product
{
    public partial class CanceledProduct : Form
    {
        public CanceledProduct()
        {
            InitializeComponent();
            GridFill();
        }

        private void CanceledProduct_Load(object sender, EventArgs e)
        {

        }


        DataTable dtProduct;

        public void GridFill()
        {
            MySqlDataAdapter sqlData = new MySqlDataAdapter("ListProductCanceled", Connection.MakeConnection());
            sqlData.SelectCommand.CommandType = CommandType.StoredProcedure;
            dtProduct = new DataTable();
            sqlData.Fill(dtProduct);
            dataGridView1.DataSource = dtProduct;
            DataGridViewColumn column = dataGridView1.Columns[0];
            column.Visible = false;
            for (int i = 0; i <= 12; i++)
            {

                DataGridViewColumn column1 = dataGridView1.Columns[i];
                if (column1 == dataGridView1.Columns[11])
                {
                    column1.Visible = false;
                }

                if (column1 == dataGridView1.Columns[12])
                {
                    column1.Visible = false;
                }
                column1.Width = 81;
            }
        }
    }
}
