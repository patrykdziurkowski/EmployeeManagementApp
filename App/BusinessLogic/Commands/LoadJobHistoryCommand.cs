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
    public class LoadJobHistoryCommand : ICommand
    {
        private JobHistoryMenuViewModel _viewModel;
        private JobHistoryRepository _jobHistoryRepository;

        public event EventHandler? CanExecuteChanged;

        public LoadJobHistoryCommand(
            JobHistoryMenuViewModel viewModel,
            JobHistoryRepository jobHistoryRepository)
        {
            _viewModel = viewModel;
            _jobHistoryRepository = jobHistoryRepository;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public async void Execute(object? parameter)
        {
            List<JobHistoryDto> jobHistoryDtos = (await _jobHistoryRepository.GetAllAsync()).ToListOfJobHistoryDto();
            ObservableCollection<JobHistoryDto> jobHistory = new(jobHistoryDtos);

            _viewModel.JobHistory = jobHistory;
            _viewModel.JobHistory.CollectionChanged += _viewModel.JobHistory_CollectionChanged;
        }
    }
}
