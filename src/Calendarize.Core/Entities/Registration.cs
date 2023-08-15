using Calendarize.Core.Entities.Common;
using MongoDB.Bson;

namespace Calendarize.Core.Entities
{
    public class Registration : BaseEntity
    {
        public ObjectId UserId { get; set; }
        public ObjectId EventId { get; set; }
    }
}
