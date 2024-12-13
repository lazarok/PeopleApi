using FluentValidation;

namespace People.Application.Features.Persons.Commands.CreatePerson;

public class CreatePersonCommandValidator : AbstractValidator<CreatePersonCommand>
{
    public CreatePersonCommandValidator()
    {
        RuleFor(x => x.Fullname)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MaximumLength(100).WithMessage("Fullname must not exceed 100 characters.");
        
        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Date of birth is required.")
            .GreaterThan(new DateTime(1900, 1, 1)).WithMessage("The date of birth must be greater than 1900-01-01");
        
        RuleFor(x => x.Email)
            .NotEmpty().EmailAddress().WithMessage("The email address must not be empty.")
            .EmailAddress().WithMessage("The email address must be a valid email address.");
        
        RuleFor(x => x.PhoneNumber)
            .Matches(@"^\+[1-9]{1}[0-9]{3,14}$")
            .WithMessage("The phone number must start with '+' followed by a country code and then the number.");
        
        RuleFor(x => x.Dni)
            .MaximumLength(50).WithMessage("Fullname must not exceed 50 characters.");
    }
}