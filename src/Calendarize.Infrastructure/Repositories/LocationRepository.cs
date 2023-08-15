using Calendarize.Core.Entities;
using Calendarize.Infrastructure.Providers;
using Calendarize.Infrastructure.Repositories.Common;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Calendarize.Infrastructure.Repositories
{
    public class LocationRepository : BaseRepository<Location>
    {
        public LocationRepository(ILocationCollectionProvider collectionProvider) : base(collectionProvider)
        {
        }

        public async Task DeleteAsync(ObjectId id)
        {
            var filter = Builders<Location>.Filter
                .Eq(x => x.Id, id);

            var result = await Collection.DeleteOneAsync(filter);
        }

        public async Task<IEnumerable<Location>> GetAllAsync(string name, string city, string address)
        {
            var builder = Builders<Location>.Filter;
            var filter = builder.Empty;

            var searchFilters = GetSearchFilters(name, city, address);
            if (searchFilters.Count > 0)
                filter = builder.And(searchFilters);

            var result = await Collection.Find(filter).ToListAsync();
            return result;
        }

        public async Task<Location> GetAsync(ObjectId id)
        {
            var filter = Builders<Location>.Filter.Eq(x => x.Id, id);

            var result = await Collection.Find(filter).FirstOrDefaultAsync();
            return result;
        }

        public async Task<Location> UpsertAsync(Location location)
        {
            var filter = Builders<Location>.Filter.Eq(x => x.Id, location.Id);

            var update = Builders<Location>.Update
                .SetOnInsert(x => x.Id, location.Id)
                .SetOnInsert(x => x.CreatedAt, location.CreatedAt)
                .Set(x => x.UpdatedAt, location.UpdatedAt)
                .Set(x => x.Name, location.Name)
                .Set(x => x.City, location.City)
                .Set(x => x.Address, location.Address)
                .Set(x => x.Longtitude, location.Longtitude)
                .Set(x => x.Latitude, location.Latitude);

            var result = await Collection.FindOneAndUpdateAsync(filter, update, new FindOneAndUpdateOptions<Location>
            {
                IsUpsert = true,
                ReturnDocument = ReturnDocument.After
            });

            return result;
        }

        private List<FilterDefinition<Location>> GetSearchFilters(string name, string city, string address)
        {
            var filterDefinitions = new List<FilterDefinition<Location>>();
            var builder = Builders<Location>.Filter;
            var regexOptions = RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.IgnorePatternWhitespace;

            if (!string.IsNullOrEmpty(name)) 
                filterDefinitions.Add(builder.Regex(x => x.Name, new (new Regex(name, regexOptions))));

            if (!string.IsNullOrEmpty(address)) 
                filterDefinitions.Add(builder.Regex(x => x.Address, new(new Regex(address, regexOptions))));

            if (!string.IsNullOrEmpty(city))
                filterDefinitions.Add(builder.Regex(x => x.City, new(new Regex(city, regexOptions))));

            return filterDefinitions;
        }
    }
}
