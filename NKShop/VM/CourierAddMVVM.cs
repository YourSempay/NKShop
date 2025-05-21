using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NKShop.Model;
using NKShop.View;
using NKShop.VM;
using static System.Reflection.Metadata.BlobBuilder;

namespace NKShop.VM
{
    internal class CourierAddMVVM : BaseVM
    {
        public CommandMvvm AddCourier { get; set; }


        private string windowinfo;

        public string WindowInfo
        {
            get => windowinfo;
            set
            {
                windowinfo = value;
                Signal();
            }
        }


        private string buttonadd;

        public string ButtonAdd
        {
            get => buttonadd;
            set
            {
                buttonadd = value;
                Signal();
            }
        }

        private string courierfirstname;
        public string CourierFirstName
        {
            get => courierfirstname;
            set
            {
                courierfirstname = value;
                Signal();
            }
        }

        private string courierlastName;
        public string CourierLastName
        {
            get => courierlastName;
            set
            {
                courierlastName = value;
                Signal();
            }
        }

        private string courierpatronymic;
        public string CourierPatronymic
        {
            get => courierpatronymic;
            set
            {
                courierpatronymic = value;
                Signal();
            }
        }

        private string courierpledge;
        public string CourierPledge
        {
            get => courierpledge;
            set
            {
                courierpledge = value;
                Signal();
            }
        }

        private DateTime courierworkstart;
        public DateTime CourierWorkStart
        {
            get => courierworkstart;
            set
            {
                courierworkstart = value;
                Signal();
            }
        }

        private Courier selectedcourier;

        public Courier SelectedCourier
        {
            get => selectedcourier;
            set
            {
                selectedcourier = value;
                Signal();
            }
        }

        public CourierAddMVVM()
        {
            AddCourier = new CommandMvvm(() =>
            {
                var orderreturn = MessageBox.Show($"Вы уверены что хотите {ButtonAdd}?", "Подтверждение", MessageBoxButton.YesNo);

                if (orderreturn == MessageBoxResult.Yes)
                {
                    if (SelectedCourier.Id == 0)
                    {
                        SelectedCourier.WorkStart = CourierWorkStart;
                        SelectedCourier.Pledge = CourierPledge;
                        SelectedCourier.FirstName = CourierFirstName;
                        SelectedCourier.LastName = CourierLastName;
                        SelectedCourier.Patronymic = CourierPatronymic;
                        SelectedCourier.QuantityProduct = 0;
                        CourierDB.GetDb().Insert(SelectedCourier);
                    } else
                    {
                        SelectedCourier.WorkStart = CourierWorkStart;
                        SelectedCourier.Pledge = CourierPledge;
                        SelectedCourier.FirstName = CourierFirstName;
                        SelectedCourier.LastName = CourierLastName;
                        SelectedCourier.Patronymic = CourierPatronymic;
                        CourierDB.GetDb().Update(SelectedCourier);
                        MessageBox.Show("Курьер успешно изменён!");

                    }

                    close?.Invoke();
                }

            }, () => 
            CourierWorkStart.Year >= 2000 &&
            !string.IsNullOrWhiteSpace(CourierFirstName) &&
            !string.IsNullOrWhiteSpace(CourierLastName) &&
            !string.IsNullOrWhiteSpace(CourierPatronymic));

        }



        Action close;
        internal void SetClose(Action close)
        {
            this.close = close;
        }
        public void SelCourier(Courier SelCOurier)
        {
            SelectedCourier = SelCOurier;
            if (SelectedCourier.Id == 0)
            {
                ButtonAdd = "Добавить курьера";
                WindowInfo = "Добавление курьера";
                CourierWorkStart = DateTime.Today;
            }
            else
            {
                ButtonAdd = "Изменить информацию о курьере";
                WindowInfo = "Изменение информации о курьере";
                CourierWorkStart = SelectedCourier.WorkStart;
                CourierPledge = SelectedCourier.Pledge;
                CourierFirstName = SelectedCourier.FirstName;
                CourierLastName = SelectedCourier.LastName;
                CourierPatronymic = SelectedCourier.Patronymic;
            }
        }


    }
}
