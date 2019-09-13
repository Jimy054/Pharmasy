using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Model
{
    class ProductModel
    {
        public int ProductID { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Quantity { get; set; }

        public int Units { get; set; }


        public decimal Price { get; set; }

        public decimal PriceSale { get; set; }

        public decimal Gain { get; set; }
        

        public string Image { get; set; }

        [ForeignKey("Category")]
        public int CategoryID { get; set; }
        public  CategoryModel Category { get; set; }

        [ForeignKey("Provider")]
        public int ProviderID { get; set; }
        public CategoryModel Provider { get; set; }




    }
}
