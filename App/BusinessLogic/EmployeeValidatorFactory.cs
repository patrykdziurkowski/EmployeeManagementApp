using BusinessLogic.Interfaces;
using BusinessLogic.Validators;
using BusinessLogic.ViewModels;
using DataAccess.Models;
using DataAccess.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class EmployeeValidatorFactory : IEmployeeValidatorFactory
    {
        private DepartmentRepository _departmentRepository;

        public EmployeeValidatorFactory(DepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public IValidator<EmployeeDto> GetValidator(Type validatorType)
        {
            if (validatorType == typeof(CommissionPctValidator))
            {
                return new CommissionPctValidator(_departmentRepository);
            }
            else if (validatorType == typeof(EmployeeValidator))
            {
                return new EmployeeValidator();
            }    
            
            throw new NotImplementedException("No such validator type is implemented.");
        }
    }
}
