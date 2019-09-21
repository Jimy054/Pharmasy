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
    public partial class UpdateProduct : Form
    {

        ProductModel productModel = new ProductModel();

        public UpdateProduct(int id, string code, string name, string description, int quantity, int unit, decimal price, decimal priceSale, decimal gain, int categoryID, int providerID)
        {
            InitializeComponent();
            ComboboxFillProvider();
            ComboboxFillCategory();

            productModel.ProductID = id;
            txtCode.Text = code;
            txtName.Text = name;
            rtDescription.Text = description;
            txtQuantity.Text = quantity.ToString();
            txtUnit.Text = unit.ToString();
            txtPrice.Text = price.ToString();
            txtPriceSale.Text = priceSale.ToString();
            txtGain.Text = gain.ToString();
            MessageBox.Show("Provider " + providerID);
            MessageBox.Show("Category " + categoryID);
            productModel.ProviderID = providerID;
            productModel.CategoryID = categoryID;
            SearchCategory();
            SearchProvider();
        }

        public void ComboboxFillCategory()
        {
            //   MySqlDataAdapter sqlData = new MySqlDataAdapter();
            MySqlCommand mySqlCommand = new MySqlCommand("select name from Categories", Connection.MakeConnection());
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



        private void SearchCategory()
        {
            MySqlCommand mySqlCommand = new MySqlCommand("select name from Categories where CategoryID='" + productModel.CategoryID + "'", Connection.MakeConnection());
            MySqlDataReader myReader;
            myReader = mySqlCommand.ExecuteReader();
            while (myReader.Read())
            {
                string name = myReader.GetString("Name").ToString();
                MessageBox.Show(name);
                cmbCategory.Text = name;
            }

        }


        private void SearchProvider()
        {
            MySqlCommand mySqlCommand = new MySqlCommand("select name from Providers where ProviderID='" + productModel.ProviderID + "'", Connection.MakeConnection());
            MySqlDataReader myReader;
            myReader = mySqlCommand.ExecuteReader();
            while (myReader.Read())
            {
                string name = myReader.GetString("Name");
                cmbProvider.Text = name;
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





        public void ComboboxFillProvider()
        {
            //   MySqlDataAdapter sqlData = new MySqlDataAdapter();
            MySqlCommand mySqlCommand = new MySqlCommand("select name from Providers", Connection.MakeConnection());
            MySqlDataReader myReader;
            myReader = mySqlCommand.ExecuteReader();
            while (myReader.Read())
            {
                string name = myReader.GetString("name");
                cmbProvider.Items.Add(name);
            }

        }






        private void btnNew_Click(object sender, EventArgs e)
        {
            MySqlCommand command = new MySqlCommand("UpdateProduct", Connection.MakeConnection());
            productModel.Code = txtCode.Text;
            productModel.Name = txtName.Text;
            productModel.Description = rtDescription.Text;
            productModel.Quantity = int.Parse(txtQuantity.Text);
            productModel.Units = int.Parse(txtUnit.Text);
            productModel.Price = decimal.Parse(txtPrice.Text);
            productModel.PriceSale = decimal.Parse(txtPriceSale.Text);
            productModel.Gain = productModel.PriceSale - productModel.Price;
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("_ProductID", productModel.ProductID);
            command.Parameters.AddWithValue("_code", productModel.Code);
            command.Parameters.AddWithValue("_name", productModel.Name);
            command.Parameters.AddWithValue("_description", productModel.Description);
            command.Parameters.AddWithValue("_box", productModel.Quantity);
            command.Parameters.AddWithValue("_price", productModel.Price);
            command.Parameters.AddWithValue("_SalePrice", productModel.PriceSale);
            command.Parameters.AddWithValue("_gain", productModel.Gain);
            command.Parameters.AddWithValue("_units", productModel.Units);
            command.Parameters.AddWithValue("_CategoryID", productModel.CategoryID);
            command.Parameters.AddWithValue("_ProviderID", productModel.ProviderID);
            try
            {
                command.ExecuteNonQuery();
                MessageBox.Show("Registro Actualizado Exitosamente", "Registro Actualizado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception)
            {

                MessageBox.Show("Código Repetido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        
           
        }
    }
}
