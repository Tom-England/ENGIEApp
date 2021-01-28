using ENGIE_App.views;
using System;
using Xamarin.Forms;

namespace ENGIE_App
{
    public partial class App : Application
    {
        public static NavigationPage NavigationPage { get; private set; }
        private static RootPage rootPage;
        private static MenuPage menuPage;
        public static bool MenuIsPresented
        {
            get
            {
                return rootPage.IsPresented;
            }
            set
            {
                rootPage.IsPresented = value;
            }
        }
        public App()
        {
            InitializeComponent();
            // Set Up For Nav Bar
            menuPage = new MenuPage();
            NavigationPage = new NavigationPage(new LoginPage());
            rootPage = new RootPage();
            rootPage.Master = menuPage;
            rootPage.Detail = NavigationPage;
            MainPage = rootPage;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        private void Expand(object sender, EventArgs e)
        {
            menuPage.UpdateAdminButton();
            MenuIsPresented = true;
        }
    }
}
