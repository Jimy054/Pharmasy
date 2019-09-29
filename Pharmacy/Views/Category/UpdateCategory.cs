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

namespace Pharmacy.Views.Category
{
    public partial class UpdateCategory : Form
    {
        CategoryModel categoryModel = new CategoryModel();

        public UpdateCategory(int id, string name, string code )
        {
            InitializeComponent();
           //MessageBox.Show(id+"");
          //  MessageBox.Show("Name" + name + "Code" + code);
            categoryModel.CategoryID = id;
            txtName.Text = name;
            txtCode.Text = code;
        }
        


        private void button1_Click(object sender, EventArgs e)
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
                this.Close();
            }
            catch (Exception)
            {

                MessageBox.Show("Código Repetido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
