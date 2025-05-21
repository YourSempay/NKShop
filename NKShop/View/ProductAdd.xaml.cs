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
    /// Логика взаимодействия для ProductAdd.xaml
    /// </summary>
    public partial class ProductAdd : Window
    {
        public ProductAdd(Product SelectedProduct)
        {
            InitializeComponent();
            ((ProductAddMVVM)this.DataContext).SetClose(Close);
            ((ProductAddMVVM)this.DataContext).SelProd(SelectedProduct);
        }
    }
}
