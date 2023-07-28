using BusinessLogic.Commands;
using BusinessLogic.ViewModels;
using DataAccess;
using DataAccess.Interfaces;
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
    public class LoadJobHistoryCommandTests
    {
        private LoadJobHistoryCommand _subject;

        private Mock<JobHistoryRepository> _mockJobHistoryRepository;
        private Mock<JobHistoryMenuViewModel> _mockViewModel;

        public LoadJobHistoryCommandTests()
        {
            Mock<ISqlDataAccess> mockDataAccess = new();
            _mockJobHistoryRepository = new(mockDataAccess.Object);

            _mockViewModel = new(_mockJobHistoryRepository.Object);

            _subject = new(
                _mockViewModel.Object,
                _mockJobHistoryRepository.Object);
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
        public async Task Execute_GivenJobHistoryFromDatabase_AssignsItToViewModelProperty()
        {
            //Arrange
            IEnumerable<JobHistory> jobHistory = new List<JobHistory>()
            {
                new JobHistory { EmployeeId = 1 },
                new JobHistory { EmployeeId = 2 },
                new JobHistory { EmployeeId = 3 }
            };

            _mockJobHistoryRepository
                .Setup(x => x.GetAllAsync())
                .Returns(Task.FromResult(jobHistory));

            //Act
            bool canExecute = _subject.CanExecute(null);
            _subject.Execute(null);

            //Assert
            Assert.True(canExecute);
            Assert.Equal(3, _mockViewModel.Object.JobHistory.Count);
            Assert.Contains(_mockViewModel.Object.JobHistory, jobHistory => jobHistory.EmployeeId == 1);
            Assert.Contains(_mockViewModel.Object.JobHistory, jobHistory => jobHistory.EmployeeId == 2);
            Assert.Contains(_mockViewModel.Object.JobHistory, jobHistory => jobHistory.EmployeeId == 3);
        }

    }
#pragma warning restore CS1998
}
