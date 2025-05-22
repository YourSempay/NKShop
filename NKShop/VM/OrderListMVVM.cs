using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Mysqlx.Connection;
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

        public CommandMvvm OrderReady { get; set; }
        public CommandMvvm CourierNew { get; set; }
        public CommandMvvm ListCouriers { get; set; }
        public CommandMvvm ListProducts { get; set; }
        public CommandMvvm HistoryWindow { get; set; }
        public CommandMvvm RedactOrder { get; set; }
        public CommandMvvm DeleteOrder { get; set; }

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

        private ObservableCollection<Account> accounts = new();

        public ObservableCollection<Account> Accounts
        {
            get => accounts;
            set
            {
                accounts = value;
                Signal();
            }
        }

        public OrderListMVVM()
        {
            SelectAll();


            OrderReady = new CommandMvvm(() =>
            {
                if (SelectedOrder.ProductID != 1)
                {
                    if (SelectedOrder.Product.TitleBlock != "Товар в блоке")
                    {
                          if (SelectedOrder.CourierID != 1)
                          {
                                if (SelectedOrder.Product.Quantity - SelectedOrder.Quantity >= 0)
                                {
                                    var orderreturn = MessageBox.Show("Вы уверены что хотите пометить заказ готовым?", "Подтверждение", MessageBoxButton.YesNo);
                                    Product product = SelectedOrder.Product;

                                    if (orderreturn == MessageBoxResult.Yes)
                                    {
                                        SelectedOrder.IsReady = true;
                                        if (SelectedOrder.CourierID == 1)
                                        {
                                            SelectedOrder.Courier.QuantityProduct = 0;
                                        }
                                        else SelectedOrder.Courier.QuantityProduct += SelectedOrder.Quantity;

                                        SelectedOrder.Product.Quantity -= SelectedOrder.Quantity;

                                        if (SelectedOrder.Product.Quantity == 0)
                                        {
                                            product.IsReadyProd = false;
                                        }

                                        OrderDB.GetDb().Update(SelectedOrder);
                                        CourierDB.GetDb().Update(SelectedOrder.Courier);
                                        ProductDB.GetDb().Update(product);
                                        SelectAll();
                                    }
                                }
                                else MessageBox.Show($"На складе нет столько товара! Не хватает: {Math.Abs(SelectedOrder.Product.Quantity - SelectedOrder.Quantity)}г.", "Ошибка!");
                          } else MessageBox.Show("Курьер обязательно должен быть назначен!", "Ошибка!");
                    } else MessageBox.Show("Товар заблокирован. Для начала разблокируйте его!", "Ошибка!");
                } else MessageBox.Show("Товар в заказ не назначен. Заказ не может быть выполнен!", "Ошибка!");
                    





                SelectAll();
            }, () => SelectedOrder != null);

            RedactOrder = new CommandMvvm(() =>
            {
                var redactWindow = new OrderEditWindow(SelectedOrder);
                redactWindow.ShowDialog();
                SelectAll();
            }, () => SelectedOrder != null);


            DeleteOrder = new CommandMvvm(() =>
            {
                if (SelectedOrder.Product.IsReadyProd == true)
                {
                    if (SelectedOrder.ProductID == 1)
                    {
                        var res = MessageBox.Show("Вы уверены, что хотите удалить заказ?", "Подтверждение", MessageBoxButton.YesNo);
                        if (res == MessageBoxResult.Yes)
                        {
                            OrderDB.GetDb().Delete(SelectedOrder);
                            MessageBox.Show("Заказ успешно удален");
                            SelectAll();
                        }

                    } else MessageBox.Show("Удалить нельзя, в заказ назначен товар", "Ошибка");
                } else MessageBox.Show("Нельзя удалить заказ, так как товар заблокирован!", "Ошибка");

            }, () => SelectedOrder != null);


            CourierNew = new CommandMvvm(() =>
            {
                if (SelectedOrder.ProductID != 1)
                {
                    if (SelectedOrder.Product.TitleBlock != "Товар в блоке")
                    {
                        PurposeCourier pc = new PurposeCourier(SelectedOrder);
                        pc.ShowDialog();
                        SelectAll();
                    }
                    else MessageBox.Show("Товар заблокирован. Для начала разблокируйте его!", "Ошибка!");
                } else MessageBox.Show("Товар в заказ не назначен. Курьер не может выполнить этот заказ!", "Ошибка!");
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
            Couriers = new ObservableCollection<Courier>(CourierDB.GetDb().SelectAll());
            Accounts = new ObservableCollection<Account>(AccountDB.GetDb().SelectAllUser());
        }

        Action close;
        internal void SetClose(Action close)
        {
            this.close = close;
        }

    }
}
