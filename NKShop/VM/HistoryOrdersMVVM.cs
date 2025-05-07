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
    internal class HistoryOrdersMVVM : BaseVM
    {
        public CommandMvvm OrderNotReady { get; set; }

        private Order selectedorder;

        public Order SelectedOrder
        {
            get => selectedorder;
            set
            {
                selectedorder = value;
                Signal();
            }
        }

        private ObservableCollection<Order> orders = new();

        public ObservableCollection<Order> Orders
        {
            get => orders;
            set
            {
                orders = value;
                Signal();
            }
        }
        public HistoryOrdersMVVM()
        {
            SelectAllHis();

            OrderNotReady = new CommandMvvm(() =>
            {
                var orderreturn = MessageBox.Show("Вы уверены что хотите пометить заказ неготовым?", "Подтверждение", MessageBoxButton.YesNo);

                if (orderreturn == MessageBoxResult.Yes)
                {
                    SelectedOrder.IsReady = false;
                    SelectedOrder.CourierID = 1;

                    OrderDB.GetDb().Update(SelectedOrder);
                    SelectAllHis();
                }


                SelectAllHis();
            }, () => SelectedOrder != null);

        }


        private void SelectAllHis()
        {
            Orders = new ObservableCollection<Order>(OrderDB.GetDb().SelectAllHis());
        }

    }
}
