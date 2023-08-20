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
                    .WithState(x => "invalidUserId")
                .Must(x => x.IsValidBsonObjectId())
                    .WithMessage(x => $"'UserId' is not a valid ID")
                    .WithState(x => "invalidUserId")
                .DependentRules(() => {
                    RuleFor(x => x.UserId)
                        .Must(BeValidUser)
                            .WithMessage(x => $"'UserId' with value '{x.UserId}' not found")
                            .WithState(x => "notFoundUserId");
                              });

            RuleFor(x => x.EventId)
                .NotEmpty()
                    .WithState(x => "invalidEventId")
                .Must(x => x.IsValidBsonObjectId())
                    .WithMessage(x => $"'EventId' is not a valid ID")
                    .WithState(x => "invalidEventId")
                .DependentRules(() => {
                    RuleFor(x => x.UserId)
                        .Must(BeValidEvent)
                            .WithMessage(x => $"'EventId' with value '{x.EventId}' not found")
                            .WithState(x => "notFoundEventId");
                }) ;
        }

        private bool BeValidEvent(string eventId)
        {
            var @event = _eventService.GetEventAsync(eventId).Result;
            return !(@event is null);
        }

        private bool BeValidUser(string userId)
        {
            var user = _userService.GetUserAsync(userId).Result;
            return !(user is null);
        }
    }
}
