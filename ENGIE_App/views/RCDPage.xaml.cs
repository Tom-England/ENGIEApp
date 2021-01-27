using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using Syncfusion.Pdf.Parsing;

using Xamarin.Forms;
using GettingStarted;

namespace ENGIE_App.views
{
    public partial class RCDPage : ContentPage
    {
        public RCDPage()
        {
            InitializeComponent();
        }
        public async void Button_Clicked(object sender, System.EventArgs e)
        {


            var reference = RefEntry.Text;
            var address = AddressEntry.Text;
            var date = DateEntry.Date;

            var annual = AnnualEntry.Text;
            var threeMonthly = ThreeMonthlyEntry.Text;

            var name = NameEntry.Text;
            var signature = SignatureEntry.Text;

            var switchboard = SwitchboardEntry.Text;
            var circuit = CircuitEntry.Text;
            var functional = FunctionalEntry.Text;
            var xOne = XOneEntry.Text;
            var xFive = XFiveEntry.Text;

            var comments = CommentsEntry.Text;


            //FileStream outputFileStream = File.Open(filepath, FileMode.Create);
            var assembly = typeof(MainPage).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream("ProjectParseTestingV2.RCDTestSheet.pdf");
            PdfLoadedDocument loadedDocument = new PdfLoadedDocument(stream);

            PdfLoadedForm form = loadedDocument.Form;

            (form.Fields["Job Ref"] as PdfLoadedTextBoxField).Text = RefEntry.Text;
            (form.Fields["Site Address"] as PdfLoadedTextBoxField).Text = AddressEntry.Text;
            //(form.Fields["Date"] as PdfLoadedTextBoxField).Text = DateEntry.ToString;

            (form.Fields["Annual Service"] as PdfLoadedTextBoxField).Text = AnnualEntry.Text;
            (form.Fields["3 Monthly"] as PdfLoadedTextBoxField).Text = ThreeMonthlyEntry.Text;

            (form.Fields["Name of operative"] as PdfLoadedTextBoxField).Text = NameEntry.Text;
            (form.Fields["Signature"] as PdfLoadedTextBoxField).Text = SignatureEntry.Text;

            (form.Fields["SwitchboardRow1"] as PdfLoadedTextBoxField).Text = SwitchboardEntry.Text;
            (form.Fields["CircuitRow1"] as PdfLoadedTextBoxField).Text = CircuitEntry.Text;
            (form.Fields["Functional testRow1"] as PdfLoadedTextBoxField).Text = FunctionalEntry.Text;
            (form.Fields["X1_1_1"] as PdfLoadedTextBoxField).Text = XOneEntry.Text;
            (form.Fields["X5_1_1"] as PdfLoadedTextBoxField).Text = XFiveEntry.Text;

            (form.Fields["Details Comments on any failed RCDs"] as PdfLoadedTextBoxField).Text = CommentsEntry.Text;


            MemoryStream streams = new MemoryStream();


            loadedDocument.Save(streams);
            //Xamarin.Forms.DependencyService.Get<ISave>().SaveTextAsync("Creation.pdf", "application/pdf", streams);
            loadedDocument.Close(true);

            Xamarin.Forms.DependencyService.Get<ISave>().SaveAndView("Output.pdf", "application/pdf", streams);

            var filename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Output.pdf");
            var email = new EmailHelper();
            email.SendEmail(filename);



        }
    }
}

