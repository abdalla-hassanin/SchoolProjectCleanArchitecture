using FluentValidation;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Features.Students.Commands.Models;
using SchoolProject.Core.Resources;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Core.Features.Students.Commands.Validatiors
{
    public class AddStudentValidator : AbstractValidator<AddStudentCommand>
    {
        #region Fields
        private readonly IStudentService _studentService;
        private readonly IDepartmentService _departmentService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public AddStudentValidator(IStudentService studentService,
            IDepartmentService departmentService,
                                   IStringLocalizer<SharedResources> localizer
                             )
        {
            _studentService = studentService;
            _departmentService = departmentService;
            _localizer = localizer;
            ApplyValidationsRules();
            ApplyCustomValidationsRules();
        }
        #endregion

        #region Actions
        public void ApplyValidationsRules()
        {
            RuleFor(x => x.NameAr)
                 .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                 .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
                 .MaximumLength(100).WithMessage(_localizer[SharedResourcesKeys.MaxLengthis100]);

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
                .MaximumLength(100).WithMessage(_localizer[SharedResourcesKeys.MaxLengthis100]);

            RuleFor(x => x.DepartmementId)
                 .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                 .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required]);
        }

        public void ApplyCustomValidationsRules()
        {
            RuleFor(x => x.NameAr)
                .MustAsync(async (Key, CancellationToken) => !await _studentService.IsNameArExist(Key))
                .WithMessage(_localizer[SharedResourcesKeys.IsExist]);
            RuleFor(x => x.NameEn)
               .MustAsync(async (Key, CancellationToken) => !await _studentService.IsNameEnExist(Key))
               .WithMessage(_localizer[SharedResourcesKeys.IsExist]);

            RuleFor(x => x.DepartmementId)
               .MustAsync(async (Key, CancellationToken) => await _departmentService.IsDepartmentIdExist(Key))
               .WithMessage(_localizer[SharedResourcesKeys.IsNotExist]);

        }

        #endregion

    }
}
