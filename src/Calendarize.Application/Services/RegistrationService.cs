using Calendarize.Core.Entities;
using Calendarize.Infrastructure.Repositories;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Calendarize.Core.Services
{
    public class RegistrationService
    {
        private readonly RegistrationRepository _registrationRepository;

        public RegistrationService(RegistrationRepository registrationRepository) 
        {
            _registrationRepository = registrationRepository;
        }

        public async Task<IEnumerable<Registration>> GetRegistrationsAsync(string userId, string eventId)
        {
            var userSearch = ObjectId.Empty;
            var eventSearch = ObjectId.Empty;

            if (!string.IsNullOrWhiteSpace(userId))
                userSearch = new ObjectId(userId);

            if (!string.IsNullOrWhiteSpace(eventId))
                eventSearch = new ObjectId(eventId);

            return await _registrationRepository.GetAllAsync(userSearch, eventSearch);
        }

        public async Task DeleteRegistrationAsync(string id)
        {
            var objectId = new ObjectId(id);
            await _registrationRepository.DeleteAsync(objectId);
        }

        public async Task CreateRegistrationAsync(Registration registration)
        {
            registration.Id = ObjectId.GenerateNewId();
            var dateTime = DateTime.UtcNow;
            registration.CreatedAt = dateTime;
            registration.UpdatedAt = dateTime;

            await _registrationRepository.InsertAsync(registration);
            return;
        }
    }
}
