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
                    int sel = SelectedOrder.CourierID;
                    if (SelectedOrder.CourierID == 1)
                    {
                        SelectedOrder.Courier.QuantityProduct = 0;
                    }
                    else if(SelectedOrder.Courier.QuantityProduct - SelectedOrder.Quantity < 0)
                    {
                        SelectedOrder.Courier.QuantityProduct = 0;
                    } else SelectedOrder.Courier.QuantityProduct -= SelectedOrder.Quantity;

                    SelectedOrder.IsReady = false;
                    SelectedOrder.CourierID = 1;
                    SelectedOrder.Product.Quantity += SelectedOrder.Quantity;

                    if (SelectedOrder.Product.Quantity > 0)
                    {
                        SelectedOrder.Product.IsReadyProd = true;
                    }

                    OrderDB.GetDb().Update(SelectedOrder);
                    CourierDB.GetDb().Update(SelectedOrder.Courier);
                    ProductDB.GetDb().Update(SelectedOrder.Product);
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
