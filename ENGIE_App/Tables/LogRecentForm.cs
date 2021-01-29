using System;
using System.Collections.Generic;
using System.Text;

namespace ENGIE_App.Tables
{
    public class LogRecentForm
    {
        public Guid FormId { get; set; }
        public DateTime DateTime { get; set; }
        public string Form { get; set; }
        public Boolean Sent { get; set; }
    }
}
