using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Model
{
    class SaleModel
    {
        public int SalesID { get; set; }
        public DateTime SalesDate { get; set; }
        public float SalesTotal { get; set; }
        public string SalesReference { get; set; }
        public string Serie { get; set; }
        [ForeignKey("Clients")]
        public int ClientID { get; set; }
        public ICollection<ClientModel> Client { get; set; }
    }
}
