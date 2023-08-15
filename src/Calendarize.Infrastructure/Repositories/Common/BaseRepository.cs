using Calendarize.Infrastructure.Providers;
using MongoDB.Driver;

namespace Calendarize.Infrastructure.Repositories.Common
{
    public abstract class BaseRepository<T> where T : class
    {
        protected readonly IMongoCollection<T> Collection;

        protected BaseRepository(IMongoCollectionProvider collectionProvider)
        {
            Collection = collectionProvider.Get<T>();
        }
    }
}
