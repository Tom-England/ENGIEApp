using Xamarin.Forms;

namespace ENGIE_App
{
    public partial class MainPage : ContentPage
    {
        /// <summary>
        /// Initializer for the main page, sets the welcome tag to inclue the users name.
        /// </summary>
        public MainPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();
            if (Application.Current.Properties.ContainsKey("Firstname")) {
                WecomeLabel.Text = "Welcome, " + Application.Current.Properties["Firstname"];
            }
            
        }
    }
}
