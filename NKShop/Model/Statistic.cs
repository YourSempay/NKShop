using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NKShop.Model
{
    public class Statistic
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int QuantityOrders { get; set; }
        public decimal QuantitySells { get; set; }
        public int ProductId { get; set; }

    }
}
