using Calendarize.Core.Dto;
using Calendarize.Core.Services;
using FluentValidation;

namespace Calendarize.API.Validation
{
    public class EventValidator : AbstractValidator<EventCreateDto>
    {
        private readonly UserService _userService;
        private readonly LocationService _locationService;

        public EventValidator(UserService userService, LocationService locationService)
        {
            _userService = userService;
            _locationService = locationService;

            RuleFor(x => x.Capacity)
                .NotEmpty()
                .InclusiveBetween(1, 1000)
                .WithState(x => "invalidCapacityAmount");

            RuleFor(x => x.Description)
                .MaximumLength(500)
                .WithState(x => "invalidDescriptionLength");

            RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(100)
                .WithState(x => "invalidNameLength");

            RuleFor(x => x.Tags)
                .Must(x => x.Count < 5)
                .WithMessage("Cannot add more than 5 tags to event.")
                .WithState(x => "invalidTagCount");

            RuleFor(x => x.CoachId)
                .NotEmpty()
                .Must(IsValidUser)
                .WithMessage(x => $"'CoachId' with value '{x.CoachId}' not found")
                .WithState(x => "notFoundUserId");

            RuleFor(x => x.LocationId)
                .NotEmpty()
                .Must(IsValidLocation)
                .WithMessage(x => $"'LocationId' with value '{x.LocationId}' not found")
                .WithState(x => "notFoundLocationId");
        }

        private bool IsValidLocation(string locationId)
        {
            var location = _locationService.GetLocationAsync(locationId).Result;
            return !(location is null);
        }

        private bool IsValidUser(string userId)
        {
            var user = _userService.GetUserAsync(userId).Result;
            return !(user is null);
        }
    }
}
