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
    public class UserRepository : BaseRepository<User>
    {
        public UserRepository(IUserCollectionProvider collectionProvider) : base(collectionProvider)
        {
        }

        public async Task DeleteAsync(ObjectId id)
        {
            var filter = Builders<User>.Filter
                .Eq(x => x.Id, id);

            var result = await Collection.DeleteOneAsync(filter);
        }

        public async Task<IEnumerable<User>> GetAllAsync(string name, string lastName, string email, string phoneNumber)
        {
            var builder = Builders<User>.Filter;
            var filter = builder.Empty;

            var searchFilters = GetSearchFilters(name, lastName, email, phoneNumber);
            if (searchFilters.Count > 0)
                filter = builder.And(searchFilters);

            var result = await Collection.Find(filter).ToListAsync();
            return result;
        }

        public async Task<User> GetAsync(ObjectId id)
        {
            var filter = Builders<User>.Filter.Eq(x => x.Id, id);

            var result = await Collection.Find(filter).FirstOrDefaultAsync();
            return result;
        }

        public async Task<User> UpsertAsync(User user)
        {
            var filter = Builders<User>.Filter.Eq(x => x.Id, user.Id);

            var update = Builders<User>.Update
                .SetOnInsert(x => x.Id, user.Id)
                .SetOnInsert(x => x.CreatedAt, user.CreatedAt)
                .Set(x => x.Name, user.Name)
                .Set(x => x.Lastname, user.Lastname)
                .Set(x => x.Email, user.Email)
                .Set(x => x.PhoneNumber, user.PhoneNumber)
                .Set(x => x.UpdatedAt, user.UpdatedAt);

            var result = await Collection.FindOneAndUpdateAsync(filter, update, new FindOneAndUpdateOptions<User>
            {
                IsUpsert = true,
                ReturnDocument = ReturnDocument.After
            });

            return result;
        }

        private List<FilterDefinition<User>> GetSearchFilters(string name, string lastName, string email, string phoneNumber)
        {
            var filterDefinitions = new List<FilterDefinition<User>>();
            var builder = Builders<User>.Filter;
            var regexOptions = RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.IgnorePatternWhitespace;

            if (!string.IsNullOrEmpty(name))
                filterDefinitions.Add(builder.Regex(x => x.Name, new(new Regex(name, regexOptions))));

            if (!string.IsNullOrEmpty(lastName))
                filterDefinitions.Add(builder.Regex(x => x.Lastname, new(new Regex(lastName, regexOptions))));

            if (!string.IsNullOrEmpty(email))
                filterDefinitions.Add(builder.Regex(x => x.Email, new(new Regex(email, regexOptions))));

            if (!string.IsNullOrEmpty(phoneNumber))
                filterDefinitions.Add(builder.Regex(x => x.PhoneNumber, new(new Regex(phoneNumber, regexOptions))));

            return filterDefinitions;
        }
    }
}
