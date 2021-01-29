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
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();
            (Application.Current.MainPage as RootPage).disableGesture();
        }

        public delegate void HandlePopDelegate(string parameter);
        public event HandlePopDelegate DidFinishPopping;
        public async void Button_Clicked(object sender, System.EventArgs e)
        {
            Boolean connected = Connection.isConnected();
            try
            {
                var assembly = typeof(MainPage).GetTypeInfo().Assembly;

                Stream stream = assembly.GetManifestResourceStream("ENGIE_App.RCDTestSheet.pdf");

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
                loadedDocument.Close(true);

                var date = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
                var title = "RCD-Form-" + date + ".pdf";
                string filepath = await Xamarin.Forms.DependencyService.Get<ISave>().Save(title, "application/pdf", streams);
                var email = new EmailHelper();
                var subject = "RCD form submission";
                var body = "";
                
                // Check if firstname/lastname are present.  Admins do not need have names stored and as such the message could disrupt admins who are testing the app
                if (Application.Current.Properties.ContainsKey("Firstname") && Application.Current.Properties.ContainsKey("Lastname"))
                {
                    body = "RCD form attatched as PDF.  Submitted by " + Application.Current.Properties["Firstname"] + " " + Application.Current.Properties["Lastname"];
                }
                else
                {
                    body = "RCD form attatched as PDF.";
                }
                
                // Set the destination email address for the form and send
                email.SendEmail(subject, body, filepath);
                if (connected)
                {
                    email.SendEmail(subject, body, filepath);
                }
                RecordForm.addToRecentForms(title, connected);

                // Close page, and trigger event when doing so.
                // This refreshes scan page to fix a visual bug and refresh qr scanner results
                (Application.Current.MainPage as RootPage).enableGesture();
                await Navigation.PopAsync();
                DidFinishPopping("FormPopped");

            }
            catch (ArgumentNullException)
            {
                DisplayAlert("Error", "All fields must be filled in.", "Continue");
            }
            catch (KeyNotFoundException)
            {
                DisplayAlert("Error", "No destination email address set", "Continue");
            }
        }
    }
}

