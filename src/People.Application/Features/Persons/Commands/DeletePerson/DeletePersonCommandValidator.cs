using FluentValidation;

namespace People.Application.Features.Persons.Commands.DeletePerson;

public class DeletePersonCommandValidator : AbstractValidator<DeletePersonCommand>
{
    public DeletePersonCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
    }
}