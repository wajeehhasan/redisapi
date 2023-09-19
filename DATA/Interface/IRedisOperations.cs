
namespace DATA.Interface
{
    public interface IRedisOperations
    {

        T GetData<T>(string key);
        bool SetData<T>(string key, T value, DateTimeOffset expirationTime);
        object RemoveData(string key);

    }
}
