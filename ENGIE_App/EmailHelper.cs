﻿using MySqlConnector;
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

using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace ENGIE_App
{
    class EmailHelper
    {

        public string email;

        public EmailHelper()
        {
        }

        public void SetDes(string desEmail)
        {
            email = desEmail;
        }

        public string GetDes()
        {
            return email;
        }

        // Main function for sending emails, enter a filepath if you wish to attach a file.
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
