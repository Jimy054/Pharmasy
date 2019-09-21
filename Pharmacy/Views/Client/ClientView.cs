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
using Pharmacy.Views.Client;

namespace Pharmacy.Views
{
    public partial class ClientView : Form
    {
        ClientModel clientModel = new ClientModel();
        DataTable dtClient;
        enum Search { Name,NIT,Code}
        Search name, nit, code;
     
        public ClientView()
        {
            InitializeComponent();
            GridFill();
            btnDelete.Enabled = false;
            btnUpdate.Enabled = false;
        }


        

        private void btnNew_Click(object sender, EventArgs e)
        {
            AddClient addClient = new AddClient();
            addClient.ShowDialog();
            GridFill();
        }




      

        public void GridFill()
        {
            MySqlDataAdapter sqlData = new MySqlDataAdapter("ListClients", Connection.MakeConnection());
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




        private void btnDelete_Click(object sender, EventArgs e)
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
                }            
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            try
            {

                if (dataGridView1.CurrentRow.Index != -1)
                {
                    clientModel.Code = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                    clientModel.Name = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                    clientModel.Address = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                    clientModel.Telephone = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                    clientModel.Telephone2 = dataGridView1.CurrentRow.Cells[5].Value.ToString();
                    clientModel.Email = dataGridView1.CurrentRow.Cells[6].Value.ToString();
                    clientModel.MethodPayment= dataGridView1.CurrentRow.Cells[7].Value.ToString();
                    clientModel.NIT = dataGridView1.CurrentRow.Cells[8].Value.ToString();
                    clientModel.ClientID = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value.ToString());
                    btnDelete.Enabled = true;
                    btnUpdate.Enabled = true;
                }
            }
            catch (Exception)
            {


                MessageBox.Show("Debe de escoger un código válido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    code = Search.Code;
                    name = Search.Code;
                    nit = Search.Code;
                    break;
                case 1:
                    name = Search.Name;
                    code = Search.Name;
                    nit = Search.Name;
                    break;
                case 2:
                    nit = Search.NIT;
                    name = Search.NIT;
                    code = Search.NIT;
                    break;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateClient updateClient = 
          new UpdateClient(clientModel.ClientID,clientModel.Name, clientModel.Address,
          clientModel.Telephone,clientModel.Telephone2,
          clientModel.Email,clientModel.MethodPayment,
          clientModel.NIT,clientModel.Code);

            updateClient.ShowDialog();
            GridFill();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            DataView dv = new DataView(dtClient);

            if (code == Search.Code && name ==Search.Code && nit == Search.Code)
            {
                dv.RowFilter = string.Format("Codigo like '%{0}%'", textBox1.Text);
                dataGridView1.DataSource = dv;
            }
            else if (code == Search.Name && name == Search.Name && nit == Search.Name)
            {
                dv.RowFilter = string.Format("Nombre like '%{0}%'", textBox1.Text);
                dataGridView1.DataSource = dv;
            }
            else if (code == Search.NIT && name == Search.NIT && nit == Search.NIT)
            {
                dv.RowFilter = string.Format("NIT like '%{0}%'", textBox1.Text);
                dataGridView1.DataSource = dv;
            }
        }



    }
}
