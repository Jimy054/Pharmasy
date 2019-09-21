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

namespace Pharmacy.Views.Product
{
    public partial class AddProduct : Form
    {
        public AddProduct()
        {
            InitializeComponent();
            ComboboxFillProvider();
            ComboboxFillCategory();


        }
        ProductModel productModel = new ProductModel();

        OpenFileDialog openFileDialog = new OpenFileDialog();
  
        float price1,price2;


        public void ComboboxFillCategory()
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

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            MySqlCommand mySqlCommand = new MySqlCommand("select * from Categories where name='" + cmbCategory.Text + "'", Connection.MakeConnection());
            MySqlDataReader myReader;
            myReader = mySqlCommand.ExecuteReader();
            while (myReader.Read())
            {
                string id = myReader.GetInt32("CategoryID").ToString();
                productModel.CategoryID = int.Parse(id);



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








        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult im = openFileDialog.ShowDialog();
            if (im == DialogResult.OK)
            {
                pictureBox1.Load(openFileDialog.FileName);
            }
        }



        private void btnNew_Click(object sender, EventArgs e)
        {
            productModel.Code = txtCode.Text;
            productModel.Name = txtName.Text;
            productModel.Description = rtDescription.Text;
            productModel.Quantity = int.Parse(txtQuantity.Text);
            productModel.Units = int.Parse(txtUnit.Text);
            productModel.Price = decimal.Parse(txtPrice.Text);
            productModel.Image = openFileDialog.FileName;
            productModel.PriceSale = decimal.Parse( txtPriceSale.Text);
            productModel.Gain =  productModel.PriceSale-productModel.Price;
            MySqlCommand command = new MySqlCommand("GenerateCodeProducts", Connection.MakeConnection());
            if (checkBox1.Checked)
            {
                productModel.Code = "";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("_code", productModel.Code);
                command.Parameters.AddWithValue("_name", productModel.Name);
                command.Parameters.AddWithValue("_description", productModel.Description);
                command.Parameters.AddWithValue("_box", productModel.Quantity);
                command.Parameters.AddWithValue("_price", productModel.Price);

                command.Parameters.AddWithValue("_SalePrice", productModel.PriceSale);
                command.Parameters.AddWithValue("_gain", productModel.Gain);
                command.Parameters.AddWithValue("_units", productModel.Units);
                
                command.Parameters.AddWithValue("_image", productModel.Image);
                command.Parameters.AddWithValue("_CategoryID", productModel.CategoryID);
                command.Parameters.AddWithValue("_ProviderID", productModel.ProviderID);
                command.Parameters.AddWithValue("_automatically", false);
                try
                {
                    command.ExecuteNonQuery();
                    MessageBox.Show("Registro Agregado Exitosamente", "Registro Agregado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                catch
                {
                    MessageBox.Show("Código Repetido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
             
            }
            else
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("_code", productModel.Code);
                command.Parameters.AddWithValue("_name", productModel.Name);
                command.Parameters.AddWithValue("_description", productModel.Description);
                command.Parameters.AddWithValue("_box", productModel.Quantity);
                command.Parameters.AddWithValue("_price", productModel.Price);

                command.Parameters.AddWithValue("_SalePrice", productModel.PriceSale);
                command.Parameters.AddWithValue("_gain", productModel.Gain);
                command.Parameters.AddWithValue("_units", productModel.Units);
                command.Parameters.AddWithValue("_image", productModel.Image);
                command.Parameters.AddWithValue("_CategoryID", productModel.CategoryID);
                command.Parameters.AddWithValue("_ProviderID", productModel.ProviderID);
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

        private void txtPrice_TextChanged(object sender, EventArgs e)
        {
            if (txtPrice.Text == "")
            {
                price1 = 0.00f;
            }
            else
            {
                price1 = float.Parse(txtPrice.Text);
            }

            txtGain.Text = (price2 - price1).ToString();
        }

        private void txtQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {

            OnlyNumbers(sender, e);
        }

        private void txtUnit_KeyPress(object sender, KeyPressEventArgs e)
        {
            OnlyNumbers(sender, e);
        }



        public void OnlyNumbers(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
        (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void txtPriceSale_KeyPress(object sender, KeyPressEventArgs e)
        {
            OnlyNumbers(sender,e);
        }

        private void AddProduct_Load(object sender, EventArgs e)
        {

        }

        private void txtPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            OnlyNumbers(sender, e);
        }

        private void txtGain_KeyPress(object sender, KeyPressEventArgs e)
        {
            OnlyNumbers(sender, e);
        }

        private void txtPriceSale_TextChanged(object sender, EventArgs e)
        {
            if (txtPriceSale.Text == "")
            {
                price2 = 0.00f;
            }
            else
            {
                price2 = float.Parse(txtPriceSale.Text);
            }


            txtGain.Text = (price2 - price1).ToString();
        }
    }
}
