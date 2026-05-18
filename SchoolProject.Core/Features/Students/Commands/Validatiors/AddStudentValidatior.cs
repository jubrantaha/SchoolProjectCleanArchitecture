using FluentValidation;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Features.Students.Commands.Models;
using SchoolProject.Core.Resources;
using SchoolProject.Service.Apstracts;

namespace SchoolProject.Core.Features.Students.Commands.Validatiors
{
    public class AddStudentValidatior : AbstractValidator<AddStudentCommand>
    {
        private readonly IStudentService studentService;
        private readonly IStringLocalizer<SharedResources> stringLocalizer;

        public AddStudentValidatior(IStudentService studentService, IStringLocalizer<SharedResources> stringLocalizer)
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
                .MustAsync(async (Key, CancellationToken) => !await studentService.IsNameArExist(Key)).WithMessage(stringLocalizer[SharedResourcesKeys.IsExist]);

            RuleFor(x => x.NameEn)
                .MustAsync(async (Key, CancellationToken) => !await studentService.IsNameEnExist(Key)).WithMessage(stringLocalizer[SharedResourcesKeys.IsExist]);
        }
    }
}
