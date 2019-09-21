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

namespace Pharmacy.Views.Menu
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            DashboardData();
        }



        public void DashboardData()
        {
            MySqlCommand command = new MySqlCommand("Dashboard", Connection.MakeConnection());
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@totalClient", MySqlDbType.Int16).Direction =ParameterDirection.Output;
            command.Parameters.Add("@totalProvider",MySqlDbType.Int16).Direction = ParameterDirection.Output ;
            command.Parameters.Add("@totalProduct",MySqlDbType.Int16).Direction = ParameterDirection.Output ;
            command.Parameters.Add("@totalSale",MySqlDbType.Decimal).Direction = ParameterDirection.Output ;
            command.Parameters.Add("@totalPurchase", MySqlDbType.Decimal).Direction = ParameterDirection.Output;
            command.ExecuteNonQuery();
            lblClient.Text = command.Parameters["@totalClient"].Value.ToString();

            lblProvider.Text = command.Parameters["@totalProvider"].Value.ToString();

            lblProduct.Text = command.Parameters["@totalProduct"].Value.ToString();

            lblTotalVentas.Text = command.Parameters["@totalSale"].Value.ToString();

            lblTotalCompras.Text = command.Parameters["@totalPurchase"].Value.ToString();
            Connection.MakeConnection().Close();
        }
    }
}
