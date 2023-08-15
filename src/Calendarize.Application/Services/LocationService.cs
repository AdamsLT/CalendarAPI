using Calendarize.Core.Entities;
using Calendarize.Infrastructure.Repositories;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Calendarize.Core.Services
{
    public class LocationService
    {
        private readonly LocationRepository _locationRepository;

        public LocationService(LocationRepository locationRepository) 
        {
            _locationRepository = locationRepository;
        }

        public async Task<IEnumerable<Location>> GetLocationsAsync(string name, string city, string address)
        {
            return await _locationRepository.GetAllAsync(name, city, address);
        }

        public async Task<Location> GetLocationAsync(string id)
        {
            var objectId = new ObjectId(id);
            return await _locationRepository.GetAsync(objectId);
        }

        public async Task<Location> CreateLocationAsync(Location location)
        {
            location.Id = ObjectId.GenerateNewId();
            var dateTime = DateTime.UtcNow;
            location.CreatedAt = dateTime;
            location.UpdatedAt = dateTime;

            return await _locationRepository.UpsertAsync(location);
        }

        public async Task DeleteLocationAsync(string id)
        {
            var objectId = new ObjectId(id);
            await _locationRepository.DeleteAsync(objectId);
        }

        public async Task<Location> UpdateLocationAsync(Location location)
        {
            var dateTime = DateTime.UtcNow;
            location.UpdatedAt = dateTime;

            return await _locationRepository.UpsertAsync(location);
        }
    }
}
