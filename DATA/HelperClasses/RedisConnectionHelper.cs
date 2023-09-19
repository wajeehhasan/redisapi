using StackExchange.Redis;

namespace DATA.HelperClasses
{

    public class ConnectionHelper
    {
        public static ConfigurationOptions GetConfiguration(string host, bool
ssl, string accessKey, string clientName)
        {
            var configuration = ConfigurationOptions.Parse(host);
            configuration.Ssl = ssl;
            configuration.ClientName = clientName;
            configuration.AbortOnConnectFail = false;
            configuration.Password = accessKey;
            return configuration;
        }

    }
    
}
