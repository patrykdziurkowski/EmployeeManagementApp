using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ViewModels
{
    public class JobViewModel : INotifyPropertyChanged
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
#nullable disable
        public string JobId { get; set; }
        public string JobTitle { get; set; }
#nullable enable
        public int? MinSalary { get; set; }
        public int? MaxSalary { get; set; }

        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public JobViewModel()
        {

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

    }
}
