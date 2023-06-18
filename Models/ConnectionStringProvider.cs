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


        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public ConnectionStringProvider()
        {
            _host = ConfigurationManager.AppSettings["host"].ToString();
            _port = ConfigurationManager.AppSettings["port"].ToString();
            _sid = ConfigurationManager.AppSettings["sid"].ToString();
        }


        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////
        public string GetConnectionString(string username, string password)
        {
            return $"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={_host})(PORT={_port})))  (CONNECT_DATA = (SERVER = DEDICATED)(SERVICE_NAME = {_sid}))); User Id = {username};Password = {password}";
        }
    }
}
