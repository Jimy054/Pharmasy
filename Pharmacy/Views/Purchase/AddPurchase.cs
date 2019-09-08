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

namespace Pharmacy.Views.Purchase
{
    public partial class AddPurchase : Form
    {
        PurchaseModel purchaseModel = new PurchaseModel();
        PurchasesDetailsModel purchasesDetailsModel = new PurchasesDetailsModel();
        float price1;
        int quantity1;

        public AddPurchase(int id)
        {
            InitializeComponent();
            ComboboxFillProvider();
            ComboboxFillProduct();
            txtDiscount.Visible = false;
            label9.Visible = false;
            purchasesDetailsModel.PurchasesID = id;
            purchaseModel.PurchasesID = id;
       
            //       GridFill();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MySqlCommand command = new MySqlCommand("UpdatePurchase", Connection.MakeConnection());
            purchaseModel.PurchasesReference = txtReference.Text;
            purchaseModel.PurchasesDate = DateTime.Parse(dtDate.Text);
            purchaseModel.PurchasesTotal = 0.00f;
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("_PurchasesID", purchaseModel.PurchasesID);
            command.Parameters.AddWithValue("_PurchasesReference", purchaseModel.PurchasesReference);
            command.Parameters.AddWithValue("_PurchasesDate", purchaseModel.PurchasesDate);
            command.Parameters.AddWithValue("_PurchasesTotal", purchaseModel.PurchasesTotal);
            command.Parameters.AddWithValue("_ProviderID", purchaseModel.ProviderID);
            

                command.ExecuteNonQuery();
                MessageBox.Show("Registro Actualizado Exitosamente", "Registro Actualizado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
            //     GridFill();         
          



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


        public void ComboboxFillProduct()
        {
            //   MySqlDataAdapter sqlData = new MySqlDataAdapter();
            MySqlCommand mySqlCommand = new MySqlCommand("select * from Products", Connection.MakeConnection());
            MySqlDataReader myReader;

            myReader = mySqlCommand.ExecuteReader();

            while (myReader.Read())
            {
                string name = myReader.GetString("name");
                
                cmbProduct.Items.Add(name);
                
            }

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
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
              
                purchasesDetailsModel.ProductID = int.Parse(productID);

            }
        }

        private void ckDiscount_CheckedChanged(object sender, EventArgs e)
        {
            if (ckDiscount.Checked)
            {
                txtDiscount.Visible = true;
                label9.Visible = true;
            }
            else
            {
                txtDiscount.Visible = false;
                label9.Visible = false;
                txtDiscount.Text = "";
            }
        }

        private void btnNewDetails_Click(object sender, EventArgs e)
        {
            MySqlCommand command = new MySqlCommand("InsertPurchasesDetails", Connection.MakeConnection());
            purchasesDetailsModel.Quantity = int.Parse(txtQuantity.Text);
            purchasesDetailsModel.Price = float.Parse(txtPrice.Text);           
            if (txtDiscount.Text!="")
            {
                purchasesDetailsModel.Discount = float.Parse(txtDiscount.Text);
            }
            else
            {
                purchasesDetailsModel.Discount = 0.00f;
            }
            purchasesDetailsModel.SubTotal = purchasesDetailsModel.Quantity * purchasesDetailsModel.Price;
            
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("_Price", purchasesDetailsModel.Price);
            command.Parameters.AddWithValue("_Quantity", purchasesDetailsModel.Quantity);
            command.Parameters.AddWithValue("_SubTotal", purchasesDetailsModel.SubTotal);
            command.Parameters.AddWithValue("_Discount", purchasesDetailsModel.Discount);
            command.Parameters.AddWithValue("_PurchasesID", purchasesDetailsModel.PurchasesID);
            command.Parameters.AddWithValue("_ProductID", purchasesDetailsModel.ProductID);
           
                command.ExecuteNonQuery();
                MessageBox.Show("Registro Agregado Exitosamente", "Registro Agregado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                GridFill();
                Clear();
           
        }


        public void GridFill()
        {
            MySqlDataAdapter sqlData = new MySqlDataAdapter("ListPurchasesDetails", Connection.MakeConnection());   
            sqlData.SelectCommand.Parameters.AddWithValue("_PurchasesID", purchasesDetailsModel.PurchasesID);
            sqlData.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dtProduct = new DataTable();
            sqlData.Fill(dtProduct);


            dataGridView1.DataSource = dtProduct;
            DataGridViewColumn column = dataGridView1.Columns[0];
            column.Visible = false;

            for (int i = 0; i < 5; i++)
            {
                DataGridViewColumn column1 = dataGridView1.Columns[i];       
                column1.Width = 188;
            }

        }

        private void cmbProvider_SelectedIndexChanged(object sender, EventArgs e)
        {
            MySqlCommand mySqlCommand = new MySqlCommand("select * from Providers where name='" + cmbProvider.Text + "'", Connection.MakeConnection());
            MySqlDataReader myReader;

            myReader = mySqlCommand.ExecuteReader();

            while (myReader.Read())
            {
                string providerID = myReader.GetInt32("ProviderID").ToString();               
                purchaseModel.ProviderID = int.Parse(providerID);

            }
        }

        public void Clear()
        {
          
            cmbProduct.Text = "";
            txtQuantity.Text = "";
            txtPrice.Text = "";
            txtDiscount.Text = "";
            label11.Text = "";
        }


        private void dataGridView1_Click(object sender, EventArgs e)
        {
            try
            {

                if (dataGridView1.CurrentRow.Index != -1)
                {

                    // MemoryStream memoryStream = new MemoryStream();
                    //  byte[] img = (byte[])dataGridView1.;

                  
                    txtPrice.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                    txtQuantity.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                    cmbProduct.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                  //  cmbProduct.Text = dataGridView1.CurrentRow.Cells[7].Value.ToString();
                    //pictureBox1. = dataGridView1.CurrentRow.Cells[5].Value.ToString();
                    purchasesDetailsModel.PurchasesDetailsID = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value.ToString());

                }
            }
            catch (Exception)
            {


                MessageBox.Show("Debe de escoger un código válido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
         /*   MySqlCommand command = new MySqlCommand("DeletePurchaseDetails", Connection.MakeConnection());
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("_ProductID", productModel.ProductID);
            DialogResult dialogResult = MessageBox.Show("Desea Eliminar Este Registro", "Eliminar Registro", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {

                command.ExecuteNonQuery();
                GridFill();
                Clear();
            }*/
        }

        private void txtPrice_TextChanged(object sender, EventArgs e)
        {
            if (txtPrice.Text=="")
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
            if (txtQuantity.Text=="")
            {
                quantity1 = 0;
            }
            else
            {
                quantity1 = int.Parse(txtQuantity.Text);
            }
            label13.Text =( price1 * quantity1).ToString();

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
