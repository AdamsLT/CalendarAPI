using MongoDB.Bson;

namespace Calendarize.Core.Extensions
{
    public static class StringExtensions
    {
        public static bool IsValidBsonObjectId(this string value)
        {
            var isValid = ObjectId.TryParse(value, out _);
            return isValid;
        }
    }
}
