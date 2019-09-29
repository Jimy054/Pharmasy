using MySql.Data.MySqlClient;
using Pharmacy.DB;
using Pharmacy.Model;
using Pharmacy.Views.Product;
using Pharmacy.Views.Provider;
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
        int control;
        string quantity;
        MySqlCommand mySqlCommand;
        float discount1;

        public AddPurchase(int id)
        {
            InitializeComponent();
            ComboboxFillProvider();
            ComboboxFillProduct();
            txtDiscount.Visible = false;
            label9.Visible = false;
             btnNew.Enabled = false;
            btnNewDetails.Enabled = false;
            purchasesDetailsModel.PurchasesID = id;
            purchaseModel.PurchasesID = id;
            TextFillNIT();         
            TextFillCode();
            TextFillCodeProduct();
        }



        //Methods
        public void ValidatePurchasesDetails()
        {
            if (cmbCodeProduct.Text == "" && txtQuantity.Text == "" && txtPrice.Text == "" && txtDiscount.Text == "" && cmbCodeProduct.Text == "")
            {
                btnNewDetails.Enabled = false;
            }
            else
            {
                btnNewDetails.Enabled = true;
            }
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


        public void Clear()
        {

            cmbProduct.Text = "";
            txtQuantity.Text = "";
            txtPrice.Text = "";
            txtDiscount.Text = "";
            lbTotal.Text = "";
            cmbCodeProduct.Text = "";
            richTextBox1.Text = "";
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



        public void ValidatePurchases()
        {
            if (cmbNIT.Text=="" || cmbCode.Text==""|| cmbProvider.Text=="")
            {
                btnNew.Enabled = false;
            }
            else
            {
                btnNew.Enabled = true;
            }
        }



        //Providers
        public void ComboboxFillProvider()
        {
            try
            {
                mySqlCommand = new MySqlCommand("select name from Providers where Status='Ingresado'", Connection.MakeConnection());
                MySqlDataReader myReader;
                myReader = mySqlCommand.ExecuteReader();
                AutoCompleteStringCollection MyCollection = new AutoCompleteStringCollection();
                while (myReader.Read())
                {
                    string name = myReader.GetString("name");
                    cmbProvider.Items.Add(name);
                }

                cmbProvider.AutoCompleteCustomSource = MyCollection;
            }
            finally
            {
                Connection.MakeConnection().Close();
            }
        }

        
        private void TextFillNIT()
        {
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand("select nit from Providers where Status='Ingresado'", Connection.MakeConnection());
                MySqlDataReader myReader;
                myReader = mySqlCommand.ExecuteReader();
                AutoCompleteStringCollection MyCollection = new AutoCompleteStringCollection();

                while (myReader.Read())
                {

                    string nit = myReader.GetString("nit");
                    cmbNIT.Items.Add(nit);
                }
                   cmbNIT.AutoCompleteCustomSource = MyCollection;

            }
            finally
            {
                Connection.MakeConnection().Close();
            }

        }


        private void TextFillCode()
        {
            try
            {
                mySqlCommand = new MySqlCommand("select code from Providers where Status='Ingresado' ", Connection.MakeConnection());
                MySqlDataReader myReader;
                myReader = mySqlCommand.ExecuteReader();
                AutoCompleteStringCollection MyCollection = new AutoCompleteStringCollection();

                while (myReader.Read())
                {
                    string name = myReader.GetString("code");
                    cmbCode.Items.Add(name);
                }

                cmbCode.AutoCompleteCustomSource = MyCollection;
            }
            finally
            {
                Connection.MakeConnection().Close();
            }
        }



        private void cmbProvider_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                mySqlCommand = new MySqlCommand("select ProviderID,NIT,Code from Providers where name='" + cmbProvider.Text + "'", Connection.MakeConnection());
                MySqlDataReader myReader;
                myReader = mySqlCommand.ExecuteReader();
                while (myReader.Read())
                {

                    string providerID = myReader.GetInt32("ProviderID").ToString();
                    string nit = myReader.GetString("NIT");
                    string code = myReader.GetString("Code");
                 //   string name = myReader.GetString("Name");
                   // cmbProvider.Text = name;
                    purchaseModel.ProviderID = int.Parse(providerID);
                    cmbNIT.Text = nit;
                    cmbCode.Text = code;
                }
            }
            finally
            {
                Connection.MakeConnection().Close();
            }
        }

        private void cmbNIT_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbNIT.SelectedIndex==0) {
                    AddProvider addProvider = new AddProvider();
                    addProvider.ShowDialog();
                    TextFillNIT();
                }
                else{   
                     mySqlCommand = new MySqlCommand("select ProviderID,NIT,Code,Name from Providers where  nit='" + cmbNIT.Text + "'", Connection.MakeConnection());
                    MySqlDataReader myReader;
                    myReader = mySqlCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        string providerID = myReader.GetInt32("ProviderID").ToString();
                        string nit = myReader.GetString("NIT");
                        string code = myReader.GetString("Code");
                        string name = myReader.GetString("Name");
                        cmbProvider.Text = name;
                        purchaseModel.ProviderID = int.Parse(providerID);
                        cmbNIT.Text = nit;
                        cmbCode.Text = code;
                    }
                }
            }
            finally
            {
                Connection.MakeConnection().Close();
            }

        }


        private void cmbCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                mySqlCommand = new MySqlCommand("select ProviderID,NIT,Code,Name from Providers where  code='" + cmbCode.Text + "'", Connection.MakeConnection());
                MySqlDataReader myReader;
                myReader = mySqlCommand.ExecuteReader();
                while (myReader.Read())
                {
                    string providerID = myReader.GetInt32("ProviderID").ToString();
                    string nit = myReader.GetString("NIT");
                    string code = myReader.GetString("Code");
                    string name = myReader.GetString("Name");
                    cmbProvider.Text = name;
                    purchaseModel.ProviderID = int.Parse(providerID);
                    cmbNIT.Text = nit;
                    cmbCode.Text = code;
                }
            }
            finally
            {
                Connection.MakeConnection().Close();
            }
        }

        






        //Products
        public void ComboboxFillProduct()
        {
            try
            {
                mySqlCommand = new MySqlCommand("select name from Products where Status='Ingresado'", Connection.MakeConnection());
                MySqlDataReader myReader;
                myReader = mySqlCommand.ExecuteReader();
                AutoCompleteStringCollection MyCollection = new AutoCompleteStringCollection();
                while (myReader.Read())
                {
                    string name = myReader.GetString("name");
                    cmbProduct.Items.Add(name);
                }
                cmbCodeProduct.AutoCompleteCustomSource = MyCollection;
            }
            finally
            {
                Connection.MakeConnection().Close();
            }

        }

        private void cmbProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                mySqlCommand = new MySqlCommand("select ProductID,code from Products where  name='" + cmbProduct.Text + "'", Connection.MakeConnection());
                MySqlDataReader myReader;
                myReader = mySqlCommand.ExecuteReader();
                while (myReader.Read())
                {
                    string productID = myReader.GetInt32("ProductID").ToString();
                    string name = myReader.GetString("code");
                 //   string price = myReader.GetInt32("Price").ToString();
                   // txtPrice.Text = price;
                    txtQuantity.Text = quantity;
                    var operation = float.Parse(txtPrice.Text) * float.Parse(quantity);
                    cmbCodeProduct.Text = name;
                    label13.Text = operation.ToString();
                    purchasesDetailsModel.ProductID = int.Parse(productID);                   
                }
            }
            finally
            {
                Connection.MakeConnection().Close();
            }
        }


        private void TextFillCodeProduct()
        {
            try
            {
                //Connection.MakeConnection().Open();
                mySqlCommand = new MySqlCommand("select code from Products where Status='Ingresado'", Connection.MakeConnection());
                MySqlDataReader myReader;
                myReader = mySqlCommand.ExecuteReader();
                AutoCompleteStringCollection MyCollection = new AutoCompleteStringCollection();

                while (myReader.Read())
                {
                    string name = myReader.GetString("code");
                    cmbCodeProduct.Items.Add(name);
                }

                cmbCodeProduct.AutoCompleteCustomSource = MyCollection;

            }
            finally
            {
                Connection.MakeConnection().Close();
            }

        }



        private void cmbCodeProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                if (cmbCodeProduct.SelectedIndex==0)
                {
                    AddProduct addProduct = new AddProduct();
                    addProduct.ShowDialog();
                    TextFillCodeProduct();
                }
                else { 
                    mySqlCommand = new MySqlCommand("select ProductID,Name from Products where code='" + cmbCodeProduct.Text + "'", Connection.MakeConnection());
                    MySqlDataReader myReader;
                    myReader = mySqlCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        string productID = myReader.GetInt32("ProductID").ToString();
                     //   string price = myReader.GetString("Price");
                        string name = myReader.GetString("Name");
                        purchasesDetailsModel.ProductID = int.Parse(productID);
                       // txtPrice.Text = price;
                        txtQuantity.Text = quantity;
                        cmbProduct.Text = name;
                    }
                }
            }
            finally
            {
                Connection.MakeConnection().Close();
            }
        }

        




        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                MySqlCommand command = new MySqlCommand("UpdatePurchases", Connection.MakeConnection());
                purchaseModel.PurchasesReference = txtReference.Text;
                purchaseModel.PurchasesDate = DateTime.Parse(dtDate.Text);
                purchaseModel.Serie = txtSerie.Text;
                if (checkBox1.Checked)
                {

                    purchaseModel.PurchasesReference = "";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("_Series", purchaseModel.Serie);
                    command.Parameters.AddWithValue("_PurchaseID", purchaseModel.PurchasesID);
                    command.Parameters.AddWithValue("_PurchasesReference", purchaseModel.PurchasesReference);
                    command.Parameters.AddWithValue("_PurchasesDate", purchaseModel.PurchasesDate);
                    command.Parameters.AddWithValue("_ProviderID", purchaseModel.ProviderID);
                    command.Parameters.AddWithValue("_automatically", false);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Factura Agregada Exitosamente", "Registro Actualizado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("_Series", purchaseModel.Serie);
                    command.Parameters.AddWithValue("_PurchaseID", purchaseModel.PurchasesID);
                    command.Parameters.AddWithValue("_PurchasesReference", purchaseModel.PurchasesReference);
                    command.Parameters.AddWithValue("_PurchasesDate", purchaseModel.PurchasesDate);
                    command.Parameters.AddWithValue("_ProviderID", purchaseModel.ProviderID);
                    command.Parameters.AddWithValue("_automatically", true);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Factura Agregada Exitosamente", "Registro Actualizado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
            finally
            {
                Connection.MakeConnection().Close();
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
            try
            {

                MySqlCommand command = new MySqlCommand("InsertPurchasesDetails", Connection.MakeConnection());
                if (txtPrice.Text=="")
                {
                    purchasesDetailsModel.Price = 0;
                }
                else
                {
                    purchasesDetailsModel.Price = float.Parse(txtPrice.Text);
                }

                if (txtQuantity.Text == "")
                {
                    purchasesDetailsModel.Quantity = 0;
                }
                else
                {
                    purchasesDetailsModel.Quantity = int.Parse(txtQuantity.Text);
                }


                
                purchasesDetailsModel.Observation = richTextBox1.Text;

                if (txtDiscount.Text != "")
                {
                    purchasesDetailsModel.Discount = float.Parse(txtDiscount.Text);
                }
                else
                {
                    purchasesDetailsModel.Discount = 0.00f;
                }
                purchasesDetailsModel.SubTotal = purchasesDetailsModel.Quantity * purchasesDetailsModel.Price - purchasesDetailsModel.Discount;

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("_Price", purchasesDetailsModel.Price);
                command.Parameters.AddWithValue("_Quantity", purchasesDetailsModel.Quantity);
                command.Parameters.AddWithValue("_SubTotal", purchasesDetailsModel.SubTotal);
                command.Parameters.AddWithValue("_Discount", purchasesDetailsModel.Discount);
                command.Parameters.AddWithValue("_PurchasesID", purchasesDetailsModel.PurchasesID);
                command.Parameters.AddWithValue("_ProductID", purchasesDetailsModel.ProductID);
                command.Parameters.AddWithValue("_Observation", purchasesDetailsModel.Observation);
                command.ExecuteNonQuery();
                MessageBox.Show("Registro Agregado Exitosamente", "Registro Agregado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnNew.Enabled = true;
                GridFill();
                lbTotal.Text = purchaseModel.PurchasesTotal.ToString();
                Clear();
            }

            finally
            {
                Connection.MakeConnection().Close();
            }

        }


      


        private void dataGridView1_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentRow.Index != -1)
                {
                    cmbCodeProduct.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                    cmbProduct.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                    txtPrice.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                    txtQuantity.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
                    txtDiscount.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
                    label13.Text = dataGridView1.CurrentRow.Cells[7].Value.ToString();
                    purchasesDetailsModel.PurchasesDetailsID = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value.ToString());
                }
            }
            catch (Exception)
            {

              
            }
        }

        
        private void txtPrice_TextChanged(object sender, EventArgs e)
        {
            ValidatePurchasesDetails();

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
            ValidatePurchasesDetails();
            if (txtQuantity.Text == "")
            {
                quantity1 = 0;
            }
            else
            {
                quantity1 = int.Parse(txtQuantity.Text);
            }

            label13.Text = (price1 * quantity1).ToString();
            //    MessageBox.Show("Qunatity"+quantity1);
            //  MessageBox.Show("Details Modal"+salesDetailsModel.Quantity);
            //control = int.Parse(quantity);
            if (quantity1.ToString() == "0")
            {
             //   MessageBox.Show("No hay suficientes productos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnNewDetails.Enabled = false;
            }
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

        




        private void button2_Click(object sender, EventArgs e)
        {
            purchaseModel.Serie = txtSerie.Text;
            purchaseModel.PurchasesReference = txtReference.Text;


            try
            {
                MySqlCommand command = new MySqlCommand("DeletePurchases", Connection.MakeConnection());
                command.CommandType = CommandType.StoredProcedure;
                if (txtReference.Text == "")
                {

                    purchaseModel.PurchasesReference = "Nula";

                }
                else
                {
                    purchaseModel.PurchasesReference = txtReference.Text;
                }

                if (txtSerie.Text == "")
                {
                    purchaseModel.Serie = "Nula";
                }
                else
                {
                    purchaseModel.Serie = "Nula";
                }
                if (cmbNIT.Text == "")
                {
                    purchaseModel.ProviderID = 1;
                }

                command.Parameters.AddWithValue("_PurchasesID", purchaseModel.PurchasesID);
                command.Parameters.AddWithValue("_PurchasesReference", purchaseModel.PurchasesReference);
                command.Parameters.AddWithValue("_Series", purchaseModel.Serie);
                command.Parameters.AddWithValue("_ProviderID", purchaseModel.ProviderID);


                DialogResult dialogResult = MessageBox.Show("Desea Cancelar La Factura", "Eliminar Registro", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    command.ExecuteNonQuery();
                    this.Close();
                }
            }
            finally
            {
                Connection.MakeConnection().Close();
            }

        }










     

        private void txtPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            OnlyNumbers(sender, e);
        }

        private void txtQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            OnlyNumbers(sender, e);
        }




        private void txtDiscount_TextChanged(object sender, EventArgs e)
        {
            if (txtDiscount.Text == "")
            {
                discount1 = 0;
            }
            else
            {
                discount1 = float.Parse(txtDiscount.Text);
            }

            label13.Text = (price1 * quantity1 - discount1).ToString();
        }

        private void txtDiscount_KeyPress(object sender, KeyPressEventArgs e)
        {
            OnlyNumbers(sender, e);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            MySqlCommand command = new MySqlCommand("DeletePurchasesDetails", Connection.MakeConnection());
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("_PurchasesDetailsID", purchasesDetailsModel.PurchasesDetailsID);
            MessageBox.Show("ID "+purchasesDetailsModel.PurchasesDetailsID);
            DialogResult dialogResult = MessageBox.Show("Desea borrar este articulo", "Eliminar Registro", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                command.ExecuteNonQuery();
                Clear();
                GridFill();
               
            }
        }
    }
}
