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

        private void SelectAll()
        {
            Couriers = new ObservableCollection<Courier>(CourierDB.GetDb().SelectAll());
        }

       public PurposeCourierMVVM()
        {
            SelectAll();

            Save = new CommandMvvm(() =>
            {
                // ИЗМЕНИТЬ
                OrderDB.GetDb().Update();
                SelectAll();
            }, () => SelectedCourier != null);
        }

    }
}
