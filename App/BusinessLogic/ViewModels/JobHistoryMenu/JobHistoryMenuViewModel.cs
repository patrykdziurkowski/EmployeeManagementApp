using DataAccess.Repositories;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BusinessLogic.ViewModels
{
    public class JobHistoryMenuViewModel : INotifyPropertyChanged
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
        private JobHistoryRepository _jobHistoryRepository;

        private ObservableCollection<JobHistoryDto> _jobHistory;
        public ObservableCollection<JobHistoryDto> JobHistory
        {
            get
            {
                return _jobHistory;
            }
            set
            {
                _jobHistory = value;
                OnPropertyChanged();
            }
        }
        
        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public JobHistoryMenuViewModel(JobHistoryRepository jobHistoryRepository)
        {
            _jobHistoryRepository = jobHistoryRepository;

            _jobHistory = new ObservableCollection<JobHistoryDto>();
        }
        
        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////
        public async Task InitializeDataAsync()
        {
            List<JobHistoryDto> jobHistoryViewModels = (await _jobHistoryRepository.GetAllAsync()).ToListOfJobHistoryViewModel();
            ObservableCollection<JobHistoryDto> jobHistory = new ObservableCollection<JobHistoryDto>(jobHistoryViewModels);

            JobHistory = jobHistory;
            JobHistory.CollectionChanged += JobHistory_CollectionChanged;
        }


        ////////////////////////////////////////////
        //  Events and Data Binding
        ////////////////////////////////////////////
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void JobHistory_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("JobHistory"));
        }


    }
}
