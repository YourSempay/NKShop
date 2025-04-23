using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NKShop.Model
{
    public class Courier
    {
        public int Id { get; set; }
        public string FIO { get; set; }
        public int Pledge { get; set; }
        public DateTime WorkStart { get; set; }
        public int QuantityProduct { get; set; }
    }
}
