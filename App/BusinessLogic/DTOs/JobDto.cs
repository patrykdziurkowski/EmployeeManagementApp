using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ViewModels
{
    public class JobDto : INotifyPropertyChanged
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
        public string JobId { get; set; }
        public string JobTitle { get; set; }
        public int? MinSalary { get; set; }
        public int? MaxSalary { get; set; }

        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public JobDto(
            string jobId,
            string jobTitle)
        {
            JobId = jobId;
            JobTitle = jobTitle;
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
