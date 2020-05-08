using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore
{
    public class Product
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int SupplierID { get; set; }
        public string SupplierName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public Product()
        {

        }
    }
}
