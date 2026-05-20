using FluentValidation;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Features.ApplicationUser.Commands.Models;
using SchoolProject.Core.Resources;

namespace SchoolProject.Core.Features.ApplicationUser.Commands.Validatiors
{
    public class AddUserValidatior : AbstractValidator<AddUserCommand>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> stringLocalizer;
        #endregion

        #region Constructors
        public AddUserValidatior(IStringLocalizer<SharedResources> stringLocalizer)
        {
            this.stringLocalizer = stringLocalizer;

            ApplyValidationsResult();
            ApplayCustomValidationsResult();
        }
        #endregion

        #region Handle Functions
        public void ApplyValidationsResult()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage(stringLocalizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(stringLocalizer[SharedResourcesKeys.Required])
                .MaximumLength(100).WithMessage(stringLocalizer[SharedResourcesKeys.MaxLengthis100]);

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage(stringLocalizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(stringLocalizer[SharedResourcesKeys.Required])
                .MaximumLength(100).WithMessage(stringLocalizer[SharedResourcesKeys.MaxLengthis100]);

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(stringLocalizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(stringLocalizer[SharedResourcesKeys.Required]);

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(stringLocalizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(stringLocalizer[SharedResourcesKeys.Required]);

            RuleFor(x => x.ConfirmPassword)
               .Matches(x => x.Password).WithMessage(stringLocalizer[SharedResourcesKeys.PasswordNotEqualsConfirmPass]);
        }

        public void ApplayCustomValidationsResult()
        {

        }
        #endregion
    }
}
