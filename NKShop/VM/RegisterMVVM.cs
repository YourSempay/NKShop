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

namespace NKShop.VM
{
    internal class RegisterMVVM : BaseVM
    {
        public CommandMvvm AddBook { get; set; }
        public CommandMvvm RemoveBook { get; set; }
        public CommandMvvm EditBook { get; set; }
        public CommandMvvm UpdateBook { get; set; }

        public RegisterMVVM()
        {
            SelectAll();

            EditBook = new CommandMvvm(() =>
            {
                new EditBooks(SelectedBook).ShowDialog();
                SelectAll();
            }, () => SelectedBook != null);

            RemoveBook = new CommandMvvm(() =>
            {
                var bookvozvrat = MessageBox.Show("Вы уверены что хотите удалить кигу?", "Подтверждение", MessageBoxButton.YesNo);

                if (bookvozvrat == MessageBoxResult.Yes)
                {
                    BookDB.GetDb().Remove(SelectedBook);
                }
                SelectAll();
            }, () => SelectedBook != null);

            AddBook = new CommandMvvm(() =>
            {
                new EditBooks(new Book()).ShowDialog();
                SelectAll();
            }, () => true);

            UpdateBook = new CommandMvvm(() =>
            {
                SelectAll();
            }, () => true);

        }

        private void SelectAll()
        {
            Books = new ObservableCollection<Book>(BookDB.GetDb().SelectAll());
        }
    }
}
