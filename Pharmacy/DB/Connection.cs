using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
namespace Pharmacy.DB
{
    class Connection
    {


        public static MySqlConnection MakeConnection()
        {
            MySqlConnection connection;
            try
            {
                string pass = "";
                connection = new MySqlConnection("server=localhost; database=pharmasy; user id=root;password=" + pass);
               connection.Open();
              //  connection.Close();
                return connection;
              
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Se ha producido un error "+ex);
            }

            return connection = new MySqlConnection();
        }
    }
}
