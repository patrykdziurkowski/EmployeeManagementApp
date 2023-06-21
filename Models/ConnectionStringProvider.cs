using System.Configuration;

namespace Models
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

            _host = ConfigurationManager.AppSettings["host"].ToString();
            _port = ConfigurationManager.AppSettings["port"].ToString();
            _sid = ConfigurationManager.AppSettings["sid"].ToString();
        }


        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////
        public string GetConnectionString()
        {
            return $"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={_host})(PORT={_port})))  (CONNECT_DATA = (SERVER = DEDICATED)(SERVICE_NAME = {_sid}))); User Id = {_userCredentials.UserName};Password = {_userCredentials.Password}";
        }
    }
}
