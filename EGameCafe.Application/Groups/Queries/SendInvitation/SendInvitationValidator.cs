using FluentValidation;


namespace EGameCafe.Application.Groups.Queries.SendInvitation
{
    public class SendInvitationValidator : AbstractValidator<SendInvitationQuery>
    {
        public SendInvitationValidator()
        {
            RuleFor(x => x.GroupId)
              .Length(36).WithMessage("GroupId must be 64 characters.")
              .NotEmpty().WithMessage("GroupId is required.");
        }
    }
}
