using Calendarize.Core.Entities.Common;

namespace Calendarize.Core.Entities
{
    public class Location : BaseEntity
    {
        public string Name { get; set; }
        public decimal Longtitude { get; set; }
        public decimal Latitude { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
    }
}
