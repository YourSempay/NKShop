using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NKShop.Model
{
    public class Order
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal FullPrice { get; set; }
        public string Coordinates { get; set; }
    }
}
