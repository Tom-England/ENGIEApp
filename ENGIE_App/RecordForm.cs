using System;
using System.Collections.Generic;
using System.Text;

using ENGIE_App.Tables;
using SQLite;
using System.IO;

namespace ENGIE_App
{
    class RecordForm
    {
        public static void addToRecentForms(String form, Boolean connected)
        {
            // local database
            var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDatabase.db");
            var db = new SQLiteConnection(dbpath);
            db.CreateTable<LogRecentForm>();

            // adds values to local database
            var item = new LogRecentForm()
            {
                DateTime = DateTime.Now,
                Form = form,
                Sent = connected
            };

            db.Insert(item);
        }
    }
}
