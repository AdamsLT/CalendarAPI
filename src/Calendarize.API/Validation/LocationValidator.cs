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
                .InclusiveBetween(-90, 90)
                .PrecisionScale(9, 7, ignoreTrailingZeros:false)
                .WithState(x => "invalidLatitudeValue");

            RuleFor(x => x.Longtitude)
                .NotEmpty()
                .InclusiveBetween(-180, 180)
                .PrecisionScale(10, 7, ignoreTrailingZeros:false)
                .WithState(x => "invalidLongtitudeValue");

            RuleFor(x => x.Address)
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(50)
                .WithState(x => "invalidAddressLength");

            RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(50)
                .WithState(x => "invalidNameLength");

            RuleFor(x => x.City)
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(50)
                .WithState(x => "invalidCityLength");
        }
    }
}
