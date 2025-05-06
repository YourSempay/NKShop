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
        }

        private void SelectAll()
        {
            Orders = new ObservableCollection<Order>(OrderDB.GetDb().SelectAll());
        }

    }
}
