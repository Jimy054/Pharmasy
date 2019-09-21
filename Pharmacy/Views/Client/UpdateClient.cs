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
    public partial class UpdateClient : Form
    {
        ClientModel clientModel = new ClientModel();
        public UpdateClient(int id, string name,string address, string telephone, string telephone2, string email, string methodpayment,string nit, string  code)
        {
            InitializeComponent();
            clientModel.ClientID = id;
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
            MySqlCommand commandUpdate = new MySqlCommand("UpdateClient", Connection.MakeConnection());
            clientModel.Name = txtName.Text;
            clientModel.Address = txtAddress.Text;
            clientModel.Telephone = txtTelephone.Text;
            clientModel.Telephone2 = txtTelephone2.Text;
            clientModel.Email = txtEmail.Text;
            clientModel.MethodPayment = cmbMethodPayment.Text;
            clientModel.NIT = txtNIT.Text;
            clientModel.Code = txtCode.Text;
            commandUpdate.CommandType = CommandType.StoredProcedure;
            commandUpdate.Parameters.AddWithValue("_ClientID", clientModel.ClientID);
            commandUpdate.Parameters.AddWithValue("_code", clientModel.Code);
            commandUpdate.Parameters.AddWithValue("_name", clientModel.Name);
            commandUpdate.Parameters.AddWithValue("_address", clientModel.Address);
            commandUpdate.Parameters.AddWithValue("_telephone", clientModel.Telephone);
            commandUpdate.Parameters.AddWithValue("_telephone2", clientModel.Telephone2);
            commandUpdate.Parameters.AddWithValue("_email", clientModel.Email);
            commandUpdate.Parameters.AddWithValue("_methodPayment", clientModel.MethodPayment);
            commandUpdate.Parameters.AddWithValue("_nit", clientModel.NIT);
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
