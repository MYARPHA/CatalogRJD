using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogRJD.Library.AI
{
    public class ProductParameters
    {
        public ProductParameter[] product_parameters { get; set; }

        public class ProductParameter
        {
            public string parameter_name { get; set; }
            public string parameter_value { get; set; }
        }
    }
}