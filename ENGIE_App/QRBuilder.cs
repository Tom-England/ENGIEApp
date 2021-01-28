using System;
using SkiaSharp;
using QRCoder;
using System.IO;

namespace ENGIE_App
{
    class QRBuilder
    {
        /// <summary>
        /// Colours for the QR code
        /// </summary>
        public SKColor lightColour;
        public SKColor darkColour;

        /// <summary>
        /// Initializer for the QRBuilder, sets the default colours
        /// </summary>
        public QRBuilder()
        {
            lightColour = SKColors.White;
            darkColour = SKColors.Black;
        }

        /// <summary>
        /// Creates a QR code out of ASCII characters from the provided text
        /// </summary>
        /// <param name="text"></param>
        /// <returns>string</returns>
        public string CreateQRCode(String text)
        {
            if(text == null) { text = ""; }
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
            AsciiQRCode qrCode = new AsciiQRCode(qrCodeData);
            string qrCodeImageAsASCII = qrCode.GetGraphic(1);
            return qrCodeImageAsASCII;
        }

        /// <summary>
        /// Takes in the ASCII QR Code generated in CreateQRCode and then converts it to a bitmap
        /// </summary>
        /// <param name="text"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public SKBitmap CreateImageFromText(string text, int size=294)
        {
            SKBitmap qr = new SKBitmap(size, size);
            var pixelHeight = size / 21;
            var pixelWidth = size / 42;
            var currentRow = 0;
            var currentColumn = 0;
            var bufferSize = 8; // QR code is generated with 8 cells of whitespace on either side
            foreach(char ch in text)
            {
                if (currentColumn >= bufferSize && currentColumn < 42+bufferSize)
                {
                    if (ch.Equals(' '))
                    {
                        for(int y = 0; y < pixelHeight; y++)
                        {
                            for(int x = 0; x < pixelWidth; x++)
                            {
                                qr.SetPixel((currentColumn - bufferSize)*pixelWidth+x, (currentRow) * pixelHeight+y, this.lightColour);
                            }
                        }
                        currentColumn++;
                    }
                    else
                    {
                        for (int y = 0; y < pixelHeight; y++)
                        {
                            for (int x = 0; x < pixelWidth; x++)
                            {
                                qr.SetPixel((currentColumn - bufferSize) * pixelWidth + x, (currentRow) * pixelHeight + y, this.darkColour);
                            }
                        }
                        currentColumn++;
                    }
                } else
                {
                    if (ch.Equals('\n'))
                    {
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

        /// <summary>
        /// Takes a bitmap and an email then converts the bitmap to a png and emails it to the user.
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="desEmail"></param>
        /// /// <param name="name"></param>
        public void SaveImage(SKBitmap bmp, string name)
        {
            var image = SKImage.FromBitmap(bmp);
            var data = image.Encode();
            // get a writable file path
            var filename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), name+".png");
            using (var stream = File.OpenWrite(filename))
            {
                data.SaveTo(stream);
            }

            EmailHelper eHelper = new EmailHelper();
            eHelper.SendEmail("QR Code", "ENGIE App QR Code", filename);
        }
    }
}
