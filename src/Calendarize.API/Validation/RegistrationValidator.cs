using Calendarize.Core.Dto;
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

            RuleFor(x => x.EventId)
                .NotEmpty()
                .WithState(x => "invalidEventId");

            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithState(x => "invalidUserId");

            RuleFor(x => x.UserId)
                .NotEmpty()
                .Must(IsValidUser)
                .WithMessage(x => $"'UserId' with value '{x.UserId}' not found")
                .WithState(x => "notFoundUserId");

            RuleFor(x => x.EventId)
                .NotEmpty()
                .Must(IsValidEvent)
                .WithMessage(x => $"'EventId' with value '{x.EventId}' not found")
                .WithState(x => "notFoundEventId");
        }

        private bool IsValidEvent(string eventId)
        {
            var @event = _eventService.GetEventAsync(eventId).Result;
            return !(@event is null);
        }

        private bool IsValidUser(string userId)
        {
            var user = _userService.GetUserAsync(userId).Result;
            return !(user is null);
        }
    }
}
