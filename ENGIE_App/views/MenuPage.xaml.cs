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

        /// <summary>
        /// Method checks if the current user is admin
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Method shows the admin button in the menu if the user is admin
        /// </summary>
        public void UpdateAdminButton()
        {
            if (CheckIfAdmin())
            {
                AdminButton.IsVisible = true;
                AdminButton.IsEnabled = true;
            }
        }

        /// <summary>
        /// Sends the user to the root of the app (Main for users, AdminOptions for admin)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void GoHome(object sender, EventArgs args)
        {
            App.MenuIsPresented = false;
            App.NavigationPage.Navigation.PopToRootAsync();
        }

        /// <summary>
        /// Sends the user to the help page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void GoHelp(object sender, EventArgs args)
        {
            App.MenuIsPresented = false;
            App.NavigationPage.Navigation.PushAsync(new HelpPage());
        }

        /// <summary>
        /// Sends the user to the recently submitted form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void GoRecentlySubmitted(object sender, EventArgs args)
        {
            App.MenuIsPresented = false;
            App.NavigationPage.Navigation.PushAsync(new RecentlySubmitted());
        }

        /// <summary>
        /// Sends the user to the QR scanner
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void GoScan(object sender, EventArgs args)
        {
            App.MenuIsPresented = false;
            App.NavigationPage.Navigation.PushAsync(new ScanPage());
        }

        /// <summary>
        /// Sends the user to the admin options page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void GoAdmin(object sender, EventArgs args)
        {
            App.MenuIsPresented = false;
            App.NavigationPage.Navigation.PushAsync(new AdminOptionsPage());
        }

        /// <summary>
        /// Sends the user to the login page and clears the users details from the session variables.
        /// Also sets the app root to the login page to prevent returning back to the main page after logout.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
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