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
    internal class PurposeCourierMVVM : BaseVM
    {

        private Order selectedorder = new();

        public Order SelectedOrder
        {
            get => selectedorder;
            set
            {
                selectedorder = value;
                Signal();
            }
        }

        public CommandMvvm Save { get; set; }

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

        private ObservableCollection<Courier> couriers = new();

        public ObservableCollection<Courier> Couriers
        {
            get => couriers;
            set
            {
                couriers = value;
                Signal();
            }
        }

        private Account accnow;
        public Account Accnow
        {
            get => accnow;
            set
            {
                accnow = value;
                Signal();
            }
        }
        public PurposeCourierMVVM()
        {
            SelectAll();

            Save = new CommandMvvm(() =>
            {
                SelectedOrder.CourierID = SelectedCourier.Id;
                OrderDB.GetDb().Update(SelectedOrder);
                close?.Invoke();
            }, () => SelectedCourier != null);


        }

        private void SelectAll()
        {
            Couriers = new ObservableCollection<Courier>(CourierDB.GetDb().SelectAll());
        }

        Action close;
        internal void SetClose(Action close)
        {
            this.close = close;
        }
        public void SelOrder(Order SelOr)
        {
            SelectedOrder = SelOr;
        }
    }
}
