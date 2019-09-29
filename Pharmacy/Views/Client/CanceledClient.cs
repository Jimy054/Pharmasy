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

namespace Pharmacy.Views.Client
{
    public partial class CanceledClient : Form
    {

        DataTable dtClient;

        public CanceledClient()
        {
            InitializeComponent();
            GridFill();
        }

        private void ClientCanceled_Load(object sender, EventArgs e)
        {

        }

        public void GridFill()
        {
            MySqlDataAdapter sqlData = new MySqlDataAdapter("ListClientCanceled", Connection.MakeConnection());
            sqlData.SelectCommand.CommandType = CommandType.StoredProcedure;
            dtClient = new DataTable();
            sqlData.Fill(dtClient);


            dataGridView1.DataSource = dtClient;
            DataGridViewColumn column = dataGridView1.Columns[0];
            column.Visible = false;

            for (int i = 0; i < 9; i++)
            {

                DataGridViewColumn column1 = dataGridView1.Columns[i];

                column1.Width = 103;
            }

        }
    }
}
