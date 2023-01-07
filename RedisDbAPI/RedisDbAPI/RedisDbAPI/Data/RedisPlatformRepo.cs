using RedisDbAPI.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace RedisDbAPI.Data
{
    public class RedisPlatformRepo : IPlatformRepo
    {
        private readonly IConnectionMultiplexer _redis;

        public RedisPlatformRepo(IConnectionMultiplexer redis)
        {
            this._redis = redis;
        }
        public void CreatePlatform(Platform plat)
        {
            if(plat == null)
            { 
                throw new ArgumentOutOfRangeException(nameof(plat));
            }

            var db = _redis.GetDatabase();

            var serialPlat = JsonSerializer.Serialize(plat);

            //db.StringSet(plat.Id, serialPlat);

            //here we add new platform to set with key PlatformSet
            //db.SetAdd("PlatformSet", serialPlat);

            db.HashSet("hashplatform", new HashEntry[]
            {
                new HashEntry(plat.Id, serialPlat)
            }) ;
        }

        public Platform GetPlatformById(string id)
        {
            var db = _redis.GetDatabase();

            /* this is example where we are saving individual platforms as strings in redisDb 
            var plat = db.StringGet(id);

            if (!string.IsNullOrEmpty(plat))
            {
                return JsonSerializer.Deserialize<Platform>(plat);
            }
            return null; */

            var plat = db.HashGet("hashplatform", id);

            if (!string.IsNullOrEmpty(plat))
            {
                return JsonSerializer.Deserialize<Platform>(plat);
            }
            return null; 

        }

        public IEnumerable<Platform> GetPlatforms()
        {
            var db = _redis.GetDatabase();

            /* this is example when using Sets but it is better to use Hashes
             
            //retrive all members of Set with key PlatformSet
            var completeSet = db.SetMembers("PlatformSet");

            if(completeSet.Length > 0)
            {
                var obj = Array.ConvertAll(completeSet, val => JsonSerializer.Deserialize<Platform>(val)).ToList();

                return obj; 
            }
            
            return null;*/

            var completeHash = db.HashGetAll("hashplatform");

            if(completeHash.Length > 0)
            {
                var obj = Array.ConvertAll(completeHash, val => JsonSerializer.Deserialize<Platform>(val.Value)).ToList();

                return obj; 
            }
            return null; 
        }
    }
}
