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
    internal class CourierListMVVM : BaseVM
    {
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

        public CommandMvvm AddCourier { get; set; }
        public CommandMvvm EditCourier { get; set; }
        public CommandMvvm DeleteCourier { get; set; }


        public CourierListMVVM()
        {
            SelectAll();

            AddCourier = new CommandMvvm(() =>
            {
                CourierAdd ca = new CourierAdd(SelectedCourier);
                ca.ShowDialog();
                SelectAll();
            }, () => true);
        }

        private void SelectAll()
        {
            Couriers = new ObservableCollection<Courier>(CourierDB.GetDb().SelectAllProtect());
        }


    }
}
