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
using Pharmacy.Views.Provider;

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
        enum Search { Name, NIT, Code }
        Search name, nit, code;
        DataTable dtProvider;

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            try
            {

                if (dataGridView1.CurrentRow.Index != -1)
                {
                    providerModel.Code = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                    providerModel.Name = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                    providerModel.Address = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                    providerModel.Telephone = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                    providerModel.Telephone2 = dataGridView1.CurrentRow.Cells[5].Value.ToString();
                    providerModel.Email = dataGridView1.CurrentRow.Cells[6].Value.ToString();
                    providerModel.MethodPayment = dataGridView1.CurrentRow.Cells[7].Value.ToString();
                    providerModel.NIT = dataGridView1.CurrentRow.Cells[8].Value.ToString();
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
            AddProvider addProvider = new AddProvider();
            addProvider.ShowDialog();
            GridFill();
        }




        public void GridFill()
        {
            MySqlDataAdapter sqlData = new MySqlDataAdapter("ListProviders", Connection.MakeConnection());
            sqlData.SelectCommand.CommandType = CommandType.StoredProcedure;
             dtProvider = new DataTable();
            sqlData.Fill(dtProvider);


            dataGridView1.DataSource = dtProvider;
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
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateProvider updateProvider =
         new UpdateProvider(providerModel.ProviderID, providerModel.Name, providerModel.Address,
         providerModel.Telephone, providerModel.Telephone2,
         providerModel.Email, providerModel.MethodPayment,
         providerModel.NIT, providerModel.Code);
            updateProvider.ShowDialog();
            GridFill();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           // var fom = new CanceledProvider
            
            
            var form = Application.OpenForms.OfType<CanceledProvider>().FirstOrDefault();
            if (form == null)
            {
                form = new CanceledProvider();
                form.StartPosition = FormStartPosition.CenterParent;
                form.ShowDialog(this);
                // this.Hide();
            }
            form.Activate();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            DataView dv = new DataView(dtProvider);

            if (code == Search.Code && name == Search.Code && nit == Search.Code)
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
    }
}
