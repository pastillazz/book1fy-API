using FluentValidation;

namespace Application.Users.Commands.Register;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.UserName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(255);

        // Strength rules live in the Password value object; this only bounds
        // the input so an oversized payload never reaches the hasher.
        RuleFor(x => x.Password)
            .NotEmpty()
            .MaximumLength(128);

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .MaximumLength(20)
            .Matches(@"^\+?[0-9\s\-()]+$")
            .WithMessage("Phone number contains invalid characters.");
    }
}
