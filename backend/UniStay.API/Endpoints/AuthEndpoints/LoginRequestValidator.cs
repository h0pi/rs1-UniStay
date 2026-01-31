using FluentValidation;
using static UniStay.API.Endpoints.AuthEndpoints.AuthLoginEndpoint;

namespace UniStay.API.Endpoints.AuthEndpoints;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("The email address is not valid.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}
