using BusinessLogic.ViewModels;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BusinessLogic.Commands
{
    public class LoadDepartmentsCommand : ICommand
    {
        private DepartmentsMenuViewModel _viewModel;
        private DepartmentRepository _departmentRepository;
        private EmployeeRepository _employeeRepository;

        public event EventHandler? CanExecuteChanged;

        public LoadDepartmentsCommand(
            DepartmentsMenuViewModel viewModel,
            DepartmentRepository departmentRepository,
            EmployeeRepository employeeRepository)
        {
            _viewModel = viewModel;
            _departmentRepository = departmentRepository;
            _employeeRepository = employeeRepository;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public async void Execute(object? parameter)
        {
            List<DepartmentDto> departmentDtos = (await _departmentRepository.GetAllAsync()).ToListOfDepartmentDto();
            ObservableCollection<DepartmentDto> departments = new(departmentDtos);

            List<EmployeeDto> employeeDtos = (await _employeeRepository.GetAllAsync()).ToListOfEmployeeDto();
            ObservableCollection<EmployeeDto> employees = new(employeeDtos);

            _viewModel.Employees = employees;
            _viewModel.Departments = departments;

            foreach (DepartmentDto department in _viewModel.Departments)
            {
                List<EmployeeDto> departmentEmployeesDtos = (await _departmentRepository.GetEmployeesForDepartmentAsync(department.DepartmentId)).ToListOfEmployeeDto();
                department.Employees = new(departmentEmployeesDtos);

                department.Employees.CollectionChanged += _viewModel.Employees_CollectionChanged;
            }

            _viewModel.Employees.CollectionChanged += _viewModel.Employees_CollectionChanged;
            _viewModel.Departments.CollectionChanged += _viewModel.Departments_CollectionChanged;

            foreach (EmployeeDto employee in _viewModel.Employees)
            {
                employee.PropertyChanged += _viewModel.EmployeeUpdated;
            }
        }
    }
}
