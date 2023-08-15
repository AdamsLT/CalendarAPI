using Calendarize.Core.Entities;
using Calendarize.Infrastructure.Providers;
using Calendarize.Infrastructure.Repositories.Common;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Calendarize.Infrastructure.Repositories
{
    public class EventRepository : BaseRepository<Event>
    {
        public EventRepository(IEventCollectionProvider collectionProvider) : base(collectionProvider)
        {
        }

        public async Task DeleteAsync(ObjectId id)
        {
            var filter = Builders<Event>.Filter
                .Eq(x => x.Id, id);

            var result = await Collection.DeleteOneAsync(filter);
        }

        public async Task<IEnumerable<Event>> GetAllAsync(
            DateTime? startDate, DateTime? endDate, ObjectId? locationId, ObjectId? coachId)
        {
            var filter = GetFilters(startDate, endDate, locationId, coachId);
            var result = await Collection.Find(filter).ToListAsync();
            return result;
        }

        public async Task<Event> GetAsync(ObjectId id)
        {
            var filter = Builders<Event>.Filter.Eq(x => x.Id, id);

            var result = await Collection.Find(filter).FirstOrDefaultAsync();
            return result;
        }

        public async Task<Event> UpsertAsync(Event @event)
        {
            var filter = Builders<Event>.Filter.Eq(x => x.Id, @event.Id);

            var update = Builders<Event>.Update
                .SetOnInsert(x => x.CreatedAt, @event.CreatedAt)
                .Set(x => x.UpdatedAt, @event.UpdatedAt)
                .Set(x => x.Name, @event.Name)
                .Set(x => x.Description, @event.Description)
                .Set(x => x.CoachId, @event.CoachId)
                .Set(x => x.Capacity, @event.Capacity)
                .Set(x => x.LocationId, @event.LocationId)
                .Set(x => x.StartsAt, @event.StartsAt)
                .Set(x => x.EndsAt, @event.EndsAt)
                .Set(x => x.Tags, @event.Tags);

            var result = await Collection.FindOneAndUpdateAsync(filter, update, new FindOneAndUpdateOptions<Event>
            {
                IsUpsert = true,
                ReturnDocument = ReturnDocument.After
            });

            return result;
        }


        private FilterDefinition<Event> GetFilters(
            DateTime? startDate, DateTime? endDate, ObjectId? locationId, ObjectId? coachId)
        {
            var filterDefinitions = new List<FilterDefinition<Event>>();
            var builder = Builders<Event>.Filter;

            filterDefinitions.Add(builder.Gte(x => x.StartsAt, startDate ?? DateTime.MinValue));

            if (endDate != null) 
                filterDefinitions.Add(builder.Lte(x => x.EndsAt, endDate));

            if (locationId != null) 
                filterDefinitions.Add(builder.Eq(x => x.LocationId, locationId));

            if (coachId != null) 
                filterDefinitions.Add(builder.Eq(x => x.CoachId, coachId));

            return builder.And(filterDefinitions);
        }
    }
}
