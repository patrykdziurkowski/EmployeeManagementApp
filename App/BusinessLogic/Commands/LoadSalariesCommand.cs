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
    public class LoadSalariesCommand : ICommand
    {
        private SalariesMenuViewModel _viewModel;
        private EmployeeRepository _employeeRepository;

        public event EventHandler? CanExecuteChanged;

        public LoadSalariesCommand(
            SalariesMenuViewModel viewModel,
            EmployeeRepository employeeRepository)
        {
            _viewModel = viewModel;
            _employeeRepository = employeeRepository;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public async void Execute(object? parameter)
        {
            List<SalaryDto> salaryDtos = (await _employeeRepository.GetAllAsync()).ToListOfSalaryDto();
            ObservableCollection<SalaryDto> salaries = new(salaryDtos);
            _viewModel.Salaries = salaries;
            _viewModel.Salaries.CollectionChanged += _viewModel.Salaries_CollectionChanged;
        }
    }
}
