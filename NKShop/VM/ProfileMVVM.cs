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
    internal class ProfileMVVM : BaseVM
    {
        public CommandMvvm SaveProfile { get; set; }

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

        private string lastName;
        public string LastName
        {
            get => lastName;
            set
            {
                lastName = value;
                Signal();
            }
        }

        private string firstName;
        public string FirstName
        {
            get => firstName;
            set
            {
                firstName = value;
                Signal();
            }
        }

        private string patronymic;
        public string Patronymic
        {
            get => patronymic;
            set
            {
                patronymic = value;
                Signal();
            }
        }

        private string phoneNumber;
        public string PhoneNumber
        {
            get => phoneNumber;
            set
            {
                phoneNumber = value;
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


        public ProfileMVVM()
        {
            SelectAll();

            int currentUserId = RegisterMVVM.Session.UserId;
            Account useaccount = Accounts.FirstOrDefault(acc => acc.Id == currentUserId);

            SaveProfile = new CommandMvvm(() =>
            {
               useaccount.FirstName = FirstName;
               useaccount.LastName = LastName;
               useaccount.Patronymic = Patronymic;
               useaccount.AddressStandart = DeliveryAddress;
               useaccount.NumberAccount = PhoneNumber;
                AccountDB.GetDb().Update(useaccount);

            }, () => 
            FirstName?.Length > 2 &&
            LastName?.Length > 2 &&
            Patronymic?.Length > 2 &&
            IsRussianPhoneNumber(PhoneNumber) &&
            DeliveryAddress?.Length > 5);


        }

        private bool IsRussianPhoneNumber(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
                return false;

            string cleaned = new string(number.Where(char.IsDigit).ToArray());

            if (cleaned.Length == 11 && (cleaned.StartsWith("7") || cleaned.StartsWith("8")))
            {
                return true;
            }
            return false;
        }


        Action close;
        internal void SetClose(Action close)
        {
            this.close = close;
        }

        private void SelectAll()
        {
            Accounts = new ObservableCollection<Account>(AccountDB.GetDb().SelectAllUser());
            int currentUserId = RegisterMVVM.Session.UserId;
            Account useaccount = Accounts.FirstOrDefault(acc => acc.Id == currentUserId);
            FirstName = useaccount.FirstName;
            LastName = useaccount.LastName;
            Patronymic = useaccount.Patronymic;
            PhoneNumber = useaccount.NumberAccount;
            DeliveryAddress = useaccount.AddressStandart;
        }








    }
}
