using Pharmacy.Views;
using Pharmacy.Views.Menu;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pharmacy
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void OpenDialog(object form)
        {
            if (this.panel2.Controls.Count > 0)
                this.panel2.Controls.RemoveAt(0);
            Form myForm = form as  Form;
            myForm.TopLevel = false;
            myForm.AutoScroll = true;
            panel2.Controls.Add(myForm);
            myForm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenDialog(new CategoryView());
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            OpenDialog(new Dashboard());
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenDialog(new ClientView());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenDialog(new ProviderView());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenDialog(new ProductView());
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OpenDialog(new PurchaseView());
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenDialog(new SalesView());
        }

        private void button8_Click(object sender, EventArgs e)
        {
            OpenDialog(new Dashboard());
        }
    }
}
