using FluentValidation;


namespace EGameCafe.Application.GroupMembers.Commands.JoinViaInvitation
{
    public class JoinViaInvitationValidator : AbstractValidator<JoinViaInvitationCommand>
    {
        public JoinViaInvitationValidator()
        {
            RuleFor(x => x.token)
                .NotEmpty().WithMessage("GroupId is required.");
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.");
        }
    }
}
