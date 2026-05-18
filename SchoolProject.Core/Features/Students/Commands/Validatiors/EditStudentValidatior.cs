using FluentValidation;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Features.Students.Commands.Models;
using SchoolProject.Core.Resources;
using SchoolProject.Service.Apstracts;

namespace SchoolProject.Core.Features.Students.Commands.Validatiors
{
    public class EditStudentValidatior : AbstractValidator<EditStudentCommand>
    {
        private readonly IStudentService studentService;
        private readonly IStringLocalizer<SharedResources> stringLocalizer;

        public EditStudentValidatior(IStudentService studentService, IStringLocalizer<SharedResources> stringLocalizer)
        {
            this.studentService = studentService;
            this.stringLocalizer = stringLocalizer;
            ApplyValidationsResult();
            ApplayCustomValidationsResult();
        }

        public void ApplyValidationsResult()
        {
            RuleFor(x => x.NameAr)
                .NotEmpty().WithMessage(stringLocalizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(stringLocalizer[SharedResourcesKeys.Required])
                .MaximumLength(100).WithMessage(stringLocalizer[SharedResourcesKeys.MaxLengthis100]);

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage(stringLocalizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(stringLocalizer[SharedResourcesKeys.Required])
                .MaximumLength(100).WithMessage(stringLocalizer[SharedResourcesKeys.MaxLengthis100]);
        }

        public void ApplayCustomValidationsResult()
        {

            RuleFor(x => x.NameAr)
                .MustAsync(async (model, Key, CancellationToken) => !await studentService.IsNameArExistExcludeSelf(Key, model.Id)).WithMessage(stringLocalizer[SharedResourcesKeys.IsExist]);

            RuleFor(x => x.NameEn)
                .MustAsync(async (model, Key, CancellationToken) => !await studentService.IsNameEnExistExcludeSelf(Key, model.Id)).WithMessage(stringLocalizer[SharedResourcesKeys.IsExist]);

        }
    }
}

