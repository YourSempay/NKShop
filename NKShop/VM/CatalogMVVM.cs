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
    internal class CatalogMVVM : BaseVM
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
        private string searchCatalog;

        public string SearchCatalog
        {

            get => searchCatalog;
            set
            {
                searchCatalog = value;
                SearchHis(searchCatalog);
            }
        }

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
        public CatalogMVVM()
        {
            SelectAll();


        }
        private void SearchHis(string search)
        {

            Products = new ObservableCollection<Product>(SearchListProducts.GetDB().SearchOrder(search));
        }

        private void SelectAll()
        {
            Products = new ObservableCollection<Product>(ProductDB.GetDb().SelectAll());
        }
    }
}
