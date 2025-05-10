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
    internal class ProductEditMVVM : BaseVM
    {
        public CommandMvvm EditInfoProduct { get; set; }

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

        public ProductEditMVVM() 
        {

            

            EditInfoProduct = new CommandMvvm(() =>
            {
                var orderreturn = MessageBox.Show("Вы уверены что хотите изменить товар?", "Подтверждение", MessageBoxButton.YesNo);

                if (orderreturn == MessageBoxResult.Yes)
                {
                    if (Ready == "Товар готов")
                    {
                        SelectedProduct.IsReadyProd = true;
                    } else SelectedProduct.IsReadyProd = false;

                    ProductDB.GetDb().Update(SelectedProduct);
                    close?.Invoke();

                }

            }, () =>
            Ready != null &&
            SelectedProduct.Quantity >= 0
               );
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
