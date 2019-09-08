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
    public partial class ClientView : Form
    {
        ClientModel clientModel = new ClientModel();
     
        public ClientView()
        {
            InitializeComponent();
            GridFill();
        }

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
                            GridFill();
                            Clear();
                        }
                        catch (Exception)
                        {

                            MessageBox.Show("Código o NIT Repetido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            btnDelete.Text = "Eliminar";
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
                            GridFill();
                            Clear();
                        }
                        catch (Exception)
                        {

                            MessageBox.Show("Código o NIT Repetido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            btnDelete.Text = "Eliminar";
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

           /* switch (status)
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
            }   */      

        }

        public void GridFill()
        {
            MySqlDataAdapter sqlData = new MySqlDataAdapter("ListClient", Connection.MakeConnection());
            sqlData.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dtCategory = new DataTable();
            sqlData.Fill(dtCategory);


            dataGridView1.DataSource = dtCategory;
            DataGridViewColumn column = dataGridView1.Columns[0];
            column.Visible = false;

            for (int i = 0; i < 9; i++)
            {

                DataGridViewColumn column1 = dataGridView1.Columns[i];

                column1.Width = 98;
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
                MySqlCommand command = new MySqlCommand("DeleteClient", Connection.MakeConnection());
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("_ClientID", clientModel.ClientID);
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
                    clientModel.ClientID = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value.ToString());

                }
            }
            catch (Exception)
            {


                MessageBox.Show("Debe de escoger un código válido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
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
