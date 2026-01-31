using FluentValidation;
using UniStay.API.Dto.Hall;

namespace UniStay.API.Endpoints.Hall
{
    public class HallValidator : AbstractValidator<HallCreateDto>
    {
        public HallValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.Capacity).InclusiveBetween(1, 1000).WithMessage("Capacity must be between 1 and 1000.");
            RuleFor(x => x.AvailableTo)
                .GreaterThan(x => x.AvailableFrom)
                .WithMessage("End date must be after the start date.");
        }
    }
}