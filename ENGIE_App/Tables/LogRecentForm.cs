using System;
using System.Collections.Generic;
using System.Text;

namespace ENGIE_App.Tables
{
    /// <summary>
    /// Helper class for adding recent forms to the local database
    /// </summary>
    public class LogRecentForm
    {
        public Guid FormId { get; set; }
        public DateTime DateTime { get; set; }
        public string Form { get; set; }
        public Boolean Sent { get; set; }
    }
}
