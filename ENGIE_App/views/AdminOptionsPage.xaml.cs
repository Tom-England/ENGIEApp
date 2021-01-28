using MySqlConnector;
using System;
using System.Security.Cryptography;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
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

        /// <summary>
        /// Variables for tracking the psuedo-enabled state of the buttons
        /// </summary>
        bool setButtonEnable = false;
        bool sendButtonEnable = true;
        bool qrButtonEnable = true;

        /// <summary>
        /// Initialiser method for the page, hides original navigation functionality and disables buttons as needed
        /// </summary>
        public AdminOptionsPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();
            FakeDisable(setEmailBtn);
            //FakeDisable(sendEmailBtn);
            //FakeDisable(QRButton);
        }

        /// <summary>
        /// Computes a hashed password given an array of bytes and a salt
        /// </summary>
        /// <param name="bytesToHash"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public string ComputeHash(byte[] bytesToHash, byte[] salt)
        {
            var byteResult = new Rfc2898DeriveBytes(bytesToHash, salt, 10000);
            return Convert.ToBase64String(byteResult.GetBytes(24));
        }

        /// <summary>
        /// Method sets the font colour of the provided button object to grey to immitate the isEnabled=false appearence
        /// </summary>
        /// <param name="btn"></param>
        private void FakeDisable(Button btn)
        {
            btn.TextColor = Color.FromHex("#444444");
            
        }

        /// <summary>
        /// Sets the font colour back to white to undo FakeDisable
        /// </summary>
        /// <param name="btn"></param>
        private void FakeEnable(Button btn)
        {
            btn.TextColor = Color.FromHex("#FFF");
        }

        /// <summary>
        /// Function takes data from the XAML page and submits it to the database to create a new admin account
        /// Also calls ComputeHash to hash the entered password
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void CreateAdmin(object sender, EventArgs e)
        {
            var username = EntryUsername.Text;
            var password = EntryPassword.Text;
            var newsalt = "hvGirlXDVzdsCSrPmOdHRA==";

            if (username == null && password == null)
            {
                var nullResult = await DisplayAlert("Fill In All Fields", "Must enter Username and Password. Please Try Again", "Continue", "Cancel");
                if (nullResult)
                {
                    await Navigation.PushAsync(new ENGIE_App.views.AdminOptionsPage());
                }
            }
            else if (username == null)
            {
                var nullResult = await DisplayAlert("Fill In All Fields", "Must enter Username. Please Try Again", "Continue", "Cancel");
                if (nullResult)
                {
                    await Navigation.PushAsync(new ENGIE_App.views.AdminOptionsPage());
                }
            }
            else if (password == null)
            {
                var nullResult = await DisplayAlert("Fill In All Fields", "Must enter Password. Please Try Again", "Continue", "Cancel");
                if (nullResult)
                {
                    await Navigation.PushAsync(new ENGIE_App.views.AdminOptionsPage());
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

        }

        /// <summary>
        /// Method sets the email address for sending emails, also enables the send email button if valid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Set_Email(object sender, EventArgs e)
        {
            if (setButtonEnable)
            {
                eHelper.SetDes(EntryDesEmail.Text);

                Application.Current.Properties["desEmail"] = eHelper.GetDes();

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

        /// <summary>
        /// Sends a dummy email to test the set email
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void Send_Email(object sender, EventArgs e)
        {
            if (sendButtonEnable)
            {
                eHelper.SendEmail("Test", "This is bodyText");
                await this.DisplayAlert("Success", "email sent", "Continue", "Cancel");
            }
            else
            {
                await DisplayAlert("Alert", "Please enter a valid email", "OK");
            }
        }

        /// <summary>
        /// Uses regex to validate the email entered into the XAML page. 
        /// Runs when the box detects that the text has changed.
        /// When successful, it enables the button for calling Set_Email
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ValidateEmail(object sender, TextChangedEventArgs e)
        {
            setButtonEnable = Regex.IsMatch(EntryDesEmail.Text, @"^((([a-z, A-Z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z, A-Z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z, A-Z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z, A-Z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z, A-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z, A-Z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z, A-Z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z, A-Z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z, A-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z, A-Z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$");
            if (setButtonEnable) { FakeEnable(setEmailBtn); }
        }

        /// <summary>
        /// Takes the selected form from the XAML page and the set email address.
        /// Emails the address a newly generated QR code for the selected forms
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                        generator.SaveImage(generator.CreateImageFromText(QRData), eHelper.GetDes(), text);
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

        /// <summary>
        /// Method for converting the input from the dropdown list into the code for the QR code generator
        /// </summary>
        /// <returns></returns>
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