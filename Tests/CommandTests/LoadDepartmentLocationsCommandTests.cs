using BusinessLogic.Commands;
using BusinessLogic.ViewModels;
using DataAccess;
using DataAccess.Models;
using DataAccess.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests.CommandTests
{
#pragma warning disable CS1998
    public class LoadDepartmentLocationsCommandTests
    {
        private LoadDepartmentLocationsCommand _subject;

        private Mock<DepartmentLocationRepository> _mockDepartmentLocationRepository;
        private Mock<DepartmentLocationsMenuViewModel> _mockViewModel;

        public LoadDepartmentLocationsCommandTests()
        {
            Mock<ISqlDataAccess> mockDataAccess = new();
            _mockDepartmentLocationRepository = new(mockDataAccess.Object);

            _mockViewModel = new(_mockDepartmentLocationRepository.Object);

            _subject = new(
                _mockViewModel.Object,
                _mockDepartmentLocationRepository.Object);
        }

        [Fact]
        public async Task CanExecute_ReturnsTrue()
        {
            //Arrange

            //Act
            bool canExecute = _subject.CanExecute(null);

            //Assert
            Assert.True(canExecute);
        }

        [Fact]
        public async Task Execute_GivenDepartmentLocationsFromDatabase_AssignsThemToViewModelProperty()
        {
            //Arrange
            IEnumerable<DepartmentLocation> locations = new List<DepartmentLocation>()
            {
                new DepartmentLocation { DepartmentId = 1 },
                new DepartmentLocation { DepartmentId = 2 },
                new DepartmentLocation { DepartmentId = 3 }
            };

            _mockDepartmentLocationRepository
                .Setup(x => x.GetAllAsync())
                .Returns(Task.FromResult(locations));

            //Act
            bool canExecute = _subject.CanExecute(null);
            _subject.Execute(null);

            //Assert
            Assert.True(canExecute);
            Assert.Equal(3, _mockViewModel.Object.DepartmentLocation.Count);
            Assert.Contains(_mockViewModel.Object.DepartmentLocation, location => location.DepartmentId == 1);
            Assert.Contains(_mockViewModel.Object.DepartmentLocation, location => location.DepartmentId == 2);
            Assert.Contains(_mockViewModel.Object.DepartmentLocation, location => location.DepartmentId == 3);
        }

    }
#pragma warning restore CS1998
}
