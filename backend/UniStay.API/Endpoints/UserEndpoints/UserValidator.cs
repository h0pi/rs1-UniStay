using FluentValidation;
using UniStay.API.Data.Models.Dto.User;

namespace UniStay.API.Endpoints.UserEndpoints
{
    public class UserCreateValidator : AbstractValidator<UserCreateDTO>
    {
        public UserCreateValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
                
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MinimumLength(2).WithMessage("First name must have at least 2 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MinimumLength(2).WithMessage("Last name must have at least 2 characters.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Phone is required.")
                .Matches(@"^\d{6,15}$").WithMessage("Phone must be digits only (6–15 digits).");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must have at least 6 characters.");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Date of birth is required.")
                .LessThan(DateTime.Now).WithMessage("Date of birth cannot be in the future.");

            RuleFor(x => x.Username)
                .MinimumLength(3)
                .When(x => !string.IsNullOrWhiteSpace(x.Username))
                .WithMessage("Username must have at least 3 characters.");

            RuleFor(x => x.RoleID)
                .GreaterThan(0)
                .When(x => x.RoleID.HasValue)
                .WithMessage("RoleID must be greater than 0.");
        }
    }
}
