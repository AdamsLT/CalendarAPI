using Calendarize.Core.Constants;
using System;
using System.Collections.Generic;

namespace Calendarize.Core.Dto
{
    public class EventDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartsAt { get; set; }
        public DateTime EndsAt { get; set; }
        public string CoachId { get; set; }
        public string LocationId { get; set; }
        public int Capacity { get; set; }
        public ISet<EventTags> Tags { get; set; }
    }
}
