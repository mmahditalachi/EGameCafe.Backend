using FluentValidation;


namespace EGameCafe.Application.GroupMember.Queries.JoinViaInvitation
{
    public class JoinViaInvitationValidator : AbstractValidator<JoinViaInvitationQuery>
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
