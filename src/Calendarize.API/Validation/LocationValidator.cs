using Calendarize.Core.Constants;
using Calendarize.Core.Dto;
using FluentValidation;

namespace Calendarize.API.Validation
{
    public class LocationValidator : AbstractValidator<LocationCreateDto>
    {
        public LocationValidator()
        {
            RuleFor(x => x.Latitude)
                .NotEmpty()
                    .WithState(x => ValidationErrorStates.Required)
                .InclusiveBetween(ValidationConstants.Location.MinLatitudeValue, ValidationConstants.Location.MaxLatitudeValue)
                    .WithState(x => ValidationErrorStates.ValueOutOfBounds)
                .PrecisionScale(ValidationConstants.Location.LatitudePrecision, ValidationConstants.Location.LatitudeScale, ignoreTrailingZeros:false)
                    .WithState(x => ValidationErrorStates.ValueInvalidFormat);

            RuleFor(x => x.Longtitude)
                .NotEmpty()
                    .WithState(x => ValidationErrorStates.Required)
                .InclusiveBetween(ValidationConstants.Location.MinLongtitudeValue, ValidationConstants.Location.MaxLongtitudeValue)
                    .WithState(x => ValidationErrorStates.ValueOutOfBounds)
                .PrecisionScale(ValidationConstants.Location.LongtitudePrecision, ValidationConstants.Location.LongtitudeScale, ignoreTrailingZeros:false)
                    .WithState(x => ValidationErrorStates.ValueInvalidFormat);

            RuleFor(x => x.Address)
                .NotEmpty()
                    .WithState(x => ValidationErrorStates.ValueEmpty)
                .MinimumLength(ValidationConstants.Location.MinAddressLength)
                    .WithState(x => ValidationErrorStates.ValueInvalidLength)
                .MaximumLength(ValidationConstants.Location.MaxAddressLength)
                    .WithState(x => ValidationErrorStates.ValueInvalidLength);

            RuleFor(x => x.Name)
                .NotEmpty()
                    .WithState(x => ValidationErrorStates.ValueEmpty)
                .MinimumLength(ValidationConstants.Location.MinNameLength)
                    .WithState(x => ValidationErrorStates.ValueInvalidLength)
                .MaximumLength(ValidationConstants.Location.MaxNameLength)
                    .WithState(x => ValidationErrorStates.ValueInvalidLength);

            RuleFor(x => x.City)
                .NotEmpty()
                    .WithState(x => ValidationErrorStates.ValueEmpty)
                .MinimumLength(ValidationConstants.Location.MinCityLength)
                    .WithState(x => ValidationErrorStates.ValueInvalidLength)
                .MaximumLength(ValidationConstants.Location.MaxCityLength)
                    .WithState(x => ValidationErrorStates.ValueInvalidLength);
        }
    }
}
