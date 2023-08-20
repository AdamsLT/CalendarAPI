using Calendarize.Core.Constants;
using Calendarize.Core.Dto;
using FluentValidation;

namespace Calendarize.API.Validation
{
    public class UserValidator : AbstractValidator<UserCreateDto>
    {
        public UserValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                    .WithState(x => ValidationErrorStates.ValueEmpty)
                .MinimumLength(ValidationConstants.User.MinNameLength)
                    .WithState(x => ValidationErrorStates.ValueInvalidLength)
                .MaximumLength(ValidationConstants.User.MaxNameLength)
                    .WithState(x => ValidationErrorStates.ValueInvalidLength);

            RuleFor(x => x.Lastname)
                .NotEmpty()
                    .WithState(x => ValidationErrorStates.ValueEmpty)
                .MinimumLength(ValidationConstants.User.MinLastnameLength)
                    .WithState(x => ValidationErrorStates.ValueInvalidLength)
                .MaximumLength(ValidationConstants.User.MaxLastnameLength)
                    .WithState(x => ValidationErrorStates.ValueInvalidLength);

            RuleFor(x => x.Email)
                .NotEmpty()
                    .WithState(x => ValidationErrorStates.ValueEmpty)
                .EmailAddress()
                    .WithState(x => ValidationErrorStates.ValueInvalidFormat);

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                    .WithState(x => ValidationErrorStates.ValueEmpty)
                .MinimumLength(ValidationConstants.User.MinPhoneNumberLength)
                    .WithState(x => ValidationErrorStates.ValueInvalidLength)
                .MaximumLength(ValidationConstants.User.MaxPhoneNumberLength)
                    .WithState(x => ValidationErrorStates.ValueInvalidLength);
        }
    }
}
