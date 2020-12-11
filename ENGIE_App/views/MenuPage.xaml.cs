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
            App.NavigationPage.Navigation.PopToRootAsync();
            App.MenuIsPresented = false;
        }

        void GoHelp(object sender, EventArgs args)
        {
            App.NavigationPage.Navigation.PushModalAsync(new HelpPage());
            App.MenuIsPresented = false;
        }

        void GoForm(object sender, EventArgs args)
        {
            App.NavigationPage.Navigation.PushModalAsync(new FormPage());
            App.MenuIsPresented = false;
        }
        void GoRecentlySubmitted(object sender, EventArgs args)
        {
            App.NavigationPage.Navigation.PushModalAsync(new RecentlySubmitted());
            App.MenuIsPresented = false;
        }
        void GoScan(object sender, EventArgs args)
        {
            App.NavigationPage.Navigation.PushModalAsync(new ScanPage());
            App.MenuIsPresented = false;
        }
        void GoLogin(object sender, EventArgs args)
        {
            App.NavigationPage.Navigation.PushModalAsync(new LoginPage());
            App.MenuIsPresented = false;
        }
    }
}