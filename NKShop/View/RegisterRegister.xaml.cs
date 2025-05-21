using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using NKShop.View;
using NKShop.VM;

namespace NKShop.View
{
    /// <summary>
    /// Логика взаимодействия для RegisterRegister.xaml
    /// </summary>
    public partial class RegisterRegister : Window
    {
        public RegisterRegister()
        {
            InitializeComponent();
            var vm = new RegisterRegisterMVVM();
            vm.SetClose(() => this.Close()); // Передайте метод закрытия окна
            this.DataContext = vm;
        }
    }
}
