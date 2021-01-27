using MySqlConnector;
using Renci.SshNet;
using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using System.Net.Mail;
using MailKit;
using MimeKit;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;

namespace ENGIE_App.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminOptionsPage : ContentPage
    {
        MySqlConnection connection;
        DatabaseConnector dbconn = new DatabaseConnector();
        bool checker = false;
        EmailHelper eHelper = new EmailHelper();

        bool setButtonEnable = false;
        bool sendButtonEnable = false;
        bool qrButtonEnable = false;

        public AdminOptionsPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();
            FakeDisable(setEmailBtn);
            FakeDisable(sendEmailBtn);
            FakeDisable(QRButton);
        }

        public string ComputeHash(byte[] bytesToHash, byte[] salt)
        {
            var byteResult = new Rfc2898DeriveBytes(bytesToHash, salt, 10000);
            return Convert.ToBase64String(byteResult.GetBytes(24));
        }

        private void FakeDisable(Button btn)
        {
            btn.TextColor = Color.FromHex("#444444");
            
        }
        private void FakeEnable(Button btn)
        {
            btn.TextColor = Color.FromHex("#FFF");
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var username = EntryUsername.Text;
            var password = EntryPassword.Text;
            var newsalt = "hvGirlXDVzdsCSrPmOdHRA==";
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

                // SQL code
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "INSERT INTO Admin(Username, Password) VALUES (@Username, @Password)";

                // adds values user inputted to database
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", hashedPassword);
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            dbconn.Close_Connection();

            Console.WriteLine("Done.");

            await this.DisplayAlert("Congratulations", "Admin Succesfully Created", "Continue", "Cancel");

        }

        private void Set_Email(object sender, EventArgs e)
        {
            if (setButtonEnable)
            {
                eHelper.SetDes(EntryDesEmail.Text);

                Application.Current.Properties["Email"] = eHelper.GetDes();

                checker = true;
                sendButtonEnable = checker;
                qrButtonEnable = true;
                FakeEnable(sendEmailBtn);
                FakeEnable(QRButton);
            }
            else
            {
                DisplayAlert("Alert", "Please enter a valid email", "OK");
            }   
        }

        public async void Send_Email(object sender, EventArgs e)
        {
            if (sendButtonEnable)
            {
                eHelper.SendEmail("Test", "This is bodyText");
                await this.DisplayAlert("Succes", "email sent", "Continue", "Cancel");
            }
            else
            {
                DisplayAlert("Alert", "Please enter a valid email", "OK");
            }
        }

        private void ValidateEmail(object sender, TextChangedEventArgs e)
        {
            setButtonEnable = Regex.IsMatch(EntryDesEmail.Text, @"^((([a-z, A-Z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z, A-Z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z, A-Z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z, A-Z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z, A-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z, A-Z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z, A-Z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z, A-Z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z, A-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z, A-Z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$");
            if (setButtonEnable) { FakeEnable(setEmailBtn); }
        }

        private void GenerateQR(object sender, EventArgs e)
        {
            if (qrButtonEnable)
            {
                var text = SetSelectedItem();
                if (text != null)
                {
                    if (eHelper.GetDes() != null)
                    {
                        var generator = new QRBuilder();
                        var QRData = generator.CreateQRCode(text);

                        QRLabel.Text = "Generated Successfully";
                        //await Clipboard.SetTextAsync(QRData);
                        Console.WriteLine(QRData);
                        generator.SaveImage(generator.CreateImageFromText(QRData), eHelper.GetDes());
                        //var test = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                        //var filename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "MyQR.png");
                        //var filename = Path.Combine(test, "MyQR.png");
                    }
                }
                else
                {
                    QRLabel.Text = "No Item Selected";
                }
            }
            else
            {
                DisplayAlert("Alert", "Please set a valid email", "OK");
            }

        }

        private string SetSelectedItem()
        {
            switch (EntryQRText.SelectedIndex)
            {
                case 0:
                    return "EML";
                case 1:
                    return "ELT";
                default:
                    return null;
            }
        }
    }
}   