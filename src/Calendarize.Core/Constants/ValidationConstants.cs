namespace Calendarize.Core.Constants
{
    public static class ValidationConstants
    {
        public static class Event
        {
            public const int MinCapacity = 1;
            public const int MaxCapacity = 1000;

            public const int MaxDescriptionLength = 500;

            public const int MinNameLength = 1;
            public const int MaxNameLength = 100;

            public const int MaxTagCount = 5;
        }

        public static class Location
        {
            public const int MinCityLength = 1;
            public const int MaxCityLength = 50;

            public const int MinAddressLength = 1;
            public const int MaxAddressLength = 50;

            public const int MinNameLength = 1;
            public const int MaxNameLength = 100;

            public const int MinLongtitudeValue = -180;
            public const int MaxLongtitudeValue = 180;
            public const int LongtitudePrecision = 10;
            public const int LongtitudeScale = 7;

            public const int MinLatitudeValue = -90;
            public const int MaxLatitudeValue = 90;
            public const int LatitudePrecision = 9;
            public const int LatitudeScale = 7;
        }

        public static class User
        {
            public const int MinNameLength = 1;
            public const int MaxNameLength = 100;

            public const int MinLastnameLength = 1;
            public const int MaxLastnameLength = 100;

            public const int MinPhoneNumberLength = 9;
            public const int MaxPhoneNumberLength = 12;
        }

    }
}
