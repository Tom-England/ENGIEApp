using ENGIE_App.Tables;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ENGIE_App.views
{
    /// <summary>
    /// Class for handling the contents of Form Page, used for recently submitted forms
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FormPage : ContentPage
    {
        public FormPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();
        }
        void submit(object sender, System.EventArgs e)
        {
            String pdf = "pdf1";
            //call pdf function
            Boolean connected = Connectivity.NetworkAccess == NetworkAccess.Internet;
            ((Button)sender).Text = connected.ToString();
            if (connected)
            {
                //call email function
            }

            //store in database
            addToRecentForms(DateTime.Now, pdf, connected);
        }

        void addToRecentForms(DateTime dateTime, String form, Boolean connected)
        {
            // local database
            var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDatabase.db");
            var db = new SQLiteConnection(dbpath);
            db.CreateTable<LogRecentForm>();

            // adds values to local database
            var item = new LogRecentForm()
            {
                DateTime = dateTime,
                Form = form,
                Sent = connected
            };

            db.Insert(item);
        }
    }
}
