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
        public bool IsReadyProd { get; set; }

        public string TitleBlock
        {
            get
            {
                if (IsReadyProd == true)
                {
                    return Title;
                }
                else
                {
                    return "Товар в блоке";
                }
            }
        }

        public string IsReadyText
        {
            get
            {
                if (IsReadyProd == false)
                    return "    х Не продаётся х ";
                else return "    ✔ Продаётся ✔ ";
            }
        }
    }
}
