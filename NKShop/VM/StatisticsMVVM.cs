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
    internal class StatisticsMVVM : BaseVM
    {

        private ObservableCollection<Statistic> statistics = new();

        public ObservableCollection<Statistic> Statistics
        {
            get => statistics;
            set
            {
                statistics = value;
                Signal();
            }
        }

        private string quanityprice;

        public string QuanityPrice
        {
            get => quanityprice;
            set
            {
                quanityprice = value;
                Signal();
            }
        }

        public CommandMvvm StatSelectAll { get; set; }
        public CommandMvvm StatSelectReady { get; set; }
        public CommandMvvm StatSelectNotReady { get; set; }


        public StatisticsMVVM()
        {
            QuanityPrice = "Сколько потенциально принесли заказы(р)";
            SelectAll();

            StatSelectAll = new CommandMvvm(() =>
            {
                SelectAll();
                QuanityPrice = "Сколько потенциально принесли заказы(р)";
            }, () => true);

            StatSelectReady = new CommandMvvm(() =>
            {
                SelectReady();
                QuanityPrice = "Сколько принесли заказы(р)";
            }, () => true);

            StatSelectNotReady = new CommandMvvm(() =>
            {
                SelectNotReady();
                QuanityPrice = "Сколько должны принести заказы(р)";
            }, () => true);


        }



        private void SelectAll()
        {
            Statistics = new ObservableCollection<Statistic>(StatisticDB.GetDb().SelectStatisticAll());
        }

        private void SelectReady()
        {
            Statistics = new ObservableCollection<Statistic>(StatisticDB.GetDb().SelectStatisticReady());
        }

        private void SelectNotReady()
        {
            Statistics = new ObservableCollection<Statistic>(StatisticDB.GetDb().SelectStatisticNotReady());
        }

    }
}
