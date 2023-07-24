using BusinessLogic.ViewModels;
using FluentValidation;

namespace BusinessLogic.Validators
{
    public class EmployeeValidator : AbstractValidator<EmployeeViewModel>
    {
        public EmployeeValidator()
        {
            int salesDepartmentId = 80;

            RuleFor(x => x.EmployeeId)
                .NotEmpty()
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.FirstName)
                .MaximumLength(20);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(25);

            RuleFor(x => x.Email)
                .NotEmpty()
                .MaximumLength(25);

            RuleFor(x => x.PhoneNumber)
                .MaximumLength(20);

            RuleFor(x => x.HireDate)
                .NotEmpty();

            RuleFor(x => x.JobId)
                .NotEmpty()
                .MaximumLength(10);

            RuleFor(x => x.Salary)
                .GreaterThan(0)
                .Unless(x => x.Salary is null);

            RuleFor(x => x.CommissionPct)
                .Null()
                .Unless(x => x.DepartmentId == salesDepartmentId)
                .GreaterThan(0);

            RuleFor(x => x.ManagerId)
                .GreaterThan(0)
                .Unless(x => x.ManagerId is null);

            RuleFor(x => x.DepartmentId)
                .GreaterThan((short)1)
                .When(x => x.DepartmentId.HasValue);

        }
    }
}
