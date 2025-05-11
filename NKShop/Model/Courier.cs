using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NKShop.Model
{
    public class Courier
    {
        private string fio;

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public int Pledge { get; set; }
        public DateTime WorkStart { get; set; }
        public int QuantityProduct { get; set; }

        public bool CourierIsFree { get; set; }

        public string CourierIsFreeToSting
        {
            get
            {
                if (CourierIsFree == false)
                    return "    х Курьер на заказе х ";
                else return "    ✔ Курьер свободен ✔ ";
            }
        }


        public string FIO
        {
            get
            {
                if (string.IsNullOrWhiteSpace(FirstName) && string.IsNullOrWhiteSpace(Patronymic))
                {
                    return LastName;
                }
                else
                {
                    return LastName + " " + FirstName[0] + "." + " " + Patronymic[0] + ".";
                }
            }
        }
    }
}
