using System;
using ENGIE_App.Tables;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ENGIE_App.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecentlySubmitted : ContentPage
    {

        public ObservableCollection<MyItem> MyItems { get; set; }

        /// <summary>
        /// Initializer for the page, hides the original menu options
        /// </summary>
        public RecentlySubmitted()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();

            MyItems = new ObservableCollection<MyItem>();


            //addItemToTable(new MyItem { Date = "17/12/2020", Time = "16:32:12", Form = "Form B", Sent = true });
            var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDatabase.db");
            var db = new SQLiteConnection(dbpath);
            var info = db.GetTableInfo("LogRecentForm");
            if (info.Any())
            {
                List<LogRecentForm> rows = db.Table<LogRecentForm>().ToList();
                rows.Reverse();
                for (int i = 0; i < rows.Count; i++)
                {
                    addItemToTable(new MyItem { Date = rows[i].DateTime.ToString(), Form = rows[i].Form, Sent = rows[i].Sent });
                }
            }
        }

        /// <summary>
        /// Adds an item to the table
        /// </summary>
        /// <param name="item"></param>
        private void addItemToTable(MyItem item)
        {
            MyItems.Add(item);
            listView.ItemsSource = MyItems;
        }

        /// <summary>
        /// Method attempts to email any unsent forms to the user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void resend(object sender, System.EventArgs e)
        {
            if (Connection.isConnected())
            {
                var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDatabase.db");
                var db = new SQLiteConnection(dbpath);
                var info = db.GetTableInfo("LogRecentForm");
                if (info.Any())
                {
                    //resending code
                    var email = new EmailHelper();
                    var subject = "Re-send email that failed to send";
                    var body = "Re-send email that failed recently.";
                    var filepath = "";

                    var rows = db.Table<LogRecentForm>().ToList();
                    db.DropTable<LogRecentForm>();
                    for (int i = 0; i < rows.Count; i++)
                    {
                        if (rows[i].Sent == false)
                        {
                            filepath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), rows[i].Form);
                            rows[i].Sent = true;
                            email.SendEmail(subject, body, filepath);
                        }
                        RecordForm.addToRecentForms(rows[i].Form, true);
                    }
                    RegenTable();
                }
                
            } else
            {
                await DisplayAlert("Alert", "No internet connection, please try again later", "ok");
            }
        }

        /// <summary>
        /// Method regenerates the table with new values
        /// </summary>
        void RegenTable()
        {
            MyItems.Clear();
            var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDatabase.db");
            var db = new SQLiteConnection(dbpath);
            var info = db.GetTableInfo("LogRecentForm");
            if (info.Any())
            {
                List<LogRecentForm> rows = db.Table<LogRecentForm>().ToList();
                rows.Reverse();
                for (int i = 0; i < rows.Count; i++)
                {
                    addItemToTable(new MyItem { Date = rows[i].DateTime.ToString(), Form = rows[i].Form, Sent = rows[i].Sent });
                }
            }
        }
    }
}

/// <summary>
/// Helper class for storing a table item
/// </summary>
public class MyItem
    {
        public string Date { get; set; }
        public string Form { get; set; }
        public bool Sent { get; set; }   
    }