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
    public class RegistrationRepository : BaseRepository<Registration>
    {
        public RegistrationRepository(IRegistrationCollectionProvider collectionProvider) : base(collectionProvider)
        {
        }

        public async Task DeleteAsync(ObjectId id)
        {
            var filter = Builders<Registration>.Filter
                .Eq(x => x.Id, id);

            var result = await Collection.DeleteOneAsync(filter);
        }

        public async Task<IEnumerable<Registration>> GetAllAsync(ObjectId userId, ObjectId eventId)
        {
            var builder = Builders<Registration>.Filter;
            var filter = builder.Empty;

            var searchFilters = GetSearchFilters(userId, eventId);
            if (searchFilters.Count > 0)
                filter = builder.And(searchFilters);

            var result = await Collection.Find(filter).ToListAsync();
            return result;
        }

        public async Task InsertAsync(Registration registration)
        {
            await Collection.InsertOneAsync(registration);
            return;
        }


        private List<FilterDefinition<Registration>> GetSearchFilters(ObjectId userId, ObjectId eventId)
        {
            var filterDefinitions = new List<FilterDefinition<Registration>>();
            var builder = Builders<Registration>.Filter;

            if (userId != ObjectId.Empty) 
                filterDefinitions.Add(builder.Eq(x => x.UserId, userId));

            if (eventId != ObjectId.Empty) 
                filterDefinitions.Add(builder.Eq(x => x.EventId, eventId));

            return filterDefinitions;
        }
    }
}
