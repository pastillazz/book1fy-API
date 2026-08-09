using FluentValidation;

namespace Application.Users.Commands.Login;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        // Deliberately shallow: anything beyond "was something sent?" would
        // let a caller probe which half of the credentials was wrong.
        RuleFor(x => x.Email)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(x => x.Password)
            .NotEmpty()
            .MaximumLength(128);
    }
}
