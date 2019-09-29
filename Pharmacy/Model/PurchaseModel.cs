using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Model
{
    class PurchaseModel
    {

        public int PurchasesID { get; set; }
        public DateTime PurchasesDate { get; set; }
        public float  PurchasesTotal { get; set; }
        public string PurchasesReference { get; set; }
        public string Serie { get; set; }
        [ForeignKey("Provider")]
        public int ProviderID { get; set; }
        public ProviderModel Provider { get; set; }

    }
}
