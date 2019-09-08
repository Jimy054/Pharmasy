using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Model
{
    class CategoryModel
    {
        public int CategoryID { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public virtual ICollection<ProductModel> ProductModel { get; set; }
    }
}
