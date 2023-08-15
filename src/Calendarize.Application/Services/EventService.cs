using Calendarize.Core.Entities;
using Calendarize.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Calendarize.Core.Services
{
    public class EventService
    {
        private readonly EventRepository _eventRepository;

        public EventService(EventRepository eventRepository) 
        {
            _eventRepository = eventRepository;
        }

        public async Task<IEnumerable<Event>> GetEventsAsync(
            DateTime? startDate, DateTime? endDate, string locationId, string coachId)
        {
            var locationSearch = ObjectId.Empty;
            var coachSearch = ObjectId.Empty;

            if (!string.IsNullOrWhiteSpace(locationId))
                locationSearch = new ObjectId(locationId);

            if (!string.IsNullOrWhiteSpace(coachId))
                coachSearch = new ObjectId(coachId);

            return await _eventRepository.GetAllAsync(startDate, endDate, locationSearch, coachSearch);
        }

        public async Task<Event> GetEventAsync(string id)
        {
            var objectId = new ObjectId(id);
            return await _eventRepository.GetAsync(objectId);
        }

        public async Task<Event> CreateEventAsync(Event createEvent)
        {
            createEvent.Id = ObjectId.GenerateNewId();
            var dateTime = DateTime.UtcNow;
            createEvent.CreatedAt = dateTime;
            createEvent.UpdatedAt = dateTime;

            return await _eventRepository.UpsertAsync(createEvent);
        }

        public async Task DeleteEventAsync(string id)
        {
            var objectId = new ObjectId(id);
            await _eventRepository.DeleteAsync(objectId);
        }

        public async Task<Event> UpdateEventAsync(Event updateEvent)
        {
            var dateTime = DateTime.UtcNow;
            updateEvent.UpdatedAt = dateTime;

            return await _eventRepository.UpsertAsync(updateEvent);
        }
    }
}
