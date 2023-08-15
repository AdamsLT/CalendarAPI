using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Calendarize.Core.Entities.Common
{
    public class BaseEntity
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
