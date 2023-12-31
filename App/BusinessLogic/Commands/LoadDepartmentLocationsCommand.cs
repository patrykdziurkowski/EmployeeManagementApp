﻿using BusinessLogic.ViewModels;
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
    public class LoadDepartmentLocationsCommand : ICommand
    {
        private readonly DepartmentLocationsMenuViewModel _viewModel;
        private readonly DepartmentLocationRepository _departmentLocationRepository;

        public event EventHandler? CanExecuteChanged;

        public LoadDepartmentLocationsCommand(
            DepartmentLocationsMenuViewModel viewModel,
            DepartmentLocationRepository departmentLocationRepository)
        {
            _viewModel = viewModel;
            _departmentLocationRepository = departmentLocationRepository;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public async void Execute(object? parameter)
        {
            List<DepartmentLocationDto> departmentLocationDtos = (await _departmentLocationRepository.GetAllAsync()).ToListOfDepartmentLocationDto();
            ObservableCollection<DepartmentLocationDto> departmentLocations = new(departmentLocationDtos);

            _viewModel.DepartmentLocations = departmentLocations;
            _viewModel.DepartmentLocations.CollectionChanged += _viewModel.DepartmentLocation_CollectionChanged;
        }
    }
}
