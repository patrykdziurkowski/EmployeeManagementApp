using BusinessLogic.ViewModels;
using DataAccess.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IEmployeeValidatorFactory
    {
        IValidator<EmployeeDto> GetValidator(Type validatorType);
    }
}
