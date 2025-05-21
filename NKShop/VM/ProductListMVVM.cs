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
    internal class ProductListMVVM : BaseVM
    {
        private Product selectedproduct;

        public Product SelectedProduct
        {
            get => selectedproduct;
            set
            {
                selectedproduct = value;
                Signal();
            }
        }
        public CommandMvvm ProductEditt { get; set; }
        public CommandMvvm NewProduct { get; set; }
        public CommandMvvm DeleteProduct { get; set; }


        private ObservableCollection<Product> products = new();
        public ObservableCollection<Product> Products
        {
            get => products;
            set
            {
                products = value;
                Signal();
            }
        }

        public ProductListMVVM()
        {

            SelectAll();

            ProductEditt = new CommandMvvm(() =>
            {
                ProductsEdit pe = new ProductsEdit(SelectedProduct);
                pe.ShowDialog();
                SelectAll();

            }, () => SelectedProduct != null);

            NewProduct = new CommandMvvm(() =>
            {
                ProductAdd pa = new ProductAdd(new Product());
                pa.ShowDialog();
                SelectAll();

            }, () => true);

            DeleteProduct = new CommandMvvm(() =>
            {
                var couriervozvrat = MessageBox.Show("Вы уверены что хотите удалить курьера?", "Подтверждение", MessageBoxButton.YesNo);

                if (couriervozvrat == MessageBoxResult.Yes)
                {
                    ProductDB.GetDb().Remove(SelectedProduct);
                    SelectAll();
                }

                

            }, () => SelectedProduct != null);


        }
        private void SelectAll()
        {
            Products = new ObservableCollection<Product>(ProductDB.GetDb().SelectAll());
        }


    }
}
