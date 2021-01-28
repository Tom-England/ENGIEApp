using System;
using MailKit.Net.Smtp;
using MimeKit;
using Xamarin.Essentials;
using ENGIE_App.views;
using MySqlConnector;

namespace ENGIE_App
{
    class EmailHelper
    {
        MySqlConnection connection;
        DatabaseConnector dbconn = new DatabaseConnector();

        public string email = "csc2033team40@gmail.com";

        public EmailHelper()
        {
            SetDes(GetDes());
        }

        /// <summary>
        /// Setter method for the destination email
        /// </summary>
        /// <param name="desEmail"></param>
        public void SetDes(string desEmail)
        {
            email = desEmail;
        }

        /// <summary>
        /// Getter method for the destination email
        /// Gets most recent email from database
        /// </summary>
        /// <returns>email</returns>
        public string GetDes()
        {
            if (connection == null)
            {
                connection = dbconn.Connect_Database();
                connection.Open();
            }
            try
            {

                // SQL code
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT Email FROM AdminEmail ORDER BY Id DESC LIMIT 1";

                cmd.ExecuteNonQuery();
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    email = reader.GetValue(0).ToString();
                    Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!" + email);
                    reader.Close();
                }
                else
                {
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            dbconn.Close_Connection();
            Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>" + email);
            return email;
            
        }

        /// <summary>
        /// Main function for sending emails, enter a filepath if you wish to attach a file.
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="bodyText"></param>
        /// <param name="filepath"></param>
        public async void SendEmail(string subject, string bodyText, string filepath = null)
        {
            if (email!=null) {
                await Permissions.RequestAsync<Permissions.StorageRead>();

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Team 40", "csc2033team40@gmail.com"));
                message.To.Add(new MailboxAddress("", email));
                message.Subject = subject;

                var builder = new BodyBuilder();

                // Set the plain-text version of the message text
                builder.TextBody = bodyText;

                if (filepath != null)
                {
                    builder.Attachments.Add(filepath);
                }

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
            } else
            {
                Console.WriteLine("ERROR: Email not set");
            }
        }
    }
}
