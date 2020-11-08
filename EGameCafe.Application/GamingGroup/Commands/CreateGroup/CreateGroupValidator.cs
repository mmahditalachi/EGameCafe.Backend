using FluentValidation;


namespace EGameCafe.Application.GamingGroup.Commands.CreateGroup
{
    public class CreateGroupValidator : AbstractValidator<CreateGroupCommand>
    {
        public CreateGroupValidator()
        {
            RuleFor(x => x.GroupName)
                .MaximumLength(50).WithMessage("GroupName must not exceed 90 characters.")
                .NotEmpty().WithMessage("GroupName is required.");

            RuleFor(x => x.GroupType)
                .NotNull().WithMessage("GroupType is required.");
        }

    }
}
