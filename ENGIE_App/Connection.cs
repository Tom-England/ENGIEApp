using System;
using Xamarin.Essentials;

namespace ENGIE_App
{
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
