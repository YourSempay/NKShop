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
using NKShop.Model;
using NKShop.VM;

namespace NKShop.View
{
    /// <summary>
    /// Логика взаимодействия для OrderAdd.xaml
    /// </summary>
    public partial class OrderAdd : Window
    {
        public OrderAdd()
        {
            InitializeComponent();
            var vm = new OrderAddMVVM();
            vm.SetClose(() => this.Close());
            this.DataContext = vm;
        }
    }
}
