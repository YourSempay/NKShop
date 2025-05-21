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
    public partial class OrderEditWindow : Window
    {
        public OrderEditWindow(Order SelectedOrderRed)
        {
            InitializeComponent();
            ((OrderEditViewModel)this.DataContext).SetClose(Close);
            ((OrderEditViewModel)this.DataContext).SelOrder(SelectedOrderRed);
        }
    }
}
