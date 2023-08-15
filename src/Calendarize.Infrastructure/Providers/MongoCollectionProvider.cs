using MongoDB.Driver;

namespace Calendarize.Infrastructure.Providers
{
    public class MongoCollectionProvider :
        IEventCollectionProvider,
        ILocationCollectionProvider,
        IRegistrationCollectionProvider,
        IUserCollectionProvider
    {
        private readonly IMongoDatabase _database;

        private readonly string _collectionName;

        public MongoCollectionProvider(IMongoDatabase database, string collectionName)
        {
            _database = database;
            _collectionName = collectionName;
        }

        public IMongoCollection<T> Get<T>()
        {
            return _database.GetCollection<T>(_collectionName);
        }
    }
}
