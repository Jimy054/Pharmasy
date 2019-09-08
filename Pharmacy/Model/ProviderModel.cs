using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Model
{
    class ProviderModel
    {
        public int ProviderID { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Telephone { get; set; }

        public string Telephone2 { get; set; }

        public string Email { get; set; }

        public string MethodPayment { get; set; }

        public string NIT { get; set; }

        public string Code { get; set; }

        public virtual ICollection<ProductModel> ProductModel { get; set; }
    }
}
