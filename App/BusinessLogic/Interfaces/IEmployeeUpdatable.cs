using BusinessLogic.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace BusinessLogic
{
    public interface IEmployeeUpdatable
    {
        public ObservableCollection<EmployeeViewModel> Employees { get; set; }

        public void EmployeeUpdated(object sender, PropertyChangedEventArgs e);
    }
}
