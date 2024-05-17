
using Newtonsoft.Json;
using StackExchange.Redis;
using IServer = StackExchange.Redis.IServer;
using WebApplication1.IRepository;
using WebApplication1;
namespace WMS

{
    public class CacheService : ICacheService
    {
        private IDatabase _db;  //private variable used to store connection
        private IServer _server;
        public CacheService()//constructor
        {
            ConfigureRedis();// calling the method
        }
        private void ConfigureRedis()  // method with data as void 
        {
            var connection = ConnectionHelper.Connection;
            _db = connection.GetDatabase();
            _server = connection.GetServer(connection.GetEndPoints().First());
        }
        public T GetData<T>(string key)
        {
            // Validate input parameter
            if (string.IsNullOrEmpty(key))
            {
                // Log invalid input parameter
                Console.WriteLine("Invalid input parameter for GetData method.");
                return default;
            }

            const int maxRetries = 3;
            int retryCount = 0;

            while (retryCount < maxRetries)
            {
                try
                {
                    var value = _db.StringGet(key);
                    if (!value.IsNull)
                    {
                        return JsonConvert.DeserializeObject<T>(value);
                    }
                    return default;
                }
                catch (RedisTimeoutException)
                {
                    // Log the timeout exception
                    Console.WriteLine("Timeout exception occurred while getting data from Redis cache. Retrying...");

                    // Increment retry count
                    retryCount++;

                    // Exponential backoff: Wait for some time before retrying (you can adjust the delay logic as needed)
                    int delayMs = 100 * (int)Math.Pow(2, retryCount); // Exponential backoff with base 2
                    Thread.Sleep(delayMs);
                }
                catch (Exception ex)
                {
                    // Log any other exceptions that occur during the process
                    Console.WriteLine($"An error occurred while getting data from Redis cache: {ex.Message}");
                    return default;
                }
            }

            // Log if maximum retries reached without success
            Console.WriteLine($"Maximum retry count ({maxRetries}) reached without success.");

            return default;
        }


        public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            // Validate input parameters
            if (string.IsNullOrEmpty(key) || value == null || expirationTime <= DateTimeOffset.Now)
            {
                // Log invalid input parameters
                Console.WriteLine("Invalid input parameters for SetData method.");
                return false;
            }

            const int maxRetries = 3;
            int retryCount = 0;

            while (retryCount < maxRetries)
            {
                try
                {
                    // Serialize the value to JSON
                    string serializedValue = JsonConvert.SerializeObject(value);

                    // Calculate expiration time relative to the current time
                    TimeSpan expiryTime = expirationTime.Subtract(DateTimeOffset.Now);

                    // Set data in Redis cache
                    var isSet = _db.StringSet(key, serializedValue, expiryTime);

                    if (!isSet)
                    {
                        // Log if data could not be set in Redis
                        Console.WriteLine("Failed to set data in Redis cache.");
                    }

                    return isSet;
                }
                catch (RedisTimeoutException)
                {
                    // Log the timeout exception
                    Console.WriteLine("Timeout exception occurred while setting data in Redis cache. Retrying...");

                    // Increment retry count
                    retryCount++;

                    // Exponential backoff: Wait for some time before retrying (you can adjust the delay logic as needed)
                    int delayMs = 100 * (int)Math.Pow(2, retryCount); // Exponential backoff with base 2
                    Thread.Sleep(delayMs);
                }
                catch (Exception ex)
                {
                    // Log any other exceptions that occur during the process
                    Console.WriteLine($"An error occurred while setting data in Redis cache: {ex.Message}");
                    return false;
                }
            }

            // Log if maximum retries reached without success
            Console.WriteLine($"Maximum retry count ({maxRetries}) reached without success.");

            return false;
        }




        public object RemoveData(string key)
        {
            var allks = _server.Keys().Where(x => x.ToString().Contains(key)).Select(X => X).ToList();
            foreach (var indkey in allks)
            {
                bool isKeyExist = _db.KeyExists(indkey);
                if (isKeyExist == true)
                {

                    return _db.KeyDelete(indkey);
                }
                return false;
            }
            return false;

        }


        public bool AppendData<T>(string key, T dataToAppend)
        {
            var serializedData = JsonConvert.SerializeObject(dataToAppend);
            return _db.ListRightPush(key, serializedData) > 0;
        }
        public void InvalidateCache(string key)
        {
            var keysToRemove = _server.Keys(pattern: key).ToList();
            foreach (var keey in keysToRemove)
            {
                _db.KeyDelete(keey);
            }
        }

        public void UpdateCache<T>(string key, T value, DateTimeOffset expirationTime)
        {
            // Invalidate the existing cache entry
            _db.KeyDelete(key);

            // Set new data in cache
            SetData(key, value, expirationTime);
        }

    }
}
