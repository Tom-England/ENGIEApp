﻿using MySqlConnector;
using Renci.SshNet;
using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace ENGIE_App.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminOptionsPage : ContentPage
    {
        MySqlConnection connection;
        SshClient client;
        String bnumber = "bnum";
        String unipass = "unipass";

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

        private void GenerateQR(object sender, EventArgs e)
        {
            var generator = new QRBuilder();
            var QRData = generator.CreateQRCode(EntryQRText.Text);

            QRLabel.Text = "Generated Successfully";
            //await Clipboard.SetTextAsync(QRData);
            Console.WriteLine(QRData);

        }
    }
}