using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NKShop.Model
{
  public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public bool IsReady { get; set; }

        public string IsReadyText
        {
            get
            {
                if (IsReady == false)
                    return "    х Не продаётся х ";
                else return "    ✔ Продаётся ✔ ";
            }
        }
    }
}
