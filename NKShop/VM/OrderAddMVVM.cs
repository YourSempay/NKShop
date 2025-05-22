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
    internal class OrderAddMVVM : BaseVM
    {

        private Product selectedProduct;

        public Product SelectedProduct
        {
            get => selectedProduct;
            set
            {
                selectedProduct = value;
                Signal();
            }
        }

        private int quantityN;
        public int QuantityN
        {
            get => quantityN;
            set
            {
                quantityN = value;
                Signal();
            }
        }

        private string deliveryAddress;
        public string DeliveryAddress
        {
            get => deliveryAddress;
            set
            {
                deliveryAddress = value;
                Signal();
            }
        }

        public CommandMvvm CreateOrder { get; set; }

        private ObservableCollection<Product> products = new();

        public ObservableCollection<Product> Products
        {
            get => products;
            set
            {
                products = value;
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
        public OrderAddMVVM()
        {
            SelectAll();
            int currentUserId = RegisterMVVM.Session.UserId;

            Account useaccount = Accounts.FirstOrDefault(acc => acc.Id == currentUserId);
            DeliveryAddress = useaccount.AddressStandart;
            CreateOrder = new CommandMvvm(() =>
            {
                if (useaccount.FirstName != "Не введено")
                {
                    if (useaccount.LastName != "Не введено")
                    {
                        if (useaccount.Patronymic != "Не введено")
                        {
                            var orderreturn = MessageBox.Show($"Вы уверены что хотите создать заказ?", "Подтверждение", MessageBoxButton.YesNo);

                            if (orderreturn == MessageBoxResult.Yes)
                            {
                                Order neworder = new Order();
                                neworder.Quantity = QuantityN;
                                neworder.FullPrice = SelectedProduct.Price * QuantityN;
                                neworder.Coordinates = DeliveryAddress;
                                neworder.CourierID = 1;
                                neworder.IsReady = false;
                                neworder.ProductID = SelectedProduct.Id;
                                neworder.AccountID = useaccount.Id;
                                neworder.Account = useaccount;
                                OrderDB.GetDb().Insert(neworder);
                                close?.Invoke();
                            }

                        } else MessageBox.Show($"Для начала введите отчество", "Ошибка!");
                    } else MessageBox.Show($"Для начала введите фамилию", "Ошибка!"); 
                } else MessageBox.Show($"Для начала введите имя", "Ошибка!");

            }, () =>
            QuantityN < 10000
            && QuantityN >= 100
           && SelectedProduct != null
           && !string.IsNullOrWhiteSpace(DeliveryAddress));

        }

        Action close;
        internal void SetClose(Action close)
        {
            this.close = close;
        }

        private void SelectAll()
        {
            Products = new ObservableCollection<Product>(ProductDB.GetDb().SelectAllUser());
            Accounts = new ObservableCollection<Account>(AccountDB.GetDb().SelectAllUser());
        }

    }
}
