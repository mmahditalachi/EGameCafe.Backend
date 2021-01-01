using FluentValidation;


namespace EGameCafe.Application.Activities.Commands.CreateActivity
{
    public class CreateActivityValidator : AbstractValidator<CreateActivityCommand>
    {
        public CreateActivityValidator()
        {
            RuleFor(x => x.ActivityTitle)
                .MaximumLength(50).WithMessage("GroupName must not exceed 90 characters.")
                .NotEmpty().WithMessage("GroupName is required.");

            RuleFor(x => x.ActivityText)
                .MaximumLength(150).WithMessage("GroupName must not exceed 90 characters.")
                .NotNull().WithMessage("GroupType is required.");
        }
    }
}
