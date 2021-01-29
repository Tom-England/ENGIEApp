using System;
using System.Collections.Generic;
using System.Text;

namespace ENGIE_App.Tables
{
    /// <summary>
    /// Helper class for adding user info to the local database
    /// </summary>
    public class LogUserData
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }


    }
}
