using Calendarize.Core.Entities;
using Calendarize.Infrastructure.Providers;
using MongoDB.Driver;

namespace Calendarize.Infrastructure.Mongo.Indexes
{
    public class EventIndexConfig : BaseMongoIndexConfig<Event>
    {
        public EventIndexConfig(IEventCollectionProvider collectionProvider) : base(collectionProvider)
        {
        }

        public override void CreateIndexes()
        {
            var options = new CreateIndexOptions<Event> { Background = true };
            var builder = Builders<Event>.IndexKeys;

            var definitions = new[]
            {
                builder.Ascending(x => x.StartsAt)
                       .Ascending(x => x.EndsAt)
                       .Ascending(x => x.LocationId)
                       .Ascending(x => x.CoachId)
            };

            CreateMany(definitions, options);
        }
    }
}
