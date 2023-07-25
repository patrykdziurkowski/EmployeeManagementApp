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
        private string _jobId;
        public string JobId
        {
            get
            {
                return _jobId;
            }
            set
            {
                _jobId = value;
            }
        }

        private string _jobTitle;
        public string JobTitle
        {
            get
            {
                return _jobTitle;
            }
            set
            {
                _jobTitle = value;
            }
        }

        private int? _minSalary;
        public int? MinSalary
        {
            get
            {
                return _minSalary;
            }
            set
            {
                _minSalary = value;
            }
        }

        private int? _maxSalary;
        public int? MaxSalary
        {
            get
            {
                return _maxSalary;
            }
            set
            {
                _maxSalary = value;
            }
        }

        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public JobDto(
            string jobId,
            string jobTitle)
        {
            _jobId = jobId;
            _jobTitle = jobTitle;
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
