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
    public partial class ScanPage : ContentPage
    {
        public ScanPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();
        }



        void ZXingScannerView_OnScanResult(ZXing.Result result)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                scanResultText.Text = result.Text;

                if (result.Text == "ELT")
                {
                    //Navigation.PushAsync(new RCDPage());

                    var page = new RCDPage();
                    page.DidFinishPopping += (parameter) =>
                    {
                        App.NavigationPage.Navigation.PushAsync(new MainPage());
                    };
                    Navigation.PushAsync(page);
                }
            });
               
        }
    }
}