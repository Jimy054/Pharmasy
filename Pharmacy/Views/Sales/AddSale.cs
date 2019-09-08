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

namespace Pharmacy.Views.Sales
{
    public partial class AddSale : Form
    {
        SaleModel salesModel = new SaleModel();
        SalesDetailsModel salesDetailsModel = new SalesDetailsModel();
        int quantity1;
        float price1;

        public AddSale(int id)
        {
            InitializeComponent();
            ComboboxFillClient();
            ComboboxFillProduct();
            salesModel.SalesID = id;
            salesDetailsModel.SalesID = id;
        }


        public void Clear()
        {
            cmbProduct.Text = "";
            txtQuantity.Text = "";
            txtPrice.Text = "";
            txtDiscount.Text = "";
        }

        private void ComboboxFillClient()
        {
            MySqlCommand mySqlCommand = new MySqlCommand("select * from Clients", Connection.MakeConnection());
            MySqlDataReader myReader;

            myReader = mySqlCommand.ExecuteReader();

            while (myReader.Read())
            {
                string name = myReader.GetString("name");
                cmbClient.Items.Add(name);
            }

        }

        private void ComboboxFillProduct()
        {
            MySqlCommand mySqlCommand = new MySqlCommand("select * from Products", Connection.MakeConnection());
            MySqlDataReader myReader;

            myReader = mySqlCommand.ExecuteReader();

            while (myReader.Read())
            {
                string name = myReader.GetString("name");
                string productID = myReader.GetString("ProductID");
                cmbProduct.Items.Add(name);

            }
        }






        private void btnNew_Click(object sender, EventArgs e)
        {
            MySqlCommand command = new MySqlCommand("UpdateSales", Connection.MakeConnection());
            salesModel.SalesReference = txtReference.Text;
            salesModel.SalesDate = DateTime.Parse(dtDate.Text);

            if (checkBox1.Checked)
            {
                salesModel.SalesReference = "";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("_SalesID", salesModel.SalesID);
                command.Parameters.AddWithValue("_SalesReference", salesModel.SalesReference);
                command.Parameters.AddWithValue("_SalesDate", salesModel.SalesDate);
                command.Parameters.AddWithValue("_ClientID", salesModel.ClientID);
                command.Parameters.AddWithValue("_automatically", false);
                command.ExecuteNonQuery();
                MessageBox.Show("Registro Actualizado Exitosamente", "Registro Actualizado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("_SalesID", salesModel.SalesID);
                command.Parameters.AddWithValue("_SalesReference", salesModel.SalesReference);
                command.Parameters.AddWithValue("_SalesDate", salesModel.SalesDate);
                command.Parameters.AddWithValue("_ClientID", salesModel.ClientID);
                command.Parameters.AddWithValue("_automatically", true);
                command.ExecuteNonQuery();
                MessageBox.Show("Registro Actualizado Exitosamente", "Registro Actualizado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
         
        }




        private void cmbProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            MySqlCommand mySqlCommand = new MySqlCommand("select * from Products where name='" + cmbProduct.Text + "'", Connection.MakeConnection());
            MySqlDataReader myReader;

            myReader = mySqlCommand.ExecuteReader();

            while (myReader.Read())
            {

                string productID = myReader.GetInt32("ProductID").ToString();
                string quantity = myReader.GetInt32("Quantity").ToString();
                string price = myReader.GetInt32("Price").ToString();
                txtPrice.Text = price;
                txtQuantity.Text = quantity;
                var operation = float.Parse(price) * float.Parse(quantity);
                label13.Text = operation.ToString();
                salesDetailsModel.ProductID = int.Parse(productID);

            }
        }

        private void cmbClient_SelectedIndexChanged(object sender, EventArgs e)
        {

            MySqlCommand mySqlCommand = new MySqlCommand("select * from Clients where name='" + cmbClient.Text + "'", Connection.MakeConnection());
            MySqlDataReader myReader;

            myReader = mySqlCommand.ExecuteReader();

            while (myReader.Read())
            {
                string clientID = myReader.GetInt32("ClientID").ToString();
                salesModel.ClientID = int.Parse(clientID);

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

            label13.Text = (price1 * quantity1).ToString();
        }

        private void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            if (txtQuantity.Text == "")
            {
                quantity1 = 0;
            }
            else
            {
                quantity1 = int.Parse(txtQuantity.Text);
            }
            label13.Text = (price1 * quantity1).ToString();

        }

        private void btnNewDetails_Click(object sender, EventArgs e)
        {
            MySqlCommand command = new MySqlCommand("InsertSalesDetails", Connection.MakeConnection());
            salesDetailsModel.Quantity = int.Parse(txtQuantity.Text);
            salesDetailsModel.Price = float.Parse(txtPrice.Text);
            if (txtDiscount.Text != "")
            {
                salesDetailsModel.Discount = float.Parse(txtDiscount.Text);
            }
            else
            {
                salesDetailsModel.Discount = 0.00f;
            }
            salesDetailsModel.SubTotal = salesDetailsModel.Quantity * salesDetailsModel.Price;
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("_Price", salesDetailsModel.Price);
            command.Parameters.AddWithValue("_Quantity", salesDetailsModel.Quantity);
            command.Parameters.AddWithValue("_SubTotal", salesDetailsModel.SubTotal);
            command.Parameters.AddWithValue("_Discount", salesDetailsModel.Discount);
            command.Parameters.AddWithValue("_SalesID", salesDetailsModel.SalesID);
            command.Parameters.AddWithValue("_ProductID", salesDetailsModel.ProductID);
            command.ExecuteNonQuery();
            MessageBox.Show("Registro Agregado Exitosamente", "Registro Agregado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            GridFill();
            lbTotal.Text = salesModel.SalesTotal.ToString();
            Clear();
        }



        public void GridFill()
        {
            MySqlDataAdapter sqlData = new MySqlDataAdapter("ListSalesDetails", Connection.MakeConnection());
            sqlData.SelectCommand.Parameters.AddWithValue("_SalesID", salesDetailsModel.SalesID);
            sqlData.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dtProduct = new DataTable();
            sqlData.Fill(dtProduct);


            dataGridView1.DataSource = dtProduct;
            DataGridViewColumn column = dataGridView1.Columns[0];
            column.Visible = false;

            for (int i = 0; i < 7; i++)
            {
                DataGridViewColumn column1 = dataGridView1.Columns[i];
                column1.Width = 143;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {

            MySqlCommand command = new MySqlCommand("DeleteSales", Connection.MakeConnection());
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("_SalesID", salesModel.SalesID);
            command.Parameters.AddWithValue("_deleteSalesDetails", false);
            DialogResult dialogResult = MessageBox.Show("Desea Cancelar La Factura", "Eliminar Registro", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                command.ExecuteNonQuery();
               // MessageBox.Show("Registro Elimnado Exitosamente", "Registro Eliminado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }

        private void txtQuantity_TextChanged_1(object sender, EventArgs e)
        {
            if (txtQuantity.Text == "")
            {
                quantity1 = 0;
            }
            else
            {
                quantity1 = int.Parse(txtQuantity.Text);
            }

            label13.Text = (price1 * quantity1).ToString();
        }

        private void txtPrice_TextChanged_1(object sender, EventArgs e)
        {
            if (txtPrice.Text == "")
            {
                price1 = 0.00f;
            }
            else
            {
                price1 = float.Parse(txtPrice.Text);
            }

            label13.Text = (price1 * quantity1).ToString();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                txtReference.ReadOnly = true;
            }
            else
            {
                txtReference.ReadOnly = false;
            }
        }
    }
}

