using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Product
    {
        public string UniqueIdentifier { get; set; }
        public string Barcode { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
    }
}
