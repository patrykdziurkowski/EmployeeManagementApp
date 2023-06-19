using Models;
using Models.Entities;
using Models.Repositories;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ViewModels
{
    public class JobHistoryMenuViewModel : INotifyPropertyChanged
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
        private JobHistoryRepository _jobHistoryRepository;
        private LoginViewModel _loginViewModel;

        private readonly ObservableCollection<JobHistoryViewModel> _jobHistory;
        public ObservableCollection<JobHistoryViewModel> JobHistory
        {
            get
            {
                return _jobHistory;
            }
        }
        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public JobHistoryMenuViewModel()
        {
            _loginViewModel = LoginViewModel.GetInstance();
            ConnectionStringProvider provider = new ConnectionStringProvider();
            string connectionString = provider
                .GetConnectionString(_loginViewModel.UserName, _loginViewModel.Password);
            _jobHistoryRepository = new(new OracleSQLDataAccess(connectionString));

            _jobHistory = new ObservableCollection<JobHistoryViewModel>();
            List<JobHistoryViewModel> jobHistoryViewModels = JobHistoryViewModel
                .ToListOfJobHistoryViewModel(_jobHistoryRepository.GetAll());
            ObservableCollection<JobHistoryViewModel> jobHistory = new ObservableCollection<JobHistoryViewModel>(jobHistoryViewModels);

            _jobHistory = jobHistory;
            _jobHistory.CollectionChanged += JobHistory_CollectionChanged;
        }
        ////////////////////////////////////////////
        //  Events and Data Binding
        ////////////////////////////////////////////
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void JobHistory_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("JobHistory"));
        }


    }
}
