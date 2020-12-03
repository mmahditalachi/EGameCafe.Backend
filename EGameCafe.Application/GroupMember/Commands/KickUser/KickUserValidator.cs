using FluentValidation;


namespace EGameCafe.Application.GroupMembers.Commands.KickUser
{
    public class KickUserValidator : AbstractValidator<KickUserCommand>
    {
        public KickUserValidator()
        {
            RuleFor(x => x.GroupId)
                .Length(36).WithMessage("GroupId must be 64 characters.")
                .NotEmpty().WithMessage("GroupId is required.");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.");
        }
    }
}
