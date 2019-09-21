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
    public partial class UpdateProvider : Form
    {

        ProviderModel providerModel = new ProviderModel();

        public UpdateProvider(int id, string name, string address, string telephone, string telephone2, string email, string methodpayment, string nit, string code)
        {
            InitializeComponent();
            providerModel.ProviderID = id;
            txtName.Text = name;
            txtAddress.Text = address;
            txtTelephone.Text = telephone;
            txtTelephone2.Text = telephone2;
            txtEmail.Text = email;
            cmbMethodPayment.Text = methodpayment;
            txtNIT.Text = nit;
            txtCode.Text = code;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            MySqlCommand commandUpdate = new MySqlCommand("UpdateProviders", Connection.MakeConnection());
            providerModel.Name = txtName.Text;
            providerModel.Address = txtAddress.Text;
            providerModel.Telephone = txtTelephone.Text;
            providerModel.Telephone2 = txtTelephone2.Text;
            providerModel.Email = txtEmail.Text;
            providerModel.MethodPayment = cmbMethodPayment.Text;
            providerModel.NIT = txtNIT.Text;
            providerModel.Code = txtCode.Text;
            commandUpdate.CommandType = CommandType.StoredProcedure;
            commandUpdate.Parameters.AddWithValue("_ProviderID", providerModel.ProviderID);
            commandUpdate.Parameters.AddWithValue("_code", providerModel.Code);
            commandUpdate.Parameters.AddWithValue("_name", providerModel.Name);
            commandUpdate.Parameters.AddWithValue("_address", providerModel.Address);
            commandUpdate.Parameters.AddWithValue("_telephone", providerModel.Telephone);
            commandUpdate.Parameters.AddWithValue("_telephone2", providerModel.Telephone2);
            commandUpdate.Parameters.AddWithValue("_email", providerModel.Email);
            commandUpdate.Parameters.AddWithValue("_methodPayment", providerModel.MethodPayment);
            commandUpdate.Parameters.AddWithValue("_nit", providerModel.NIT);
            try
            {
                commandUpdate.ExecuteNonQuery();
                MessageBox.Show("Registro Actualizado Exitosamente", "Registro Actualizado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception)
            {

                MessageBox.Show("Código o NIT Repetido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
