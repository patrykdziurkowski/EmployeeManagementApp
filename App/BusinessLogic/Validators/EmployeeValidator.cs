using BusinessLogic.ViewModels;
using FluentValidation;

namespace BusinessLogic.Validators
{
    public class EmployeeValidator : AbstractValidator<EmployeeViewModel>
    {
        public EmployeeValidator()
        {
            RuleFor(x => x.EmployeeId)
                .NotEmpty();

            RuleFor(x => x.FirstName)
                .NotEmpty();

            RuleFor(x => x.LastName)
                .NotEmpty();

            RuleFor(x => x.Email)
                .NotEmpty();

            RuleFor(x => x.PhoneNumber)
                .NotEmpty();

            RuleFor(x => x.HireDate)
                .NotEmpty();

            RuleFor(x => x.JobId)
                .NotEmpty();
        }
    }
}
