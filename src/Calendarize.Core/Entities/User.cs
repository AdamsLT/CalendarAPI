using Calendarize.Core.Entities.Common;

namespace Calendarize.Core.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
