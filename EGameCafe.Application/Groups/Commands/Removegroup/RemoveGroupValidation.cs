using FluentValidation;


namespace EGameCafe.Application.Groups.Commands.Removegroup
{
    public class RemoveGroupValidation : AbstractValidator<RemoveGroupCommand>
    {
        public RemoveGroupValidation()
        {
            RuleFor(x => x.GroupId)
                .Length(36).WithMessage("GroupId must be 64 characters.")
                .NotEmpty().WithMessage("GroupId is required.");
        }
    }
}
