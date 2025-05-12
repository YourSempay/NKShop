using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
        public int CourierID { get; set; }
        public int ProductID { get; set; }
        public bool IsReady { get; set; }
        public Courier Courier { get; set; }
        public Product Product { get; set; }

        public string IsReadyText
        {
            get
            {
                if (IsReady == false)
                    return "    х Не выполнен х ";
                else return "    ✔ Выполнен ✔ ";
            } 
        }

    }
}
