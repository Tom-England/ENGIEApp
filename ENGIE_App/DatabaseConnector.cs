using MySqlConnector;
using Renci.SshNet;
using System;

namespace ENGIE_App.views
{
    /// <summary>
    /// Class for handling non local database connections
    /// </summary>
    public class DatabaseConnector
    {
        MySqlConnection connection;
        SshClient client;

        /// <summary>
        /// ENTER USERNAME AND PASSWORD HERE FOR LOGIN
        /// </summary>
        String bnumber = "bnumber";
        String unipass = "unipass";

        public DatabaseConnector()
        {

        }

        /// <summary>
        /// Method for creating a MySqlConnection object that is ready to connect to the database
        /// </summary>
        /// <returns>MySqlConnection</returns>
        public MySqlConnection Connect_Database()
        {
            PasswordConnectionInfo connectionInfo = new PasswordConnectionInfo("linux.cs.ncl.ac.uk", bnumber, unipass);
            connectionInfo.Timeout = TimeSpan.FromSeconds(30);
            client = new SshClient(connectionInfo);
            client.Connect();
            var x = client.IsConnected;
            ForwardedPortLocal portFwld = new ForwardedPortLocal("127.0.0.1", "cs-db.ncl.ac.uk", 3306);
            client.AddForwardedPort(portFwld);
            portFwld.Start();
            connection = new MySqlConnection("server = 127.0.0.1; Database = t2033t40; UID = t2033t40; PWD = AwedPace%Car; Port = " + portFwld.BoundPort);
            return connection;
        }

        /// <summary>
        /// Method for closing the connection when done
        /// </summary>
        public void Close_Connection()
        {
            connection.Close();
            client.Disconnect();
        }
    }
}
