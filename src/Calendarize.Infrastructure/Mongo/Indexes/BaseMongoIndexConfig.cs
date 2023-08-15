using Calendarize.Infrastructure.Providers;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace Calendarize.Infrastructure.Mongo.Indexes
{
    public abstract class BaseMongoIndexConfig<T> : IMongoIndexConfig
    {
        private readonly IMongoCollection<T> _collection;

        protected BaseMongoIndexConfig(IMongoCollectionProvider collectionProvider)
        {
            _collection = collectionProvider.Get<T>();
        }

        public abstract void CreateIndexes();

        protected virtual void CreateMany(IEnumerable<IndexKeysDefinition<T>> definitions,
            CreateIndexOptions<T> options)
            => _collection.Indexes.CreateMany(definitions.Select(x => new CreateIndexModel<T>(x, options)));
    }
}
