using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySqlX.XDevAPI;
using NKShop.Model;
using NKShop.View;
using NKShop.VM;
using static System.Reflection.Metadata.BlobBuilder;

namespace NKShop.VM
{

    internal class RegisterMVVM : BaseVM
    {
        public CommandMvvm Login { get; set; }

        public CommandMvvm Register { get; set; }
        public string LoginAcc { get; set; }
        public string PasswordAcc { get; set; }

        public static class Session
        {
            public static string LoggedInUser { get; set; }
            public static int UserId { get; set; }
        }
        public RegisterMVVM()
        {
            Login = new CommandMvvm(() =>
            {
                List<Account> accounts = AccountDB.GetDb().SelectLogin(LoginAcc, PasswordAcc); 

                if(accounts.Count > 0)
                {
                    var account = accounts.First();
                    Session.LoggedInUser = account.Login; 
                    Session.UserId = account.Id;
                    bool isAdmin = accounts.Any(a => a.Adm);
                    if (isAdmin)
                    {
                        OrdersList ol = new OrdersList();
                        close?.Invoke();
                        ol.ShowDialog();
                    }
                    else
                    {
                        UserOrder uo = new UserOrder();
                        close?.Invoke();
                        uo.ShowDialog();
                    }
                } else MessageBox.Show("Логин или пароль не найден");

            }, () => !string.IsNullOrWhiteSpace(LoginAcc) && !string.IsNullOrWhiteSpace(PasswordAcc));

            Register = new CommandMvvm(() =>
            {
                RegisterRegister rr = new RegisterRegister();
                rr.ShowDialog();

            }, () => true);
        }

        Action close;

        internal void SetClose(Action close)
        {
            this.close = close;
        }



    }
}
