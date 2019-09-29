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
using System.Runtime.InteropServices;
using Pharmacy.Views.Product;
using Pharmacy.Views.Client;

namespace Pharmacy.Views.Sales
{
    public partial class AddSale : Form
    {
        SaleModel salesModel = new SaleModel();
        SalesDetailsModel salesDetailsModel = new SalesDetailsModel();
        int quantity1;
        float price1,discount1;
        int control;
        string quantity;
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hand,int wasg, int wparam,int lparam);
        MySqlCommand mySqlCommand;


        public AddSale(int id)
        {
            InitializeComponent();
           ComboboxFillClient();
           ComboboxFillProduct();
            salesModel.SalesID = id;
            salesDetailsModel.SalesID = id;
            TextFillCode();
            TextFillNIT();
           TextFillCodeProduct();
            btnNew.Enabled = false;
            btnNewDetails.Enabled = false;
            txtDiscount.Visible = false;
            label9.Visible = false;
            //  validate();

        }

        //Methods
        public void Clear()
        {
            cmbProduct.Text = "";
            txtQuantity.Text = "";
            txtPrice.Text = "";
            txtDiscount.Text = "";
            cmbCodeProduct.Text = "";
            richTextBox1.Text = "";
        }

        public void ValidateSalesDetails()
        {
            if (cmbCodeProduct.Text =="" && txtQuantity.Text == "" && txtPrice.Text == ""&&  txtDiscount.Text == "" && cmbCodeProduct.Text =="")
            {
                btnNewDetails.Enabled = false;
            }
            else
            {
                btnNewDetails.Enabled = true;   
            }
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

        public void GridFill()
        {
            try
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
            finally
            {

                Connection.MakeConnection().Close();
            }
        }

        //Client

        private void ComboboxFillClient()
        {
            try
            {
                mySqlCommand = new MySqlCommand("select name from Clients where Status='Ingresado'", Connection.MakeConnection());
                MySqlDataReader myReader;
                myReader = mySqlCommand.ExecuteReader();

                while (myReader.Read())
                {
                    string name = myReader.GetString("name");
                    cmbClient.Items.Add(name);
                }
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
                mySqlCommand = new MySqlCommand("select code from Clients where Status='Ingresado' ", Connection.MakeConnection());
                MySqlDataReader myReader;
                myReader = mySqlCommand.ExecuteReader();
                AutoCompleteStringCollection MyCollection = new AutoCompleteStringCollection();

                while (myReader.Read())
                {
                    string name = myReader.GetString("code");
                    cmbCode.Items.Add(name);
                }
             //   cmbCode.AutoCompleteCustomSource = MyCollection;
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
                mySqlCommand = new MySqlCommand("select nit from Clients where Status='Ingresado'", Connection.MakeConnection());
                MySqlDataReader myReader;
                myReader = mySqlCommand.ExecuteReader();
                AutoCompleteStringCollection MyCollection = new AutoCompleteStringCollection();

                while (myReader.Read())
                {

                    string nit = myReader.GetString("nit");
                    cmbNIT.Items.Add(nit);
                }
            //    cmbNIT.AutoCompleteCustomSource = MyCollection;

            }
            finally
            {
                Connection.MakeConnection().Close();
            }

        }

        private void cmbClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
              //  Connection.MakeConnection().Open();
                mySqlCommand = new MySqlCommand("select ClientID,NIT,Code from Clients where name='" + cmbClient.Text + "'", Connection.MakeConnection());
                MySqlDataReader myReader;
                myReader = mySqlCommand.ExecuteReader();
                while (myReader.Read())
                {
                    string clientID = myReader.GetInt32("ClientID").ToString();
                    string nit = myReader.GetString("NIT");
                    string code = myReader.GetString("Code");
             //       string address = myReader.GetString("Address");
                    salesModel.ClientID = int.Parse(clientID);
                    cmbNIT.Text = nit;
                    cmbCode.Text = code;
                    //txtAddress.Text = address;
                }
            }
            finally
            {
                Connection.MakeConnection().Close();
            }
         //  
        }
        
        private void cmbNIT_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbNIT.SelectedIndex==0)
                {
                    AddClient addClient = new AddClient();
                    addClient.ShowDialog();
                    TextFillNIT();
                }
                else {


                    //Connection.MakeConnection().Open();
                    mySqlCommand = new MySqlCommand("select ClientID,NIT,Code,Name from Clients where  nit='" + cmbNIT.Text + "'", Connection.MakeConnection());
                    MySqlDataReader myReader;
                    myReader = mySqlCommand.ExecuteReader();

                    while (myReader.Read())
                    {
                        string clientID = myReader.GetInt32("ClientID").ToString();
                        string nit = myReader.GetString("NIT");
                        string code = myReader.GetString("Code");
                        string name = myReader.GetString("Name");
                        //     string address = myReader.GetString("address");
                        cmbClient.Text = name;
                        salesModel.ClientID = int.Parse(clientID);
                        cmbNIT.Text = nit;
                        cmbCode.Text = code;

                        Connection.MakeConnection().Close();
                        //    txtAddress.Text = address;
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
                mySqlCommand = new MySqlCommand("select ClientID,NIT,Code,Name from Clients where  code='" + cmbCode.Text + "'", Connection.MakeConnection());
                MySqlDataReader myReader;
                myReader = mySqlCommand.ExecuteReader();

                while (myReader.Read())
                {
                    string clientID = myReader.GetInt32("ClientID").ToString();
                    string nit = myReader.GetString("NIT");
                    string code = myReader.GetString("Code");
                    string name = myReader.GetString("Name");
                    //     string address = myReader.GetString("address");
                    cmbClient.Text = name;
                    salesModel.ClientID = int.Parse(clientID);
                    cmbNIT.Text = nit;
                    cmbCode.Text = code;

                    Connection.MakeConnection().Close();
                    //    txtAddress.Text = address;
                }
            }
            finally
            {
                Connection.MakeConnection().Close();
            }
        }


        //Product

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

                if (cmbCodeProduct.SelectedIndex == 0)
                {
                    AddProduct addProduct = new AddProduct();
                    addProduct.ShowDialog();
                    ComboboxFillProduct();
                }
                else
                {
                    //Connection.MakeConnection().Open();
                    mySqlCommand = new MySqlCommand("select ProductID,Price,Units,Name from Products where code='" + cmbCodeProduct.Text + "'", Connection.MakeConnection());
                    MySqlDataReader myReader;
                    myReader = mySqlCommand.ExecuteReader();


                    while (myReader.Read())
                    {
                        string productID = myReader.GetInt32("ProductID").ToString();
                        string price = myReader.GetString("Price");
                        quantity = myReader.GetString("units");
                        string name = myReader.GetString("Name");
                        salesDetailsModel.ProductID = int.Parse(productID);
                        txtPrice.Text = price;
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
        

        private void ComboboxFillProduct()
        {
            try
            {
                //   Connection.MakeConnection().Open();
                mySqlCommand = new MySqlCommand("select name from Products where Status='Ingresado'", Connection.MakeConnection());
                MySqlDataReader myReader;
                myReader = mySqlCommand.ExecuteReader();

                while (myReader.Read())
                {
                    string name = myReader.GetString("name");
                    cmbProduct.Items.Add(name);

                }
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
                // Connection.MakeConnection().Open();
                mySqlCommand = new MySqlCommand("select ProductID,code,units,SalePrice from Products where  name='" + cmbProduct.Text + "'", Connection.MakeConnection());
                MySqlDataReader myReader;
                myReader = mySqlCommand.ExecuteReader();
                while (myReader.Read())
                {
                    string productID = myReader.GetInt32("ProductID").ToString();
                     quantity = myReader.GetInt32("units").ToString();
                    string name = myReader.GetString("code");
                    string price = myReader.GetInt32("SalePrice").ToString();
                    txtPrice.Text = price;
                    txtQuantity.Text = quantity;
                    var operation = float.Parse(price) * float.Parse(quantity);
                    cmbCodeProduct.Text = name;
                    label13.Text = operation.ToString();
                    salesDetailsModel.ProductID = int.Parse(productID);
                  //  Connection.MakeConnection().Close();
                }
            }
            finally
            {
                Connection.MakeConnection().Close();
            }
        }






        private void btnNew_Click(object sender, EventArgs e)
        {            //Connection.MakeConnection().Open();
            try
            {
            MySqlCommand command = new MySqlCommand("UpdateSales", Connection.MakeConnection());
            salesModel.SalesReference = txtReference.Text;
            salesModel.SalesDate = DateTime.Parse(dtDate.Text);
            salesModel.Serie = txtSerie.Text;
            if (checkBox1.Checked)
            {

                salesModel.SalesReference = "";
                command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("_Series",salesModel.Serie);
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
                command.Parameters.AddWithValue("_Series", salesModel.Serie);
                command.Parameters.AddWithValue("_SalesID", salesModel.SalesID);
                command.Parameters.AddWithValue("_SalesReference", salesModel.SalesReference);
                command.Parameters.AddWithValue("_SalesDate", salesModel.SalesDate);
                command.Parameters.AddWithValue("_ClientID", salesModel.ClientID);
                command.Parameters.AddWithValue("_automatically", true);
                command.ExecuteNonQuery();
                MessageBox.Show("Registro Actualizado Exitosamente", "Registro Actualizado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();

              //  Connection.MakeConnection().Close();
            }
            }
            finally
            {

                Connection.MakeConnection().Close();
            }
         
        }



        private void btnNewDetails_Click(object sender, EventArgs e)
        {
            try
            {

                MySqlCommand command = new MySqlCommand("InsertSalesDetails", Connection.MakeConnection());
                if (txtDiscount.Text=="")
                {
                    salesDetailsModel.Quantity = 0;
                }
                else
                {
                    salesDetailsModel.Quantity = int.Parse(txtQuantity.Text);
                }
                salesDetailsModel.Price = float.Parse(txtPrice.Text);
                salesDetailsModel.Observation = richTextBox1.Text;
                if (txtDiscount.Text != "")
                {
                    salesDetailsModel.Discount = float.Parse(txtDiscount.Text);
                }
                else
                {
                    salesDetailsModel.Discount = 0.00f;
                }
                salesDetailsModel.SubTotal = salesDetailsModel.Quantity * salesDetailsModel.Price-salesDetailsModel.Discount;

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("_Price", salesDetailsModel.Price);
                command.Parameters.AddWithValue("_Quantity", salesDetailsModel.Quantity);
                command.Parameters.AddWithValue("_SubTotal", salesDetailsModel.SubTotal);
                command.Parameters.AddWithValue("_Discount", salesDetailsModel.Discount);
                command.Parameters.AddWithValue("_SalesID", salesDetailsModel.SalesID);
                command.Parameters.AddWithValue("_ProductID", salesDetailsModel.ProductID);
                command.Parameters.AddWithValue("_Observation", salesDetailsModel.Observation);
                command.ExecuteNonQuery();
                MessageBox.Show("Registro Agregado Exitosamente", "Registro Agregado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnNew.Enabled = true;
                GridFill();
                lbTotal.Text = salesModel.SalesTotal.ToString();
                Clear();
            }

            finally
            {
                Connection.MakeConnection().Close();
            }
        }



     

        private void button1_Click(object sender, EventArgs e)
        {
            salesModel.Serie = txtSerie.Text;
            salesModel.SalesReference = txtReference.Text;
            

            try
            {
                MySqlCommand command = new MySqlCommand("DeleteSales", Connection.MakeConnection());
                command.CommandType = CommandType.StoredProcedure;
                if (txtReference.Text == "" )
                {

                    salesModel.SalesReference = "Nula";

                }
                else
                {
                    salesModel.SalesReference = txtReference.Text;
                }

                if (txtSerie.Text=="") {
                    salesModel.Serie = "Nula";
                }
                else
                {
                    salesModel.Serie = "Nula";
                }
                if (cmbNIT.Text == "")
                {
                    salesModel.ClientID = 1;
                }

                command.Parameters.AddWithValue("_SalesID", salesModel.SalesID);
                command.Parameters.AddWithValue("_SalesReference", salesModel.SalesReference);
                command.Parameters.AddWithValue("_Series", salesModel.Serie);
                command.Parameters.AddWithValue("_ClientID", salesModel.ClientID);


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

        private void txtQuantity_TextChanged_1(object sender, EventArgs e)
        {
            ValidateSalesDetails();
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
            control = int.Parse(quantity);

            if (quantity1 >control) 
            {
                MessageBox.Show("No hay suficientes productos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnNewDetails.Enabled = false;
            }

        }

        private void txtPrice_TextChanged_1(object sender, EventArgs e)
        {
            ValidateSalesDetails();
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
                    salesDetailsModel.SalesDetailsID = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value.ToString());                
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Debe de escoger un código válido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnDelete_Click(object sender, EventArgs e)
        {
            MySqlCommand command = new MySqlCommand("DeleteSalesDetails", Connection.MakeConnection());
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("_SalesDetailsID", salesDetailsModel.SalesDetailsID);
            DialogResult dialogResult = MessageBox.Show("Desea borrar este articulo", "Eliminar Registro", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                command.ExecuteNonQuery();
                Clear();
                GridFill();
                // MessageBox.Show("Registro Elimnado Exitosamente", "Registro Eliminado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    this.Close();
            }
        }

        private void cmbCodeProduct_TextChanged(object sender, EventArgs e)
        {
            ValidateSalesDetails();
        }

        private void cmbProduct_TextChanged(object sender, EventArgs e)
        {
            ValidateSalesDetails();
        }

        private void ckDiscount_CheckedChanged(object sender, EventArgs e)
        {
            if (ckDiscount.Checked)
            {
                txtDiscount.Visible = true;
                label9.Visible = true;
            }
            else{
                txtDiscount.Visible = false;    
                label9.Visible = false;
            }
        }

        private void AddSale_Load(object sender, EventArgs e)
        {

        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle,0x11,0xf012,0);
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

            label13.Text = (price1 * quantity1-discount1).ToString();
        }

        private void txtPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            OnlyNumbers(sender,e);
        }

        private void txtQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            OnlyNumbers(sender,e);
        }

        private void txtDiscount_KeyPress(object sender, KeyPressEventArgs e)
        {
            OnlyNumbers(sender,e);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

       

    }
}


