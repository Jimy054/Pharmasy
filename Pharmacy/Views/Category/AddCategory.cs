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
    public partial class AddCategory : Form
    {
        CategoryModel categoryModel = new CategoryModel();
        public AddCategory()
        {
            InitializeComponent();
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
        private void button1_Click(object sender, EventArgs e)
        {
            MySqlCommand command = new MySqlCommand("GenerateCodeCategory", Connection.MakeConnection());
            categoryModel.Code = txtCode.Text;
            categoryModel.Name = txtName.Text;

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
                    this.Close();
                }
                catch (Exception)
                {
                    MessageBox.Show("Código  Repetido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    this.Close();
                }
                catch (Exception)
                {
                    MessageBox.Show("Código  Repetido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
