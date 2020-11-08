using FluentValidation;


namespace EGameCafe.Application.GroupMember.Commands.LeaveGroup
{
    public class LeaveGroupValidator : AbstractValidator<LeaveGroupCommand>
    {
        public LeaveGroupValidator()
        {
            RuleFor(x => x.GroupId)
                .Length(64).WithMessage("GroupId must be 64 characters.")
                .NotEmpty().WithMessage("GroupId is required.");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.");
        }
    }
}
