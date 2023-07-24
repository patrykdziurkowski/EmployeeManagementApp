using System.Configuration;

namespace DataAccess
{
    public class ConnectionStringProvider
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
        private readonly string _host;
        private readonly string _port;
        private readonly string _sid;

        private UserCredentials _userCredentials;


        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public ConnectionStringProvider(UserCredentials userCredentials)
        {
            _userCredentials = userCredentials;

            string? host = ConfigurationManager.AppSettings["host"];
            string? port = ConfigurationManager.AppSettings["port"];
            string? sid = ConfigurationManager.AppSettings["sid"];

            if (host is null || port is null || sid is null)
            {
                throw new ConfigurationErrorsException("Application's host, post and sid must all be configured");
            }

            _host = host;
            _port = port;
            _sid = sid;
        }


        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////
        public string GetConnectionString()
        {
            if (_userCredentials.AreNotFilledOut())
            {
                throw new InvalidOperationException("Cannot provide a connection string before the user is authenticated");
            }

            return $"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={_host})(PORT={_port})))  (CONNECT_DATA = (SERVER = DEDICATED)(SERVICE_NAME = {_sid}))); User Id = {_userCredentials.UserName};Password = {_userCredentials.Password}";
        }
    }
}
