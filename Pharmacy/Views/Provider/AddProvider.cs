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

namespace Pharmacy.Views.Provider
{
    public partial class AddProvider : Form
    {

        ProviderModel providerModel = new ProviderModel();
        
        public AddProvider()
        {
            InitializeComponent();
        }




        private void btnNew_Click(object sender, EventArgs e)
        {

            MySqlCommand command = new MySqlCommand("GenerateCodeProviders", Connection.MakeConnection());
            providerModel.Name = txtName.Text;
            providerModel.Address = txtAddress.Text;
            providerModel.Telephone = txtTelephone.Text;
            providerModel.Telephone2 = txtTelephone2.Text;
            providerModel.Email = txtEmail.Text;
            providerModel.MethodPayment = cmbMethodPayment.Text;
            providerModel.NIT = txtNIT.Text;
            providerModel.Code = txtCode.Text;

            if (checkBox1.Checked)
            {
                providerModel.Code = "";
                command.CommandType = CommandType.StoredProcedure;
               
                    command.Parameters.AddWithValue("_code", providerModel.Code);
                    command.Parameters.AddWithValue("_name", providerModel.Name);
                    command.Parameters.AddWithValue("_address", providerModel.Address);
                    command.Parameters.AddWithValue("_telephone", providerModel.Telephone);
                    command.Parameters.AddWithValue("_telephone2", providerModel.Telephone2);
                    command.Parameters.AddWithValue("_email", providerModel.Email);
                    command.Parameters.AddWithValue("_methodPayment", providerModel.MethodPayment);
                    command.Parameters.AddWithValue("_nit", providerModel.NIT);
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
                    command.Parameters.AddWithValue("_code", providerModel.Code);
                    command.Parameters.AddWithValue("_name", providerModel.Name);
                    command.Parameters.AddWithValue("_address", providerModel.Address);
                    command.Parameters.AddWithValue("_telephone", providerModel.Telephone);
                    command.Parameters.AddWithValue("_telephone2", providerModel.Telephone2);
                    command.Parameters.AddWithValue("_email", providerModel.Email);
                    command.Parameters.AddWithValue("_methodPayment", providerModel.MethodPayment);
                    command.Parameters.AddWithValue("_nit", providerModel.NIT);
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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                txtCode.ReadOnly = true;

            }
            else
            {
                txtCode.ReadOnly = false;
            }
        }
    }
}
