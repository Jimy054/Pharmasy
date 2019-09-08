using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pharmacy.DB;
using Pharmacy.Model;
using MySql.Data.MySqlClient;

namespace Pharmacy.Views

{
    public partial class CategoryView : Form
    {
        CategoryModel categoryModel = new CategoryModel();
        public CategoryView()
        {
            InitializeComponent();
            GridFill();
        }

        private void CategoryView_Load(object sender, EventArgs e)
        {
        }

        public void Clear()
        {
            txtName.Text = "";
            txtCode.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {

            switch (button1.Text)
            {
                case "Nuevo":
                    button1.Text = "Guardar";
                    button3.Text = "Cancelar";
                    button2.Enabled = false;
                    Clear();
                    break;
                case "Guardar":

                    categoryModel.Code = txtCode.Text;
                    categoryModel.Name = txtName.Text;

                    MySqlCommand command = new MySqlCommand("GenerateCodeCategory", Connection.MakeConnection());
                    if (ckGenerate.Checked)
                    {
                        categoryModel.Code = "";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("_code", categoryModel.Code);
                        command.Parameters.AddWithValue("_name", categoryModel.Name);
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

                            MessageBox.Show("Código Repetido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("_name", categoryModel.Name);
                        command.Parameters.AddWithValue("_code", categoryModel.Code);
                        command.Parameters.AddWithValue("_automatically", true);
                        //command.ExecuteNonQuery();
                        try
                        {

                            command.ExecuteNonQuery();
                            MessageBox.Show("Registro Agregado Exitosamente", "Registro Agregado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            GridFill();
                            Clear();
                        }
                        catch (Exception)
                        {

                            MessageBox.Show("Código Repetido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    button1.Text = "Nuevo";

                    button2.Enabled = true;
                    button3.Enabled = true;
                    break;
            }

        }


        public void GridFill()
        {
            MySqlDataAdapter sqlData = new MySqlDataAdapter("CategoryList", Connection.MakeConnection());
            sqlData.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dtCategory = new DataTable();
            sqlData.Fill(dtCategory);


            dataGridView1.DataSource = dtCategory;
            DataGridViewColumn column = dataGridView1.Columns[0];
            column.Visible = false;
            DataGridViewColumn column1 = dataGridView1.Columns[1];
            DataGridViewColumn column2 = dataGridView1.Columns[2];      
            column1.Width = 340;
            column2.Width = 340;
            
            //     80.Columns.GetColumnsWidth();

        }

        private void ckGenerate_CheckedChanged(object sender, EventArgs e)
        {
            if (ckGenerate.Checked)
            {
                txtCode.ReadOnly = true;

            }
            else
            {
                txtCode.ReadOnly = false;
            }



        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            try
            {

                if (dataGridView1.CurrentRow.Index != -1)
                {


                }
                else
                {
                    txtCode.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                    txtName.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                    categoryModel.CategoryID = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value.ToString());
                }
            }
            catch (Exception)
            {


                MessageBox.Show("Debe de escoger un código válido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MySqlCommand command = new MySqlCommand("UpdateCategory", Connection.MakeConnection());
            categoryModel.Code = txtCode.Text;
            categoryModel.Name = txtName.Text;
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("_CategoryID", categoryModel.CategoryID);            
            command.Parameters.AddWithValue("_code", categoryModel.Code);
            command.Parameters.AddWithValue("_name", categoryModel.Name);
            command.Parameters.AddWithValue("_automatically", false);
            try
            {

                command.ExecuteNonQuery();
                MessageBox.Show("Registro Actualizado Exitosamente", "Registro Actualizado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                GridFill();
                Clear();
            }
            catch (Exception)
            {

                MessageBox.Show("Código Repetido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (button3.Text=="Cancelar")
            {
                button1.Text = "Nuevo";
                button2.Enabled = true;
                button3.Enabled = true;
                button3.Text = "Eliminar";
            }
            else { 
                MySqlCommand command = new MySqlCommand("DeleteCategory", Connection.MakeConnection());
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("_CategoryID", categoryModel.CategoryID);
                DialogResult dialogResult=  MessageBox.Show("Desea Eliminar Este Registro", "Eliminar Registro", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
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
