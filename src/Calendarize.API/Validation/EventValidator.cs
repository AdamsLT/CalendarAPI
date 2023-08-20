using Calendarize.Core.Constants;
using Calendarize.Core.Dto;
using Calendarize.Core.Extensions;
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
                .InclusiveBetween(ValidationConstants.Event.MinCapacity, ValidationConstants.Event.MaxCapacity)
                    .WithState(x => ValidationErrorStates.Required);

            RuleFor(x => x.Description)
                .MaximumLength(ValidationConstants.Event.MaxDescriptionLength)
                    .WithState(x => ValidationErrorStates.ValueInvalidLength);

            RuleFor(x => x.Name)
                .NotEmpty()
                    .WithState(x => ValidationErrorStates.ValueEmpty)
                .MinimumLength(ValidationConstants.Event.MinNameLength)
                    .WithState(x => ValidationErrorStates.ValueInvalidLength)
                .MaximumLength(ValidationConstants.Event.MaxNameLength)
                    .WithState(x => ValidationErrorStates.ValueInvalidLength);

            RuleFor(x => x.Tags)
                .Must(x => x.Count <= ValidationConstants.Event.MaxTagCount)
                    .WithMessage(x => $"Maximum amount of tags is {ValidationConstants.Event.MaxTagCount}. You entered {x.Tags.Count}")
                    .WithState(x => ValidationErrorStates.Event.InvalidTagCount);

            RuleFor(x => x.CoachId)
                .NotEmpty()
                .Must(x => x.IsValidBsonObjectId())
                    .WithMessage(x => $"'{nameof(EventCreateDto.CoachId)}' is not a valid Id format")
                    .WithState(x => ValidationErrorStates.ValueInvalidFormat)
                .DependentRules(() => {
                    RuleFor(x => x.CoachId)
                        .Must(BeValidUser)
                            .WithMessage(x => $"'{nameof(EventCreateDto.CoachId)}' with value {x.CoachId} not found")
                            .WithState(x => ValidationErrorStates.EntityNotFound);
                });

            RuleFor(x => x.LocationId)
                .NotEmpty()
                .Must(x => x.IsValidBsonObjectId())
                    .WithMessage(x => $"'{nameof(EventCreateDto.LocationId)}' is not a valid Id format")
                    .WithState(x => ValidationErrorStates.ValueInvalidFormat)
                .DependentRules(() => {
                    RuleFor(x => x.LocationId)
                        .Must(BeValidLocation)
                            .WithMessage(x => $"'{nameof(EventCreateDto.LocationId)}' with value {x.LocationId} not found")
                            .WithState(x => ValidationErrorStates.EntityNotFound);
                });
        }

        private bool BeValidLocation(string locationId)
        {
            var entity = _locationService.GetLocationAsync(locationId).Result;
            return entity is not null;
        }

        private bool BeValidUser(string userId)
        {
            var entity = _userService.GetUserAsync(userId).Result;
            return entity is not null;
        }
    }
}
