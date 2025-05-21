using System;
using System.Windows;
using MySqlX.XDevAPI;
using NKShop.Model;

namespace NKShop.VM
{
    internal class OrderEditViewModel : BaseVM
    {
        public CommandMvvm ShowCustomerInfo { get; set; }
        public CommandMvvm SaveCommand { get; set; }

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

        private string orderCoordinates;
        public string OrderCoordinates
        {
            get => orderCoordinates;
            set
            {
                orderCoordinates = value;
                Signal();
            }
        }

        private int quantityText;
        public int QuantityText
        {
            get => quantityText;
            set
            {
                quantityText = value;
                Signal();
            }
        }

        private string productTitle;
        public string ProductTitle
        {
            get => productTitle;
            set
            {
                productTitle = value;
                Signal();
            }
        }

        private string courierFIO;
        public string CourierFIO
        {
            get => courierFIO;
            set
            {
                courierFIO = value;
                Signal();
            }
        }

        private decimal fullPrice;
        public decimal FullPrice
        {
            get => fullPrice;
            set
            {
                fullPrice = value;
                Signal();
            }
        }

        public OrderEditViewModel()
        {

            SaveCommand = new CommandMvvm(() =>
            {
                var orderreturn = MessageBox.Show($"Вы уверены что хотите изменить информацию о заказе?", "Подтверждение", MessageBoxButton.YesNo);

                if (orderreturn == MessageBoxResult.Yes)
                {
                    SelectedOrder.Coordinates = OrderCoordinates;
                    OrderDB.GetDb().Update(SelectedOrder);
                    close?.Invoke();
                }

            }, () => true);

            ShowCustomerInfo = new CommandMvvm(() =>
            {
                var account = SelectedOrder.Account;
                if (account != null)
                {
                    MessageBox.Show($"ФИО: {account.LastName} {account.FirstName} {account.Patronymic}\n" +
                                    $"Телефон: {account.NumberAccount}\n" +
                                    $"Адрес(По умолчанию): {account.AddressStandart}", "Информация о заказчике");
                }
                else
                {
                    MessageBox.Show("Детали заказчика не найдены");
                }

            }, () => true);
        }

        Action close;
        internal void SetClose(Action close)
        {
            this.close = close;
        }
        public void SelOrder(Order SelOr)
        {
            SelectedOrder = SelOr;

            if (SelectedOrder != null)
            {
                OrderCoordinates = SelectedOrder.Coordinates;
                QuantityText = SelectedOrder.Quantity;
                ProductTitle = SelectedOrder.Product.Title;
                CourierFIO = SelectedOrder.Courier.FIO;
                FullPrice = SelectedOrder.FullPrice;
            }
        }
    }
}