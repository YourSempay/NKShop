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
    internal class ProductAddMVVM : BaseVM
    {
        public CommandMvvm AddProduct { get; set; }

        private Product selectedproduct = new();

        public Product SelectedProduct
        {
            get => selectedproduct;
            set
            {
                selectedproduct = value;
                Signal();
            }
        }

        private string ready;

        public string Ready
        {
            get => ready;
            set
            {
                ready = value;
                Signal();
            }
        }


        public ProductAddMVVM()
        {
            AddProduct = new CommandMvvm(() =>
            {
                var orderreturn = MessageBox.Show("Вы уверены что хотите добавить новый товар?", "Подтверждение", MessageBoxButton.YesNo);

                if (orderreturn == MessageBoxResult.Yes)
                {
                    if (Ready == "Товар готов")
                    {
                        SelectedProduct.IsReadyProd = true;
                    }
                    else SelectedProduct.IsReadyProd = false;

                    Product newproduct = new Product();
                    newproduct.Title = SelectedProduct.Title;
                    newproduct.Price = SelectedProduct.Price;
                    newproduct.IsReadyProd = SelectedProduct.IsReadyProd;
                    newproduct.Quantity = SelectedProduct.Quantity;
                    ProductDB.GetDb().Insert(newproduct);
                    close?.Invoke();
                }

            }, () => true);

        }


        Action close;
        internal void SetClose(Action close)
        {
            this.close = close;
        }

        public void SelProd(Product SelProd)
        {
            SelectedProduct = SelProd;
        }

    }
}
