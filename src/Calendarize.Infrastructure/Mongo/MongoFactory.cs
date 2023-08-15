using MongoDB.Driver;

namespace Calendarize.Infrastructure.Mongo
{
    public static class MongoFactory
    {
        public static IMongoDatabase Create(MongoConfig mongoConfig)
        {
            var client = new MongoClient(mongoConfig.ConnectionString);
            return client.GetDatabase(mongoConfig.Database);
        }
    }
}
