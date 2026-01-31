    using FluentValidation;
    using UniStay.API.Data.Models.Dto.Equipment;

namespace UniStay.API.Endpoints.EquipmentEndpoints
{
    public class EquipmentCreateValidator : AbstractValidator<EquipmentCreateDto>
    {
        public EquipmentCreateValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required").MinimumLength(2);
            RuleFor(x => x.Quantity).GreaterThanOrEqualTo(0);
            RuleFor(x => x.AvailableQuantity).GreaterThanOrEqualTo(0)
                  .LessThanOrEqualTo(x => x.Quantity).WithMessage("AvailableQuantity must be <= Quantity");
        }
    }

    public class EquipmentUpdateValidator : AbstractValidator<EquipmentUpdateDto>
    {
        public EquipmentUpdateValidator()
        {
            RuleFor(x => x.Quantity).GreaterThanOrEqualTo(0);
            RuleFor(x => x.AvailableQuantity).GreaterThanOrEqualTo(0)
                  .LessThanOrEqualTo(x => x.Quantity);
        }
    }
}
