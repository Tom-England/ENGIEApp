using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ENGIE_App.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminOptionsPage : ContentPage
    {
        public AdminOptionsPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();
        }
    }
}