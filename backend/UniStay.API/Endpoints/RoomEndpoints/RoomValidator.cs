using FluentValidation;
using UniStay.API.Data.Models.Dto.Room;

public class RoomCreateValidator : AbstractValidator<RoomCreateDTO>
{
    public RoomCreateValidator()
    {
        RuleFor(x => x.Floor)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Floor cannot be negative.");

        RuleFor(x => x.MaxOccupancy)
            .GreaterThan(0).WithMessage("Max occupancy must be atleast 1!")
            .LessThanOrEqualTo(2).WithMessage("Max occupancy cannot exceed 2!");

        //RuleFor(x => x.Description)
        //    .NotEmpty().WithMessage("Description is required.")
        //    .MinimumLength(3).WithMessage("Description must have at least 3 characters.");
    }
}