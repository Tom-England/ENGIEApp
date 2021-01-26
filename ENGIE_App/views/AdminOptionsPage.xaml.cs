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

        public AdminOptionsPage()
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
            var email = EntryDesEmail.Text;

            Application.Current.Properties["Email"] = email;

            checker = true;
            sendEmailBtn.IsEnabled = checker;
        }

        public async void Send_Email(object sender, EventArgs e)
        {

            await Permissions.RequestAsync<Permissions.StorageRead>();

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Team 40", "csc2033team40@gmail.com"));
            message.To.Add(new MailboxAddress("", (string)Application.Current.Properties["Email"]));
            message.Subject = "TEST";

            var builder = new BodyBuilder();

            // Set the plain-text version of the message text
            builder.TextBody = @"Hi there, this is a body of the message.";

          //  builder.Attachments.Add(@"/storage/emulated/0/Download/Emergency_Lighting_Full_Sheet.pdf");   <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<  UNCOMMENT THIS!!! path needs changing to where our PDFs will be

            // Now we just need to set the message body and we're done
            message.Body = builder.ToMessageBody();

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate("csc2033team40", "!Passw0rd123");

                try
                {
                    await client.SendAsync(message);
                    await this.DisplayAlert("Succes", "email sent", "Continue", "Cancel");
                }
                catch (SmtpCommandException ex)
                {
                    Console.WriteLine("Error sending message: {0}", ex.Message);
                    Console.WriteLine("\tStatusCode: {0}", ex.StatusCode);
                    switch (ex.ErrorCode)
                    {
                        case SmtpErrorCode.RecipientNotAccepted:
                            Console.WriteLine("\tRecipient not accepted: {0}", ex.Mailbox);
                            break;
                        case SmtpErrorCode.SenderNotAccepted:
                            Console.WriteLine("\tSender not accepted: {0}", ex.Mailbox);
                            break;
                        case SmtpErrorCode.MessageNotAccepted:
                            Console.WriteLine("\tMessage not accepted.");
                            break;
                    }
                }
                catch (SmtpProtocolException ex)
                {
                    Console.WriteLine("Protocol error while sending message: {0}", ex.Message);
                }
                client.Disconnect(true);
            }
        }

        private void ValidateEmail(object sender, TextChangedEventArgs e)
        {
            setEmailBtn.IsEnabled = Regex.IsMatch(EntryDesEmail.Text, @"^((([a-z, A-Z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z, A-Z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z, A-Z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z, A-Z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z, A-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z, A-Z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z, A-Z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z, A-Z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z, A-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z, A-Z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$");
        }

        private void GenerateQR(object sender, EventArgs e)
        {


            var text = SetSelectedItem();
            if (text != null)
            {
                var generator = new QRBuilder();
                var QRData = generator.CreateQRCode(text);

                QRLabel.Text = "Generated Successfully";
                //await Clipboard.SetTextAsync(QRData);
                Console.WriteLine(QRData);
                generator.SaveImage(generator.CreateImageFromText(QRData));
                var test = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                //var filename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "MyQR.png");
                var filename = Path.Combine(test, "MyQR.png");
            }
            else
            {
                QRLabel.Text = "No Item Selected";
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