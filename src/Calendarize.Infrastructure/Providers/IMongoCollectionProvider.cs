using MongoDB.Driver;

namespace Calendarize.Infrastructure.Providers
{
    public interface IMongoCollectionProvider
    {
        IMongoCollection<T> Get<T>();
    }
}
