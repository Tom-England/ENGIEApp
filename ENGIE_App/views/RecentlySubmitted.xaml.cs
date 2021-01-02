using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ENGIE_App.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecentlySubmitted : ContentPage
    {

        public ObservableCollection<MyItem> MyItems { get; set; }
        public RecentlySubmitted()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();

            MyItems = new ObservableCollection<MyItem>();


            addItemToTable(new MyItem { Date = "17/12/2020", Time = "16:32:12", Form = "Form B", Sent = true });
        }

        private void addItemToTable(MyItem item)
        {
            MyItems.Add(item);
            listViewm.ItemsSource = MyItems;
        }
    }

    public class MyItem
    {
        public string Date { get; set; }
        public string Time { get; set; }
        public string Form { get; set; }
        public bool Sent { get; set; }   
    }

}