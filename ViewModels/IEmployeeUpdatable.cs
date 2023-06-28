using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public interface IEmployeeUpdatable
    {
        public ObservableCollection<EmployeeViewModel> Employees { get; set; }

        public void UpdateEmployee(object sender, PropertyChangedEventArgs e);
    }
}
