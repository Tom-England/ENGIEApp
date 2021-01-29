using System;
using Xamarin.Essentials;

namespace ENGIE_App
{
    /// <summary>
    /// Class for checking if the user has internet connection
    /// </summary>
    class Connection
    {
        public static Boolean isConnected()
        {
            var current = Connectivity.NetworkAccess;
            Boolean connected = current == NetworkAccess.Internet;

            return connected;
        }
    }
}
