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

        bool CheckIfAdmin()
        {
            if (Application.Current.Properties.ContainsKey("Admin"))
            {
                return true;
            } else
            {
                return false;
            }
        }

        public void UpdateAdminButton()
        {
            if (CheckIfAdmin())
            {
                AdminButton.IsVisible = true;
                AdminButton.IsEnabled = true;
            }
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
        void GoAdmin(object sender, EventArgs args)
        {
            App.MenuIsPresented = false;
            App.NavigationPage.Navigation.PushAsync(new AdminOptionsPage());
        }
        void GoLogin(object sender, EventArgs args)
        {
            Application.Current.Properties.Remove("Firstname");
            Application.Current.Properties.Remove("Lastname");
            Application.Current.Properties.Remove("Email");
            Application.Current.Properties.Remove("Phone");
            if (CheckIfAdmin())
            {
                Application.Current.Properties.Remove("Admin");
            }
            

            var mainPage = new LoginPage();
            var homePage = App.NavigationPage.Navigation.NavigationStack.First();
            App.NavigationPage.Navigation.InsertPageBefore(mainPage, homePage);
            App.MenuIsPresented = false;
            App.NavigationPage.PopToRootAsync(false);
        }
    }
}