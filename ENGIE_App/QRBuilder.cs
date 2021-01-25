using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;
using QRCoder;
using System.IO;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Essentials;

namespace ENGIE_App
{
    class QRBuilder
    {
        public SKColor lightColour;
        public SKColor darkColour;
        public QRBuilder()
        {
            lightColour = SKColors.White;
            darkColour = SKColors.Black;
        }

        public string CreateQRCode(String text)
        {
            if(text == null) { text = ""; }
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
            AsciiQRCode qrCode = new AsciiQRCode(qrCodeData);
            string qrCodeImageAsBase64 = qrCode.GetGraphic(1);
            return qrCodeImageAsBase64;
        }

        public SKBitmap CreateImageFromText(string text, int size=42)
        {
            SKBitmap qr = new SKBitmap(size, size);
            var pixelHeight = size / 20;
            var pixelWidth = size / 40;
            var currentRow = 0;
            var currentColumn = 0;
            var bufferSize = 8; // QR code is generated with 8 cells of whitespace on either side
            foreach(char ch in text)
            {
                if (currentColumn >= bufferSize && currentColumn < 42+bufferSize)
                {
                    Console.WriteLine("Y:" + currentRow + " X:" + currentColumn);
                    if (ch.Equals(' '))
                    {
                        currentColumn++;
                    }
                    else
                    {
                        qr.SetPixel(currentColumn-bufferSize, (currentRow*2), this.darkColour);
                        qr.SetPixel(currentColumn - bufferSize, (currentRow*2)+1, this.darkColour);
                        currentColumn++;
                    }
                } else
                {
                    if (ch.Equals('\n'))
                    {
                        Console.WriteLine("New Line");
                        currentColumn = 0;
                        currentRow++;
                    }
                    else
                    {
                        currentColumn++;
                    }
                    
                }
            }
            return qr;
        }

        public async void SaveImage(SKBitmap bmp)
        {
            var image = SKImage.FromBitmap(bmp);
            var data = image.Encode();
            // get a writable file path
            //var test = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var filename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "MyQR.png");
            //var filename = Path.Combine(test, "MyQR.png");
            Console.WriteLine(filename);
            using (var stream = File.OpenWrite(filename))
            {
                data.SaveTo(stream);
            }

            try
            {
                var message = new EmailMessage
                {
                    Subject = "QR Code",
                    Body = "Hello, World!",
                    To = new List<string> { "tomengland6679@gmail.com" }
                    //Cc = ccRecipients,
                    //Bcc = bccRecipients
                };
                message.Attachments.Add(new EmailAttachment(filename));
                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException fbsEx)
            {
                // Email is not supported on this device
            }
            catch (Exception ex)
            {
                // Some other exception occurred
            }
        }
    }
}
