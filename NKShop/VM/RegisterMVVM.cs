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
                    close?.Invoke();
                    ol.ShowDialog();
                } else MessageBox.Show("Логин или пароль не найден");

            }, () => !string.IsNullOrWhiteSpace(LoginAcc) && !string.IsNullOrWhiteSpace(PasswordAcc));
        }

        Action close;

        internal void SetClose(Action close)
        {
            this.close = close;
        }



    }
}
