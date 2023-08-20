using Calendarize.Core.Constants;
using Calendarize.Core.Dto;
using Calendarize.Core.Extensions;
using Calendarize.Core.Services;
using FluentValidation;

namespace Calendarize.API.Validation
{
    public class RegistrationValidator : AbstractValidator<RegistrationCreateDto>
    {
        private readonly UserService _userService;
        private readonly EventService _eventService;

        public RegistrationValidator(UserService userService, EventService eventService)
        {
            _userService = userService;
            _eventService = eventService;

            RuleFor(x => x.UserId)
                .NotEmpty()
                .Must(x => x.IsValidBsonObjectId())
                    .WithMessage(x => $"'{nameof(RegistrationCreateDto.UserId)}' is not a valid Id format")
                    .WithState(x => ValidationErrorStates.ValueInvalidFormat)
                .DependentRules(() => {
                    RuleFor(x => x.UserId)
                        .Must(BeValidUser)
                            .WithMessage(x => $"'{nameof(RegistrationCreateDto.UserId)}' with value {x.UserId} not found")
                            .WithState(x => ValidationErrorStates.EntityNotFound);
                });

            RuleFor(x => x.EventId)
                .NotEmpty()
                .Must(x => x.IsValidBsonObjectId())
                    .WithMessage(x => $"'{nameof(RegistrationCreateDto.EventId)}' is not a valid Id format")
                    .WithState(x => ValidationErrorStates.ValueInvalidFormat)
                .DependentRules(() => {
                    RuleFor(x => x.EventId)
                        .Must(BeValidEvent)
                            .WithMessage(x => $"'{nameof(RegistrationCreateDto.EventId)}' with value {x.EventId} not found")
                            .WithState(x => ValidationErrorStates.EntityNotFound);
                });
        }

        private bool BeValidEvent(string eventId)
        {
            var entity = _eventService.GetEventAsync(eventId).Result;
            return entity is not null;
        }

        private bool BeValidUser(string userId)
        {
            var entity = _userService.GetUserAsync(userId).Result;
            return entity is not null;
        }
    }
}
