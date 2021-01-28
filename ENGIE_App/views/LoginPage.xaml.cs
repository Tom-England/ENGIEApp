using ENGIE_App.Tables;
using MySqlConnector;
using SQLite;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ENGIE_App.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        MySqlConnection connection;
        DatabaseConnector dbconn = new DatabaseConnector();

        public LoginPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();


            if (Application.Current.Properties.ContainsKey("Firstname"))
            {
                EntryFirstName.Text = (string)Application.Current.Properties["Firstname"];
            }
            if (Application.Current.Properties.ContainsKey("Lastname"))
            {
                EntryLastName.Text = (string)Application.Current.Properties["Lastname"];
            }
            if (Application.Current.Properties.ContainsKey("Email"))
            {
                EntryUserEmail.Text = (string)Application.Current.Properties["Email"];
            }
            if (Application.Current.Properties.ContainsKey("Phone"))
            {
                EntryUserPhoneNumber.Text = (string)Application.Current.Properties["Phone"];
            }
        }



        public async void Button_Clicked(object sender, System.EventArgs e)
        {
            LoginPage dbConnect = new LoginPage();
            DateTime dtNow = DateTime.Now;
            var firstName = EntryFirstName.Text;
            var lastName = EntryLastName.Text;
            var email = EntryUserEmail.Text;
            var phone = EntryUserPhoneNumber.Text;

            // regular expresions to validate user input
            var emailPattern = @"^((([a-z, A-Z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z, A-Z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z, A-Z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z, A-Z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z, A-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z, A-Z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z, A-Z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z, A-Z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z, A-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z, A-Z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$";
            var phonePattern = @"^(\+[0-9]{12})|(0[0-9]{10})$";

            // local database
            var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDatabase.db");
            var db = new SQLiteConnection(dbpath);
            db.CreateTable<LogUserData>();

            // adds values to local database
            var item = new LogUserData()
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PhoneNumber = phone
            };

            db.Insert(item);
            // validation (probably a better way to do this, ideally should be done on client side but i couldn figure it out)
            if (firstName == null)
            {
                var nullResult = await DisplayAlert("Fill In All Fields", "Must enter first name. Please Try Again", "Continue", "Cancel");
                if (nullResult)
                {
                    Application.Current.Properties["Firstname"] = item.FirstName;
                    Application.Current.Properties["Lastname"] = item.LastName;
                    Application.Current.Properties["Email"] = item.Email;
                    Application.Current.Properties["Phone"] = item.PhoneNumber;
                    await Navigation.PushAsync(new ENGIE_App.views.LoginPage());
                }
            }
            else if (lastName == null)
            {
                var nullResult = await DisplayAlert("Fill In All Fields", "Must enter last name. Please Try Again", "Continue", "Cancel");
                if (nullResult)
                {
                    Application.Current.Properties["Firstname"] = item.FirstName;
                    Application.Current.Properties["Lastname"] = item.LastName;
                    Application.Current.Properties["Email"] = item.Email;
                    Application.Current.Properties["Phone"] = item.PhoneNumber;
                    await Navigation.PushAsync(new ENGIE_App.views.LoginPage());
                }
            }
            else if (email == null)
            {
                var nullResult = await DisplayAlert("Fill In All Fields", "Must enter email. Please Try Again", "Continue", "Cancel");
                if (nullResult)
                {
                    Application.Current.Properties["Firstname"] = item.FirstName;
                    Application.Current.Properties["Lastname"] = item.LastName;
                    Application.Current.Properties["Email"] = item.Email;
                    Application.Current.Properties["Phone"] = item.PhoneNumber;
                    await Navigation.PushAsync(new ENGIE_App.views.LoginPage());
                }
            }
            else if (phone == null)
            {
                var nullResult = await DisplayAlert("Fill In All Fields", "Must Enter Mobile number. Please Try Again", "Continue", "Cancel");
                if (nullResult)
                {
                    Application.Current.Properties["Firstname"] = item.FirstName;
                    Application.Current.Properties["Lastname"] = item.LastName;
                    Application.Current.Properties["Email"] = item.Email;
                    Application.Current.Properties["Phone"] = item.PhoneNumber;
                    await Navigation.PushAsync(new ENGIE_App.views.LoginPage());
                }
            }
            else if (!(Regex.IsMatch(email, emailPattern)))
            {

                var emailResult = await DisplayAlert("Invalid Email Entered", "Must be in the format of '<email name>@<domain name>.<email host>'. Please Try Again", "Continue", "Cancel");
                if (emailResult)
                {
                    Application.Current.Properties["Firstname"] = item.FirstName;
                    Application.Current.Properties["Lastname"] = item.LastName;
                    Application.Current.Properties["Email"] = item.Email;
                    Application.Current.Properties["Phone"] = item.PhoneNumber;
                    await Navigation.PushAsync(new ENGIE_App.views.LoginPage());
                }
            }
            else if (!(Regex.IsMatch(phone, phonePattern)))
            {
                var phoneResult = await DisplayAlert("Invalid Phone Number Entered", "Must be in the format of '+XXXXXXXXXXXX' or '0XXXXXXXXXX'. Please Try Again", "Continue", "Cancel");
                if (phoneResult)
                {
                    Application.Current.Properties["Firstname"] = item.FirstName;
                    Application.Current.Properties["Lastname"] = item.LastName;
                    Application.Current.Properties["Email"] = item.Email;
                    Application.Current.Properties["Phone"] = item.PhoneNumber;
                    await Navigation.PushAsync(new ENGIE_App.views.LoginPage());
                }
            }
            else
            {
                if (connection == null)
                {
                    connection = dbconn.Connect_Database();
                    connection.Open();
                }
                try
                {
                    Console.WriteLine("Connecting to MySQL...");

                    // SQL code
                    MySqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "INSERT INTO LogUserData(FirstName, LastName, Email, PhoneNumber, DateTime) VALUES (@FirstName, @LastName, @Email, @PhoneNumber, @TimeStamp)";

                    // adds values user inputted to database
                    cmd.Parameters.AddWithValue("@FirstName", firstName);
                    cmd.Parameters.AddWithValue("@LastName", lastName);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@PhoneNumber", phone);
                    cmd.Parameters.AddWithValue("@TimeStamp", dtNow);
                    cmd.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

                dbconn.Close_Connection();

                Console.WriteLine("Done.");

                Application.Current.Properties["Firstname"] = item.FirstName;
                Application.Current.Properties["Lastname"] = item.LastName;

                Device.BeginInvokeOnMainThread(async () =>
                {
                    // takes user to home page upon succesful login 
                    var result = await this.DisplayAlert("Congratulations", "User Succesfully Registered", "Continue", "Cancel");
                    if (result)
                    {
                        (Application.Current.MainPage as RootPage).enableGesture();
                        var mainPage = new MainPage();
                        var homePage = App.NavigationPage.Navigation.NavigationStack.First();
                        App.NavigationPage.Navigation.InsertPageBefore(mainPage, homePage);
                        await App.NavigationPage.PopToRootAsync(false);
                    }
                }
                );
            }


        }

        private async void Admin_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ENGIE_App.views.AdminPage());
        }
    }
}