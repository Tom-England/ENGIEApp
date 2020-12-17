using ENGIE_App.views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ENGIE_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : ContentPage
    {

        public MenuPage()
        {
            Title = "Menu";
            InitializeComponent();
        }

        void GoHome(object sender, EventArgs args)
        {
            App.MenuIsPresented = false;
            App.NavigationPage.Navigation.PopToRootAsync();
        }

        void GoHelp(object sender, EventArgs args)
        {
            App.MenuIsPresented = false;
            App.NavigationPage.Navigation.PushAsync(new HelpPage());
        }

        void GoForm(object sender, EventArgs args)
        {
            App.MenuIsPresented = false;
            App.NavigationPage.Navigation.PushAsync(new FormPage());
        }
        void GoRecentlySubmitted(object sender, EventArgs args)
        {
            App.MenuIsPresented = false;
            App.NavigationPage.Navigation.PushAsync(new RecentlySubmitted());
        }
        void GoScan(object sender, EventArgs args)
        {
            App.MenuIsPresented = false;
            App.NavigationPage.Navigation.PushAsync(new ScanPage());
        }
        void GoLogin(object sender, EventArgs args)
        {
            var mainPage = new LoginPage();
            var homePage = App.NavigationPage.Navigation.NavigationStack.First();
            App.NavigationPage.Navigation.InsertPageBefore(mainPage, homePage);
            App.MenuIsPresented = false;
            App.NavigationPage.PopToRootAsync(false);
        }
    }
}