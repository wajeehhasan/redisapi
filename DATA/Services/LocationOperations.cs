using DATA.Interface;
using DATA.Models;
using Microsoft.Extensions.Configuration;

namespace DATA.Services
{
    public class LocationOperations : ILocationOperations
    {
        private readonly IHttpOperations _httpOperations;
        private readonly IConfiguration _config;
        private readonly IRedisOperations _redisOperations;
        public LocationOperations(IConfiguration config, IHttpOperations httpOperations, IRedisOperations redisOperations)
        {
            _config = config;
            _httpOperations = httpOperations;
            _redisOperations = redisOperations;
        }
        public async Task<GenericResultSet<LocationData>> GetIpDetailsAsync(string ip_address)
        {
            //getting redis configuration from appsettings
            var enableRedis = bool.Parse(_config.GetSection("RedisConfiguration:UseRedis").Value);
            var cacheExpiry = double.Parse(_config.GetSection("RedisConfiguration:CacheExpiry").Value);

            GenericResultSet<LocationData> response = new();
            var expirationTime = DateTimeOffset.Now.AddMinutes(cacheExpiry); //setting expiry time for redis key

            if (enableRedis) //redis will be used only if set true in application settings
            {
                var redis_returned_obj = _redisOperations.GetData<LocationData>(ip_address);
                if (redis_returned_obj != null)
                {
                    response.resultSet = redis_returned_obj;
                    Console.WriteLine(response.resultSet);
                    return response;
                }
            }
            Task.Delay(5000);
            var url = _config.GetSection("IpStackUrl").Value + ip_address;
            var queryParams = new Dictionary<string, string>(){
                {"access_key", _config.GetSection("IpstackAccessKey").Value}
            };

            var returnedObj = await _httpOperations.GetHttpResponse(url, queryParams);
            response = _httpOperations.GenericResponseGenerate<LocationData>(returnedObj);
            var status_redis = _redisOperations.SetData<LocationData>(ip_address,response.resultSet, expirationTime);
            Console.WriteLine(status_redis);
            return response;
        }


    }
}
