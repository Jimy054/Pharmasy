using MySql.Data.MySqlClient;
using Pharmacy.DB;
using Pharmacy.Model;
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
    public partial class AddClient : Form
    {
        public AddClient()
        {
            InitializeComponent();
        }

        ClientModel clientModel = new ClientModel();


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                txtCode.ReadOnly = true;
                txtCode.ReadOnly = true;

            }
            else
            {

                txtCode.ReadOnly = false;
                txtCode.ReadOnly = false;
            }

        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            clientModel.Name = txtName.Text;
            clientModel.Address = txtAddress.Text;
            clientModel.Telephone = txtTelephone.Text;
            clientModel.Telephone2 = txtTelephone2.Text;
            clientModel.Email = txtEmail.Text;
            clientModel.MethodPayment = cmbMethodPayment.Text;
            clientModel.NIT = txtNIT.Text;
            clientModel.Code = txtCode.Text;
            MySqlCommand command = new MySqlCommand("GenerateCodeClients", Connection.MakeConnection());


            if (checkBox1.Checked)
            {
                clientModel.Code = "";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("_code", clientModel.Code);
                command.Parameters.AddWithValue("_name", clientModel.Name);
                command.Parameters.AddWithValue("_address", clientModel.Address);
                command.Parameters.AddWithValue("_telephone", clientModel.Telephone);
                command.Parameters.AddWithValue("_telephone2", clientModel.Telephone2);
                command.Parameters.AddWithValue("_email", clientModel.Email);
                command.Parameters.AddWithValue("_methodPayment", clientModel.MethodPayment);
                command.Parameters.AddWithValue("_nit", clientModel.NIT);
                command.Parameters.AddWithValue("_automatically", false);
                try
                {
                    command.ExecuteNonQuery();
                    MessageBox.Show("Registro Agregado Exitosamente", "Registro Agregado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                catch (Exception)
                {


                    MessageBox.Show("Código o NIT Repetido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


            }
            else
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("_code", clientModel.Code);
                command.Parameters.AddWithValue("_name", clientModel.Name);
                command.Parameters.AddWithValue("_address", clientModel.Address);
                command.Parameters.AddWithValue("_telephone", clientModel.Telephone);
                command.Parameters.AddWithValue("_telephone2", clientModel.Telephone2);
                command.Parameters.AddWithValue("_email", clientModel.Email);
                command.Parameters.AddWithValue("_methodPayment", clientModel.MethodPayment);
                command.Parameters.AddWithValue("_nit", clientModel.NIT);
                command.Parameters.AddWithValue("_automatically", true);
                try
                {
                    command.ExecuteNonQuery();
                    MessageBox.Show("Registro Agregado Exitosamente", "Registro Agregado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                catch (Exception)
                {
                    MessageBox.Show("Código o NIT Repetido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


            }
        }
    }
}
