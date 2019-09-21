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
using Pharmacy.Views.Product;

namespace Pharmacy.Views
{
    public partial class ProductView : Form
    {
        ProductModel productModel = new ProductModel();
        OpenFileDialog openFileDialog = new OpenFileDialog();
        enum Search { Name,Code}
        Search name, code;
        DataTable dtProduct;

        public ProductView()
        {
            InitializeComponent();
            GridFill();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {

            AddProduct addProduct = new AddProduct();
            addProduct.ShowDialog();
            GridFill();
            
        }


        public void Message(string status)
        {

            

        }



        public void GridFill()
        {
            MySqlDataAdapter sqlData = new MySqlDataAdapter("ListProducts", Connection.MakeConnection());
            sqlData.SelectCommand.CommandType = CommandType.StoredProcedure;
            dtProduct = new DataTable();
            sqlData.Fill(dtProduct);
            dataGridView1.DataSource = dtProduct;
            DataGridViewColumn column = dataGridView1.Columns[0];
            column.Visible = false;
            for (int i = 0; i <=12; i++)
            {

                DataGridViewColumn column1 = dataGridView1.Columns[i];
                if (column1== dataGridView1.Columns[11] )
                {
                    column1.Visible = false;
                }

                if (column1 == dataGridView1.Columns[12])
                {
                    column1.Visible = false;
                }
                column1.Width = 81;
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

                    productModel.Code = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                    productModel.Name = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                    productModel.Description = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                    productModel.Quantity = int.Parse(dataGridView1.CurrentRow.Cells[4].Value.ToString());
                    productModel.Units = int.Parse(dataGridView1.CurrentRow.Cells[5].Value.ToString());
                    productModel.Price = decimal.Parse(dataGridView1.CurrentRow.Cells[6].Value.ToString());
                    productModel.PriceSale = decimal.Parse(dataGridView1.CurrentRow.Cells[7].Value.ToString());
                    productModel.Gain = decimal.Parse(dataGridView1.CurrentRow.Cells[8].Value.ToString());
                    productModel.CategoryID = int.Parse(dataGridView1.CurrentRow.Cells[11].Value.ToString());
                    productModel.ProviderID = int.Parse( dataGridView1.CurrentRow.Cells[12].Value.ToString());       
                    //pictureBox1. = dataGridView1.CurrentRow.Cells[5].Value.ToString();
                    productModel.ProductID = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value.ToString());

                }
            }
            catch (Exception)
            {


                MessageBox.Show("Debe de escoger un código válido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        

        private void btnUpdate_Click(object sender, EventArgs e)
        {

            UpdateProduct updateProduct = new UpdateProduct(productModel.ProductID,productModel.Code,productModel.Name,productModel.Description,productModel.Quantity,productModel.Units,productModel.Price,productModel.PriceSale,productModel.Gain,productModel.CategoryID,productModel.ProviderID);
            updateProduct.ShowDialog();
            GridFill();
            
            /*
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

            
         */
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
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

            DataView dv = new DataView(dtProduct);

            if (code == Search.Code && name == Search.Code)
            {
                dv.RowFilter = string.Format("Codigo like '%{0}%'", textBox1.Text);
                dataGridView1.DataSource = dv;
            }
            else if (code == Search.Name && name == Search.Name)
            {
                dv.RowFilter = string.Format("Nombre like '%{0}%'", textBox1.Text);
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
                    break;
                case 1:
                    name = Search.Name;
                    code = Search.Name;
                    break;           
            }

        }
    }
}
