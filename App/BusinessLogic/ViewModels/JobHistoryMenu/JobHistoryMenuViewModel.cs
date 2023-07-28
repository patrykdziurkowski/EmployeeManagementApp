using BusinessLogic.Commands;
using DataAccess.Repositories;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace BusinessLogic.ViewModels
{
    public class JobHistoryMenuViewModel : INotifyPropertyChanged
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
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

        public ICommand LoadJobHistoryCommand { get; }

        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public JobHistoryMenuViewModel(JobHistoryRepository jobHistoryRepository)
        {
            _jobHistory = new();

            LoadJobHistoryCommand = new LoadJobHistoryCommand(this, jobHistoryRepository);
        }
        
        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////


        ////////////////////////////////////////////
        //  Events and Data Binding
        ////////////////////////////////////////////
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void JobHistory_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("JobHistory"));
        }


    }
}
