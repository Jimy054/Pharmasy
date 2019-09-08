using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Model
{
    class SalesDetailsModel
    {
        public int SalesDetailsID { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }
        public float SubTotal { get; set; }
        public float Discount { get; set; }

        [ForeignKey("Sales")]
        public int SalesID { get; set; }
        public ICollection<SaleModel> Sales { get; set; }

        [ForeignKey("Products")]
        public int ProductID { get; set; }
        public ICollection<ProductModel> Product { get; set; }
    }
}
