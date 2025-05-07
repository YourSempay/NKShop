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
    internal class OrderListMVVM : BaseVM
    {

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

        public CommandMvvm OrderReady { get; set; }
        public CommandMvvm CourierNew { get; set; }
        public CommandMvvm ListCouriers { get; set; }
        public CommandMvvm ListProducts { get; set; }
        public CommandMvvm HistoryWindow { get; set; }

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

        public OrderListMVVM()
        {
            SelectAll();

            OrderReady = new CommandMvvm(() =>
            {
                var orderreturn = MessageBox.Show("Вы уверены что хотите пометить заказ готовым?", "Подтверждение", MessageBoxButton.YesNo);

                if (orderreturn == MessageBoxResult.Yes)
                {
                    SelectedOrder.IsReady = true;

                    OrderDB.GetDb().Update(SelectedOrder);
                    SelectAll();
                }


                SelectAll();
            }, () => SelectedOrder != null);

            CourierNew = new CommandMvvm(() =>
            {
                PurposeCourier pc = new PurposeCourier();
                pc.ShowDialog();
                SelectAll();
            }, () => SelectedOrder != null);

            ListCouriers = new CommandMvvm(() =>
            {
                CourierList cl = new CourierList();
                cl.ShowDialog();
                SelectAll();
            }, () => true);

            ListProducts = new CommandMvvm(() =>
            {
                ProductsList pl = new ProductsList();
                pl.ShowDialog();
                SelectAll();
            }, () => true);

            HistoryWindow = new CommandMvvm(() =>
            {
                OrderHistory oh = new OrderHistory();
                oh.ShowDialog();
                SelectAll();
            }, () => true);

        }

        private void SelectAll()
        {
            Orders = new ObservableCollection<Order>(OrderDB.GetDb().SelectAll());
        }

    }
}
