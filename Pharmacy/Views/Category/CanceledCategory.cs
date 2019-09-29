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

namespace Pharmacy.Views.Category
{
    public partial class CanceledCategory : Form
    {
        public CanceledCategory()
        {
            InitializeComponent();
            GridFill();
        }
        DataTable dtCategory;


        public void GridFill()
        {
            try
            {
                MySqlDataAdapter sqlData = new MySqlDataAdapter("listcategorycanceled", Connection.MakeConnection());
                sqlData.SelectCommand.CommandType = CommandType.StoredProcedure;
                dtCategory = new DataTable();
                sqlData.Fill(dtCategory);


                dataGridView1.DataSource = dtCategory;
                DataGridViewColumn column = dataGridView1.Columns[0];
                column.Visible = false;
                DataGridViewColumn column1 = dataGridView1.Columns[1];
                DataGridViewColumn column2 = dataGridView1.Columns[2];
                column1.Width = 427;
                column2.Width = 427;

                //     80.Columns.GetColumnsWidth();

            }
            finally 
            {

                Connection.MakeConnection().Close();
            }

        }

        private void CategoryCanceled_Load(object sender, EventArgs e)
        {

        }
    }
}
