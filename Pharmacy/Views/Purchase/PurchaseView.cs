using MySql.Data.MySqlClient;
using Pharmacy.DB;
using Pharmacy.Model;
using Pharmacy.Views.Purchase;
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
    public partial class PurchaseView : Form
    {
        public PurchaseView()
        {
            InitializeComponent();
            GridFill();
        }
        PurchaseModel purchaseModel = new PurchaseModel();
        string id;
        private void button1_Click(object sender, EventArgs e)
        {
            MySqlCommand command = new MySqlCommand("GenerateCodePurchases", Connection.MakeConnection());
            command.Parameters.AddWithValue("_PurchasesDate", null);
            command.Parameters.AddWithValue("_PurchasesTotal", null);
            command.Parameters.AddWithValue("_PurchasesReference", null);
            command.Parameters.AddWithValue("_ProviderID", null);
            command.Parameters.AddWithValue("_automatically", true);
            command.CommandType = CommandType.StoredProcedure;
            command.ExecuteNonQuery();

            MySqlCommand mySqlCommand = new MySqlCommand("select * from Purchases", Connection.MakeConnection());
            MySqlDataReader myReader;

            myReader = mySqlCommand.ExecuteReader();

            while (myReader.Read())
            {
                id = myReader.GetString("PurchasesID");
             ///   MessageBox.Show("ID: "+id);
                
            }

            int providerID = int.Parse(id);
            AddPurchase addPurchase = new AddPurchase(providerID);
            addPurchase.ShowDialog();
            GridFill();

        }



        public void GridFill()
        {
            MySqlDataAdapter sqlData = new MySqlDataAdapter("ListPurchases", Connection.MakeConnection());
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

        private void PurchaseView_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            CanceledPurchase canceledPurchase = new CanceledPurchase();
            canceledPurchase.ShowDialog();

        }
    }
}
