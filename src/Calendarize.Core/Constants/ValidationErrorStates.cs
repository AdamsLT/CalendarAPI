namespace Calendarize.Core.Constants
{
    public static class ValidationErrorStates
    {
        public const string Required = "required";
        
        public const string ValueEmpty = "valueEmpty";
        public const string ValueOutOfBounds = "valueOutOfBounds";
        public const string ValueInvalidLength = "valueInvalidLength";
        public const string ValueInvalidFormat = "valueInvalidFormat";

        public const string EntityNotFound = "entityNotFound";
        
        public const string EnumValueIsNotSupported = "enumValueIsNotSupported";

        public static class Event
        {
            public const string InvalidTagCount = "invalidTagCount";
        }

    }
}
