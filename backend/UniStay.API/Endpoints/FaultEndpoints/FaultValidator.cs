using FluentValidation;
using UniStay.API.Data.Models.Dto.Fault;

public class FaultCreateDtoValidator : AbstractValidator<FaultCreateDto>
{
    public FaultCreateDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("Naslov je obavezan").MinimumLength(3);
        RuleFor(x => x.Description).MaximumLength(2000);
    }
}

public class FaultUpdateDtoValidator : AbstractValidator<FaultUpdateDto>
{
    public FaultUpdateDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MinimumLength(3);
        RuleFor(x => x.Description).MaximumLength(2000);
        RuleFor(x => x.Status).NotEmpty().MinimumLength(3);
        RuleFor(x => x.Priority).NotEmpty().MinimumLength(3);
        RuleFor(x => x).Must(x => x.IsResolved!=true || x.ResolvedAt != null)
            .WithMessage("Ako je označeno riješeno, mora postojati datum rješavanja.");
    }
}