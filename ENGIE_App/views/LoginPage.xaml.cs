using ENGIE_App.Tables;
using MySqlConnector;
using Renci.SshNet;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ENGIE_App.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();
        }

        public async void Button_Clicked(object sender, System.EventArgs e)
        {
            var email = EntryUserEmail.Text;
            var phone = EntryUserPhoneNumber.Text;
            var emailPattern = @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$";
            var phonePattern = @"^(\+[0-9]{12})$";


            if (!String.IsNullOrWhiteSpace(email) && !(Regex.IsMatch(email, emailPattern)))
            {
                // LabelError.Text = "Invalid Email Entered, Please Try Again";
                var emailResult = await DisplayAlert("Invalid Email Entered", "Must be in the format of '<email name>@<domain name>.<email host>'. Please Try Again", "Continue", "Cancel");
                if (emailResult)
                {
                    await Navigation.PushAsync(new ENGIE_App.views.LoginPage());
                }
            }
            else if (!String.IsNullOrWhiteSpace(phone) && !(Regex.IsMatch(phone, phonePattern)))
            {
                var emailResult = await DisplayAlert("Invalid Phone Number Entered", "Must be in the format of '+XXXXXXXXXXXX'. Please Try Again", "Continue", "Cancel");
                if (emailResult)
                {
                    await Navigation.PushAsync(new ENGIE_App.views.LoginPage());
                }
            }
            else
            {
                LabelError.Text = "";
                PasswordConnectionInfo connectionInfo = new PasswordConnectionInfo("linux.cs.ncl.ac.uk", "bnumber", "unipass");
                connectionInfo.Timeout = TimeSpan.FromSeconds(30);
                var client = new SshClient(connectionInfo);
                client.Connect();
                var x = client.IsConnected;
                ForwardedPortLocal portFwld = new ForwardedPortLocal("127.0.0.1", "cs-db.ncl.ac.uk", 3306);
                client.AddForwardedPort(portFwld);
                portFwld.Start();
                var connection = new MySqlConnection("server = 127.0.0.1; Database = t2033t40; UID = t2033t40; PWD = AwedPace%Car; Port = " + portFwld.BoundPort);

                try
                {
                    Console.WriteLine("Connecting to MySQL...");
                    connection.Open();

                    // string sql = "INSERT INTO LogUserData(FirstName, LastName, Email, PhoneNumber) VALUES ('This', 'works', 'just', 'barely')";


                    MySqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "INSERT INTO LogUserData(FirstName, LastName, Email, PhoneNumber) VALUES (@FirstName, @LastName, @Email, @PhoneNumber)";
                    cmd.Parameters.AddWithValue("@FirstName", EntryFirstName.Text);
                    cmd.Parameters.AddWithValue("@LastName", EntryLastName.Text);
                    cmd.Parameters.AddWithValue("@Email", EntryUserEmail.Text);
                    cmd.Parameters.AddWithValue("@PhoneNumber", EntryUserPhoneNumber.Text);
                    cmd.ExecuteNonQuery();


                    // MySqlScript script = new MySqlScript(connection, sql);

                    // script.Error += new MySqlScriptErrorEventHandler(script_Error);
                    // script.ScriptCompleted += new EventHandler(script_ScriptCompleted);
                    // script.StatementExecuted += new MySqlStatementExecutedEventHandler(script_StatementExecuted);

                    // int count = script.Execute();

                    // Console.WriteLine("Executed " + count + " statement(s).");
                    // Console.WriteLine("Delimiter: " + script.Delimiter);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

                connection.Clone();
                connection.Close();
                client.Disconnect();

                Console.WriteLine("Done.");



                var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDatabase.db");
                var db = new SQLiteConnection(dbpath);
                db.CreateTable<LogUserData>();

                var item = new LogUserData()
                {
                    FirstName = EntryFirstName.Text,
                    LastName = EntryLastName.Text,
                    Email = EntryUserEmail.Text,
                    PhoneNumber = EntryUserPhoneNumber.Text
                };

                Application.Current.Properties["Firstname"] = item.FirstName;

                db.Insert(item);
                Device.BeginInvokeOnMainThread(async () =>
                {
                    var result = await this.DisplayAlert("Congratulations", "User Succesfully Registered", "Continue", "Cancel");
                    if (result)
                    {
                        var mainPage = new MainPage();
                        var homePage = App.NavigationPage.Navigation.NavigationStack.First();
                        App.NavigationPage.Navigation.InsertPageBefore(mainPage, homePage);
                        await App.NavigationPage.PopToRootAsync(false);
                        //await App.NavigationPage.Navigation.PushAsync(new MainPage());
                    }
                }
                );
            }


        }
    }
}