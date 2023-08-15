using Calendarize.Core.Constants;
using Calendarize.Core.Entities.Common;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace Calendarize.Core.Entities
{
    public class Event : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartsAt { get; set; }
        public DateTime EndsAt { get; set; }
        public ObjectId CoachId { get; set; }
        public ObjectId LocationId { get; set; }
        public int Capacity { get; set; }
        public ISet<EventTags> Tags { get; set; }
    }
}
