using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Pharmacy.DB;
using Pharmacy.Model;

namespace Pharmacy.Views
{
    public partial class ProviderView : Form
    {
        public ProviderView()
        {
            InitializeComponent();
            GridFill();
        }
        ProviderModel providerModel = new ProviderModel();
        MySqlCommand command = new MySqlCommand("GenerateCodeProviders", Connection.MakeConnection());

        public void Clear()
        {
            txtAddress.Text = "";
            txtCode.Text = "";
            txtEmail.Text = "";
            txtName.Text = "";
            txtNIT.Text = "";
            txtTelephone.Text = "";
            txtTelephone2.Text = "";
            cmbMethodPayment.Text = "";
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            try
            {

                if (dataGridView1.CurrentRow.Index != -1)
                {
                    txtCode.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                    txtName.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                    txtAddress.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                    txtTelephone.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                    txtTelephone2.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
                    txtEmail.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
                    cmbMethodPayment.Text = dataGridView1.CurrentRow.Cells[7].Value.ToString();
                    txtNIT.Text = dataGridView1.CurrentRow.Cells[8].Value.ToString();
                    providerModel.ProviderID = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value.ToString());

                }
            }
            catch (Exception)
            {


                MessageBox.Show("Debe de escoger un código válido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnNew_Click(object sender, EventArgs e)
        {
            switch (btnNew.Text)
            {
                case "Nuevo":
                    btnNew.Text = "Guardar";
                    btnDelete.Text = "Cancelar";
                    btnUpdate.Enabled = false;
                    Clear();
                    break;
                case "Guardar":
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
                        try
                        {
                            command.Parameters.AddWithValue("_code", providerModel.Code);
                            command.Parameters.AddWithValue("_name", providerModel.Name);
                            command.Parameters.AddWithValue("_address", providerModel.Address);
                            command.Parameters.AddWithValue("_telephone", providerModel.Telephone);
                            command.Parameters.AddWithValue("_telephone2", providerModel.Telephone2);
                            command.Parameters.AddWithValue("_email", providerModel.Email);
                            command.Parameters.AddWithValue("_methodPayment", providerModel.MethodPayment);
                            command.Parameters.AddWithValue("_nit", providerModel.NIT);
                            command.Parameters.AddWithValue("_automatically", false);
                            Message("Agregar");
                        }
                        catch (Exception)
                        {


                        }
                    }
                    else
                    {
                        try
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
                            Message("Agregar");
                        }
                        catch (Exception)
                        {

                            //throw;
                        }
                    }

                    btnNew.Text = "Nuevo";
                    btnDelete.Text = "Eliminar";
                    btnUpdate.Enabled = true;
                    btnDelete.Enabled = true;
                    break;

            }


        }




        public void Message(string status)
        {

            switch (status)
            {
                case "Agregar":
                    try
                    {
                        command.ExecuteNonQuery();
                        MessageBox.Show("Registro Agregado Exitosamente", "Registro Agregado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        GridFill();
                        Clear();
                    }
                    catch (Exception)
                    {

                        MessageBox.Show("Código o NIT Repetido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        btnDelete.Text = "Eliminar";
                    }
                    break;
            }

        }

        public void GridFill()
        {
            MySqlDataAdapter sqlData = new MySqlDataAdapter("ListProvider", Connection.MakeConnection());
            sqlData.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dtProvider = new DataTable();
            sqlData.Fill(dtProvider);


            dataGridView1.DataSource = dtProvider;
            DataGridViewColumn column = dataGridView1.Columns[0];
            column.Visible = false;

            for (int i = 0; i < 9; i++)
            {

                DataGridViewColumn column1 = dataGridView1.Columns[i];

                column1.Width = 98;
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


        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (btnDelete.Text == "Cancelar")
            {
                btnNew.Text = "Nuevo";
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
                btnDelete.Text = "Eliminar";
            }
            else
            {
                MySqlCommand command = new MySqlCommand("DeleteProviders", Connection.MakeConnection());
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("_ProviderID", providerModel.ProviderID);
                DialogResult dialogResult = MessageBox.Show("Desea Eliminar Este Registro", "Eliminar Registro", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    command.ExecuteNonQuery();
                    MessageBox.Show("Registro Elimnado Exitosamente", "Registro Eliminado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    GridFill();
                    Clear();
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
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
                GridFill();
                Clear();
            }
            catch (Exception)
            {

                MessageBox.Show("Código o NIT Repetido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnDelete.Text = "Eliminar";
            }
        }
    }
}
