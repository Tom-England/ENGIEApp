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
        /// <summary>
        /// Initializer class for the page, hides original menu options.
        /// </summary>
        public ScanPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();
        }
        // Boolean to pause/resume scanning
        private bool isScanning = true;

        /// <summary>
        /// Method for handling the QR scanner
        /// </summary>
        /// <param name="result"></param>
        void ZXingScannerView_OnScanResult(ZXing.Result result)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                
                scanResultText.Text = result.Text;

                if (result.Text == "ELT" && isScanning)
                {
                    // Stop app scanning once code is identified
                    isScanning = false;
                    // Load relevant page for code ELT
                    var page = new RCDPage();
                    // Respond to trigger when page is popped off navigation stack
                    page.DidFinishPopping += (parameter) =>
                    {
                        // Reload app to main page upon successful form submission
                        App.NavigationPage.Navigation.PushAsync(new MainPage());
                    };
                    Navigation.PushAsync(page);
                }
            });
               
        }
    }
}