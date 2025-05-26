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
    internal class UserOrderMVVM : BaseVM
    {

        private ObservableCollection<Order> orders = new();
        public CommandMvvm FilterReadyOrders { get; set; }
        public CommandMvvm FilterNotReadyOrders { get; set; }
        public CommandMvvm CreateNewOrder { get; set; }
        public CommandMvvm Profile { get; set; }

        public CommandMvvm Catalog { get; set; }
        public ObservableCollection<Order> Orders
        {
            get => orders;
            set
            {
                orders = value;
                Signal();
            }
        }

        private Order selectedOrder;

        public Order SelectedOrder
        {
            get => selectedOrder;
            set
            {
                selectedOrder = value;
                Signal();
            }
        }

        public UserOrderMVVM()
        {
            SelectAllNotReady();

            CreateNewOrder = new CommandMvvm(() =>
            {
                OrderAdd oa = new OrderAdd();
                oa.ShowDialog();
                SelectAllNotReady();
            }, () => true);

            Profile = new CommandMvvm(() =>
            {
                Profile pf = new Profile();
                pf.ShowDialog();
                SelectAllNotReady();
            }, () => true);

            Catalog = new CommandMvvm(() =>
            {
                Catalog cl = new Catalog();
                cl.ShowDialog();
                SelectAllNotReady();
            }, () => true);


            FilterReadyOrders = new CommandMvvm(() =>
            {
                SelectAllReady();
            }, () => true);

            FilterNotReadyOrders = new CommandMvvm(() =>
            {
                SelectAllNotReady();
            }, () => true);

        }




        int currentUserId = RegisterMVVM.Session.UserId;

        private void SelectAllReady()
        {
            Orders = new ObservableCollection<Order>(OrderDB.GetDb().SelectAllUserReady(currentUserId));
        }

        private void SelectAllNotReady()
        {
            Orders = new ObservableCollection<Order>(OrderDB.GetDb().SelectAllUserNotReady(currentUserId));
        }


    }
}
