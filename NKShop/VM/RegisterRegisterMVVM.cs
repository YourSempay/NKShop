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
    public class RegisterRegisterMVVM : BaseVM
    {
        public CommandMvvm NewAcc { get; set; }

        private string newLogin;
        public string NewLogin
        {
            get => newLogin;
            set
            {
                newLogin = value;
                Signal();
                ValidateCanCreate();
            }
        }

        private string isAdm;
        public string IsAdm
        {
            get => isAdm;
            set
            {
                isAdm = value;
                Signal();
                ValidateCanCreate();
            }
        }

        private string newPassword;
        public string NewPassword
        {
            get => newPassword;
            set
            {
                newPassword = value;
                Signal();
                ValidateCanCreate();
            }
        }

        private string newPasswordTwo;
        public string NewPasswordTwo
        {
            get => newPasswordTwo;
            set
            {
                newPasswordTwo = value;
                Signal();
                ValidateCanCreate();
            }
        }

        private string newNumberPhone;
        public string NewNumberPhone
        {
            get => newNumberPhone;
            set
            {
                newNumberPhone = value;
                Signal();
                ValidateCanCreate();
            }
        }

        private string selectedRole;
        public string SelectedRole
        {
            get => selectedRole;
            set
            {
                selectedRole = value;
                Signal();
                ValidateCanCreate();
                UpdateIsAdmBool();
            }
        }

        private bool isAdmBool;
        public bool IsAdmBool
        {
            get => isAdmBool;
            private set
            {
                isAdmBool = value;
                Signal();
            }
        }

        private bool canCreate;
        public bool IsCreateAccEnabled
        {
            get => canCreate;
            private set
            {
                canCreate = value;
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


        public RegisterRegisterMVVM()
        {
            NewAcc = new CommandMvvm(() =>
            {
                SelectAll();
                var existingAccount = Accounts.FirstOrDefault(a => a.Login.Equals(NewLogin, StringComparison.OrdinalIgnoreCase));
                if (existingAccount != null)
                {
                    MessageBox.Show("Такой логин уже существует. Пожалуйста, выберите другой логин.", "Ошибка");
                    return;
                }

                var account = new Account
                {
                    Login = NewLogin,
                    Password = NewPassword,
                    NumberAccount = NewNumberPhone,
                    Adm = IsAdmBool
                };

                AccountDB.GetDb().Insert(account);
                MessageBox.Show("Аккаунт успешно создан!");
                close?.Invoke();
            }, () => IsCreateAccEnabled);
        }

        private void ValidateCanCreate()
        {
            bool isLoginValid = !string.IsNullOrWhiteSpace(NewLogin) &&
                                NewLogin.Length > 5 &&
                                NewLogin.All(c => (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'));

            bool isPasswordValid = !string.IsNullOrWhiteSpace(NewPassword) &&
                                   !string.IsNullOrWhiteSpace(NewPasswordTwo) &&
                                   NewPassword == NewPasswordTwo &&
                                   NewPassword.Length > 5;

            bool isPhoneValid = IsRussianPhoneNumber(NewNumberPhone);

            bool isRoleSelected = !string.IsNullOrEmpty(SelectedRole);

            IsCreateAccEnabled = isLoginValid && isPasswordValid && isPhoneValid && isRoleSelected;
        }

        private void UpdateIsAdmBool()
        {
            IsAdmBool = SelectedRole == "Администратор";
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
            Accounts = new ObservableCollection<Account>(AccountDB.GetDb().SelectAll());
        }
    }


}
