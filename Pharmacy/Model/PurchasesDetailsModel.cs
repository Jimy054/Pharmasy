using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Model
{
    class PurchasesDetailsModel
    {
        public int PurchasesDetailsID { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }
        public float SubTotal { get; set; }
        public float Discount { get; set; }
        public string Observation { get; set; }
        [ForeignKey("Purchases")]
        public int PurchasesID { get; set; }
        public ICollection<PurchaseModel> Purchases { get; set; }

        [ForeignKey("Prdocuts")]
        public int ProductID { get; set; }
        public ICollection<ProductModel> Product { get; set; }
    }
}
