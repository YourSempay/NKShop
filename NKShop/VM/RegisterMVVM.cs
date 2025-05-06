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
    internal class RegisterMVVM : BaseVM
    {
        public CommandMvvm Login { get; set; }
        public string LoginAcc { get; set; }
        public string PasswordAcc { get; set; }

        public RegisterMVVM()
        {
            Login = new CommandMvvm(() =>
            {
                List<Account> accounts = AccountDB.GetDb().SelectLogin(LoginAcc, PasswordAcc); 

                if(accounts.Count > 0)
                {
                    OrdersList ol = new OrdersList();
                    ol.Show();
                    close?.Invoke();
                } else MessageBox.Show("Логин или пароль не найден");

            }, () => !string.IsNullOrWhiteSpace(LoginAcc) && !string.IsNullOrWhiteSpace(PasswordAcc));
            // не позволяет нажать кнопку если нижнее условие не выполняется
        }

        Action close;
        private string logins;

        internal void SetClose(Action close)
        {
            this.close = close;
        }



    }
}
