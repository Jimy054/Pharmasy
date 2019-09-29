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
using Pharmacy.Views.Category;

namespace Pharmacy.Views

{
    public partial class CategoryView : Form
    {
        CategoryModel categoryModel = new CategoryModel();
        enum Search { Name,Code}
        DataTable dtCategory;
        Search  code, name;
        //String code, name;

        public CategoryView()
        {
            InitializeComponent();
            GridFill();
            button2.Enabled = false;
            button3.Enabled = false;
        }

        private void CategoryView_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {

            AddCategory addCategory = new AddCategory();
            addCategory.ShowDialog();
            GridFill();
        }


        public void GridFill()
        {
            MySqlDataAdapter sqlData = new MySqlDataAdapter("CategoryList", Connection.MakeConnection());
            sqlData.SelectCommand.CommandType = CommandType.StoredProcedure;
             dtCategory = new DataTable();
            sqlData.Fill(dtCategory);
            

            dataGridView1.DataSource = dtCategory;
            DataGridViewColumn column = dataGridView1.Columns[0];
            column.Visible = false;
            DataGridViewColumn column1 = dataGridView1.Columns[1];
            DataGridViewColumn column2 = dataGridView1.Columns[2];      
            column1.Width = 416;
            column2.Width = 416;
            
            //     80.Columns.GetColumnsWidth();

        }

      

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            try
            {

                if (dataGridView1.CurrentRow.Index != -1)
                {
                    categoryModel.Code = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                    categoryModel.Name = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                    categoryModel.CategoryID = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value.ToString());
                    button2.Enabled = true;
                    button3.Enabled = true;
                }
                else
                {
                    
                }
            }
            catch (Exception)
            {


                MessageBox.Show("Debe de escoger un código válido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            UpdateCategory updateCategory = new UpdateCategory(categoryModel.CategoryID,categoryModel.Name,categoryModel.Code);            
            updateCategory.ShowDialog();
            GridFill();

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            DataView dv = new DataView(dtCategory);
            
            if (code == Search.Code && name == Search.Code)
            {
                dv.RowFilter = string.Format("Codigo like '%{0}%'", textBox1.Text);
                dataGridView1.DataSource = dv;               
            }

            else if (name==Search.Name && code== Search.Name) 
            {
                dv.RowFilter = string.Format("Nombre like '%{0}%'", textBox1.Text);
                dataGridView1.DataSource = dv;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                MySqlCommand command = new MySqlCommand("DeleteCategory", Connection.MakeConnection());
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("_CategoryID", categoryModel.CategoryID);
                DialogResult dialogResult = MessageBox.Show("Desea Eliminar Este Registro", "Eliminar Registro", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {

                    command.ExecuteNonQuery();
                    GridFill();
                }

            }
            catch (Exception)
            {
                MessageBox.Show("Esta categoría esta en uso", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Connection.MakeConnection().Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var form = Application.OpenForms.OfType<CanceledCategory>().FirstOrDefault();
            if (form == null)
            {
                form = new CanceledCategory();
                form.StartPosition = FormStartPosition.CenterParent;
                form.ShowDialog(this);
                // this.Hide();
            }
            form.Activate();

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                      code= Search.Code;
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
