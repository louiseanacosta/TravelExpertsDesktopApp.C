using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travel_Experts
{
    /*
     * Author: DongMing Hu
     * Date: 4/5/2019
     * Purpose: A customized class to show Product Supplier with names instead of ids
     */
    public class ProductSupplierWithName
    {
        public int ProductSupplierId { get; set; }
        public string ProdName { get; set; }
        public string SupName { get; set; }
    }
}
