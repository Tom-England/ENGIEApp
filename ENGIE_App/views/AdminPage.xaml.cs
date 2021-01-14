using MySqlConnector;
using Renci.SshNet;
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
    public partial class AdminPage : ContentPage
    {
        MySqlConnection connection;
        SshClient client;
        String bnumber = "bnumber";
        String unipass = "unipass";

        public AdminPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();
        }

        public void Connect_Databse()
        {
            PasswordConnectionInfo connectionInfo = new PasswordConnectionInfo("linux.cs.ncl.ac.uk", bnumber, unipass);
            connectionInfo.Timeout = TimeSpan.FromSeconds(30);
            client = new SshClient(connectionInfo);
            client.Connect();
            var x = client.IsConnected;
            ForwardedPortLocal portFwld = new ForwardedPortLocal("127.0.0.1", "cs-db.ncl.ac.uk", 3306);
            client.AddForwardedPort(portFwld);
            portFwld.Start();
            connection = new MySqlConnection("server = 127.0.0.1; Database = t2033t40; UID = t2033t40; PWD = AwedPace%Car; Port = " + portFwld.BoundPort);
            connection.Open();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var username = EntryUsername.Text;
            var password = EntryPassword.Text;

            if (username.Equals("admin") && password.Equals("admin")) {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    // takes user to home page upon succesful login 
                    var result = await this.DisplayAlert("Congratulations", "User Succesfully Registered", "Continue", "Cancel");
                    Application.Current.Properties["Admin"] = true;
                    if (result)
                    {
                        var mainPage = new MainPage();
                        var homePage = App.NavigationPage.Navigation.NavigationStack.First();
                        App.NavigationPage.Navigation.InsertPageBefore(mainPage, homePage);
                        await App.NavigationPage.PopToRootAsync(false);
                    }
                }
                );
            }
            else
            {
                await DisplayAlert("Access Denied", "username and/or password are incorrect", "Continue", "Cancel");
                await Navigation.PushAsync(new ENGIE_App.views.AdminPage());
            }

        }

    }
}