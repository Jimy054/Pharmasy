using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Pharmacy.DB;
using Pharmacy.Model;

namespace Pharmacy.Views
{
    public partial class ProductView : Form
    {
        ProductModel productModel = new ProductModel();

        OpenFileDialog openFileDialog = new OpenFileDialog();
       

        public ProductView()
        {
            InitializeComponent();
            ComboboxFill();
            GridFill();
            ComboboxFillProvider();
        }

        public void Clear()
        {
            txtCode.Text = "";
            txtName.Text = "";
            txtQuantity.Text = "";
            txtPrice.Text = "";
            cmbCategory.Text = "";
            cmbProvider.Text = "";
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
                    productModel.Code=  txtCode.Text;
                    productModel.Name=  txtName.Text;
                    productModel.Quantity= int.Parse(txtQuantity.Text);
                    productModel.Price= decimal.Parse(txtPrice.Text);
                    productModel.Image = openFileDialog.FileName;
                    cmbProvider.Text = "";
                    MySqlCommand command = new MySqlCommand("GenerateCodeProducts", Connection.MakeConnection());
                    if (checkBox1.Checked)
                    {
                        productModel.Code = "";
                        command.CommandType = CommandType.StoredProcedure;                        
                        command.Parameters.AddWithValue("_code", productModel.Code);
                        command.Parameters.AddWithValue("_name", productModel.Name);
                        command.Parameters.AddWithValue("_quantity", productModel.Quantity);
                        command.Parameters.AddWithValue("_price", productModel.Price);
                        command.Parameters.AddWithValue("_image", productModel.Image);
                        command.Parameters.AddWithValue("_CategoryID", productModel.CategoryID);
                        command.Parameters.AddWithValue("_ProviderID", productModel.ProviderID);                
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
                            command.Parameters.AddWithValue("_code", productModel.Code);
                            command.Parameters.AddWithValue("_name", productModel.Name);
                            command.Parameters.AddWithValue("_quantity", productModel.Quantity);
                            command.Parameters.AddWithValue("_price", productModel.Price);
                            command.Parameters.AddWithValue("_image", productModel.Image);
                            command.Parameters.AddWithValue("_CategoryID", productModel.CategoryID);
                            command.Parameters.AddWithValue("_ProviderID", productModel.ProviderID);
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

            

        }



        public void GridFill()
        {
            MySqlDataAdapter sqlData = new MySqlDataAdapter("ListProduct", Connection.MakeConnection());
            sqlData.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dtProduct = new DataTable();
            sqlData.Fill(dtProduct);


            dataGridView1.DataSource = dtProduct;
            DataGridViewColumn column = dataGridView1.Columns[0];
            column.Visible = false;

            for (int i = 0; i < 8; i++)
            {

                DataGridViewColumn column1 = dataGridView1.Columns[i];
                if (column1== dataGridView1.Columns[5])
                {
                    column1.Visible = false;
                }

                column1.Width = 118;
            }

        }




        public void ComboboxFill()
        {
            //   MySqlDataAdapter sqlData = new MySqlDataAdapter();
            MySqlCommand mySqlCommand = new MySqlCommand("select * from Categories", Connection.MakeConnection());
            MySqlDataReader myReader;
            try
            {
                myReader = mySqlCommand.ExecuteReader();

                while (myReader.Read())
                {
                    string name = myReader.GetString("name");
                    cmbCategory.Items.Add(name);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Código o NIT Repetido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }               
            
        }

        public void ComboboxFillProvider()
        {
            //   MySqlDataAdapter sqlData = new MySqlDataAdapter();
            MySqlCommand mySqlCommand = new MySqlCommand("select * from Providers", Connection.MakeConnection());
            MySqlDataReader myReader;
          
                myReader = mySqlCommand.ExecuteReader();

                while (myReader.Read())
                {
                    string name = myReader.GetString("name");  
                    cmbProvider.Items.Add(name);
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

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult im = openFileDialog.ShowDialog();
            if (im==DialogResult.OK)
            {
                pictureBox1.Load(openFileDialog.FileName);
            }
        }


        private void dataGridView1_Click(object sender, EventArgs e)
        {
            try
            {

                if (dataGridView1.CurrentRow.Index != -1)
                {

                   // MemoryStream memoryStream = new MemoryStream();
                  //  byte[] img = (byte[])dataGridView1.;

                    txtCode.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                    txtName.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                    txtQuantity.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                    txtPrice.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                    cmbCategory.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
                    cmbProvider.Text = dataGridView1.CurrentRow.Cells[7].Value.ToString();       
                    //pictureBox1. = dataGridView1.CurrentRow.Cells[5].Value.ToString();
                    productModel.ProductID = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value.ToString());

                }
            }
            catch (Exception)
            {


                MessageBox.Show("Debe de escoger un código válido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            MySqlCommand mySqlCommand = new MySqlCommand("select * from Categories where name='"+cmbCategory.Text+"'", Connection.MakeConnection());
            MySqlDataReader myReader;
            myReader = mySqlCommand.ExecuteReader();
            while (myReader.Read())
            {                
                string id = myReader.GetInt32("CategoryID").ToString();
                productModel.CategoryID = int.Parse(id);
                


            }

        }

        private void cmbProvider_SelectedIndexChanged(object sender, EventArgs e)
        {
            MySqlCommand mySqlCommand = new MySqlCommand("select * from Providers where name='" + cmbProvider.Text + "'", Connection.MakeConnection());
            MySqlDataReader myReader;

            myReader = mySqlCommand.ExecuteReader();

            while (myReader.Read())
            {

                string id = myReader.GetInt32("ProviderID").ToString();
               
                productModel.ProviderID = int.Parse(id);

            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            MySqlCommand command = new MySqlCommand("UpdateProduct", Connection.MakeConnection());
            productModel.Code = txtCode.Text;
            productModel.Name = txtName.Text;
            productModel.Quantity = int.Parse(txtQuantity.Text);
            productModel.Price = decimal.Parse(txtPrice.Text); 
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("_ProductID", productModel.ProductID);
            command.Parameters.AddWithValue("_code", productModel.Code);
            command.Parameters.AddWithValue("_name", productModel.Name);
            command.Parameters.AddWithValue("_quantity", productModel.Quantity);
            command.Parameters.AddWithValue("_price", productModel.Price);
            command.Parameters.AddWithValue("_image", productModel.Image);
            command.Parameters.AddWithValue("_CategoryID", productModel.CategoryID);
            command.Parameters.AddWithValue("_ProviderID", productModel.ProviderID);
            try
            {

                command.ExecuteNonQuery();
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
                MySqlCommand command = new MySqlCommand("DeleteProduct", Connection.MakeConnection());
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("_ProductID", productModel.ProductID);
                DialogResult dialogResult = MessageBox.Show("Desea Eliminar Este Registro", "Eliminar Registro", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {

                    command.ExecuteNonQuery();
                    GridFill();
                    Clear();
                }
            }
        }
    }
}
