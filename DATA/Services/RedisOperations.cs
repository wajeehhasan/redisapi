using DATA.HelperClasses;
using DATA.Interface;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;


namespace DATA.Services
{
    public class RedisOperations : IRedisOperations
    {
        private IDatabase _db;
        private readonly IConfiguration _config;
        public RedisOperations(IConfiguration config)
        {
            _config = config;
            //redis-server connection
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(_config.GetSection("RedisConfiguration:RedisURL").Value);
            _db = redis.GetDatabase();
            //redis configuration

        }
        public T GetData<T>(string key)
        {
            var value = _db.StringGet(key);
            if (!string.IsNullOrEmpty(value))
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
            return default;
        }

        public object RemoveData(string key)
        {
            bool _isKeyExist = _db.KeyExists(key);
            if (_isKeyExist == true)
            {
                return _db.KeyDelete(key);
            }
            return false;
        }

        public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            TimeSpan expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
            var isSet = _db.StringSet(key, JsonConvert.SerializeObject(value), expiryTime);
            return isSet;
        }
    }
}
