using ENGIE_App.views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ENGIE_App
{
    public partial class App : Application
    {
        public static NavigationPage NavigationPage { get; private set; }
        private static RootPage rootPage;
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
            var menuPage = new MenuPage();
            NavigationPage = new NavigationPage(new HelpPage());
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
            MenuIsPresented = true;
        }
    }
}
