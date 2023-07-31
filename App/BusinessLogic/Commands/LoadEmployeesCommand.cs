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
    public class LoadEmployeesCommand : ICommand
    {
        private readonly EmployeesMenuViewModel _viewModel;
        private readonly EmployeeRepository _employeeRepository;
        private readonly JobRepository _jobRepository;


        public event EventHandler? CanExecuteChanged;

        public LoadEmployeesCommand(
            EmployeesMenuViewModel viewModel,
            EmployeeRepository employeeRepository,
            JobRepository jobRepository)
        {
            _viewModel = viewModel;
            _employeeRepository = employeeRepository;
            _jobRepository = jobRepository;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public async void Execute(object? parameter)
        {
            _viewModel.NewEmployeeAlreadyExists = false;

            List<EmployeeDto> employeeDtos = (await _employeeRepository.GetAllAsync()).ToListOfEmployeeDto();
            ObservableCollection<EmployeeDto> employees = new(employeeDtos);
            _viewModel.Employees = employees;
            _viewModel.Employees.CollectionChanged += _viewModel.Employees_CollectionChanged;

            List<JobDto> jobDtos = (await _jobRepository.GetAllAsync()).ToListOfJobDto();
            ObservableCollection<string> jobs = new(jobDtos.Select(job => job.JobId));
            _viewModel.Jobs = jobs;
            _viewModel.Jobs.CollectionChanged += _viewModel.Jobs_CollectionChanged;

            foreach (EmployeeDto employee in _viewModel.Employees)
            {
                employee.PropertyChanging += _viewModel.EmployeeUpdating;
                employee.PropertyChanged += _viewModel.EmployeeUpdated;
            }

        }
    }
}
