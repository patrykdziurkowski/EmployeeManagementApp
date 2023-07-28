using BusinessLogic.ViewModels;
using DataAccess.Models;
using DataAccess.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Validators
{
    public class CommissionPctValidator : AbstractValidator<EmployeeDto>
    {
        private DepartmentRepository _departmentRepository;

        public CommissionPctValidator(DepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;

            RuleFor(x => x.CommissionPct)
                .Null()
                .UnlessAsync(async (employee, cancellationToken) =>
                    {
                        return await IsSalesDepartment(employee);
                    })
                .WithMessage("An employee from a department other than Sales cannot have a commission");
        }

        private async Task<bool> IsSalesDepartment(EmployeeDto employee)
        {
            IEnumerable<Department> departments = await _departmentRepository.GetAllAsync();
            short? salesDepartmentId = departments
                                            .FirstOrDefault(department => department.DepartmentName == "Sales")?
                                            .DepartmentId;
            if (salesDepartmentId is null)
            {
                return false;
            }

            if (employee.DepartmentId != salesDepartmentId)
            {
                return false;
            }
            return true;
        }
    }
}
