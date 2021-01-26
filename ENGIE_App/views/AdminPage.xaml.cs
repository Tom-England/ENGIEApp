using MySqlConnector;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
        DatabaseConnector dbconn = new DatabaseConnector();

        public AdminPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();
        }

        public string ComputeHash(byte[] bytesToHash, byte[] salt)
        {
            var byteResult = new Rfc2898DeriveBytes(bytesToHash, salt, 10000);
            return Convert.ToBase64String(byteResult.GetBytes(24));
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var username = EntryUsername.Text;
            var password = EntryPassword.Text;
            bool check = false;
            var newsalt = "hvGirlXDVzdsCSrPmOdHRA==";

            if (username == null && password == null)
            {
                var nullResult = await DisplayAlert("Fill In All Fields", "Must enter Username and Password. Please Try Again", "Continue", "Cancel");
                if (nullResult)
                {
                    await Navigation.PushAsync(new ENGIE_App.views.AdminPage());
                }
            }
            else if (username == null)
            {
                var nullResult = await DisplayAlert("Fill In All Fields", "Must enter Username. Please Try Again", "Continue", "Cancel");
                if (nullResult)
                {
                    await Navigation.PushAsync(new ENGIE_App.views.AdminPage());
                }
            }
            else if (password == null)
            {
                var nullResult = await DisplayAlert("Fill In All Fields", "Must enter Password. Please Try Again", "Continue", "Cancel");
                if (nullResult)
                {
                    await Navigation.PushAsync(new ENGIE_App.views.AdminPage());
                }
            }
            else
            {
                var hashedPassword = ComputeHash(Encoding.UTF8.GetBytes(password),
                    Encoding.UTF8.GetBytes(newsalt));

                if (connection == null)
                {
                    connection = dbconn.Connect_Database();
                    connection.Open();
                }
                try
                {

                    Console.WriteLine("Connecting to MySQL...");
                    Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA " + hashedPassword);

                    // SQL code
                    MySqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "SELECT Username, Password FROM Admin WHERE Username = @username and Password = @password";

                    // adds values user inputted to database
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", hashedPassword);
                    cmd.ExecuteNonQuery();
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        reader.Close();
                        check = true;
                    }
                    else
                    {
                        reader.Close();
                        check = false;
                    }

                    Application.Current.Properties["Firstname"] = username;

                    if (check)
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            // takes user to home page upon succesful login 
                            var result = await this.DisplayAlert("Congratulations", "User Succesfully logged on", "Continue", "Cancel");
                            Application.Current.Properties["Admin"] = true;
                            if (result)
                            {
                                var mainPage = new MainPage();
                                var homePage = App.NavigationPage.Navigation.NavigationStack.First();
                                App.NavigationPage.Navigation.InsertPageBefore(mainPage, homePage);
                                await Navigation.PushAsync(new ENGIE_App.views.AdminOptionsPage());
                                //await App.NavigationPage.PopToRootAsync(false);

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
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

                dbconn.Close_Connection();
            }
            
        }

    }
}