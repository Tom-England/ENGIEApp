using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;
using QRCoder;

namespace ENGIE_App
{
    class QRBuilder
    {
        public QRBuilder()
        {
        }

        public string CreateQRCode(String text)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
            AsciiQRCode qrCode = new AsciiQRCode(qrCodeData);
            string qrCodeImageAsBase64 = qrCode.GetGraphic(1);
            return qrCodeImageAsBase64;
        }

    }
}
