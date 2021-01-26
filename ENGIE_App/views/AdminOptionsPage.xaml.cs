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
        SshClient client;
        String bnumber = "bunumb";
        String unipass = "unipass";
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
            var newsalt = "hvGirlXDVzdsCSrPmOdHRA==";
            var hashedPassword = ComputeHash(Encoding.UTF8.GetBytes(password),
                Encoding.UTF8.GetBytes(newsalt));

            if (connection == null)
            {
                Connect_Databse();
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

            connection.Clone();
            connection.Close();
            client.Disconnect();

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

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Team 40", "csc2033team40@gmail.com"));
            message.To.Add(new MailboxAddress("", (string)Application.Current.Properties["Email"]));
            message.Subject = "TEST";

            // create our message text, just like before (except don't set it as the message.Body)
            var body = new TextPart("plain")
            {
                Text = "Hi there, this is a body of the message."
            };

            // now create the multipart/mixed container to hold the message text and the
            // image attachment


            message.Body = body;

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
    }
}   