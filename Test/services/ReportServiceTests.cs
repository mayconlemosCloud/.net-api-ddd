using Xunit;
using Moq;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Services;
using Domain.Repositories;
using Domain.Entities;
using Application.DTOs;

namespace Test.Services
{
    public class ReportServiceTests
    {
        private readonly Mock<IBaseRepository<User>> _userRepoMock;
        private readonly Mock<IBaseRepository<TaskEntity>> _taskRepoMock;
        private readonly ReportService _service;

        public ReportServiceTests()
        {
            _userRepoMock = new Mock<IBaseRepository<User>>();
            _taskRepoMock = new Mock<IBaseRepository<TaskEntity>>();
            _service = new ReportService(_userRepoMock.Object, _taskRepoMock.Object);
        }

        [Fact]
        public async Task GetUserPerformanceReportAsync_ReturnsReportWithCorrectAverages()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var users = new List<User>
            {
                new User { Id = userId, Name = "User1" }
            };
            var now = DateTime.UtcNow;
            var tasks = new List<TaskEntity>
            {
                new TaskEntity { UserId = userId, Status = "Concluída", UpdatedAt = now.AddDays(-1) },
                new TaskEntity { UserId = userId, Status = "Concluída", UpdatedAt = now.AddDays(-10) },
                new TaskEntity { UserId = userId, Status = "Pendente", UpdatedAt = now.AddDays(-5) },
                new TaskEntity { UserId = userId, Status = "Concluída", UpdatedAt = now.AddDays(-40) }, // fora do range
            };

            _userRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(users);
            _taskRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(tasks);

            // Act
            var result = (await _service.GetUserPerformanceReportAsync()).ToList();

            // Assert
            result.Should().HaveCount(1);
            result[0].UserId.Should().Be(userId);
            result[0].UserName.Should().Be("User1");
            // Apenas 2 tarefas "Concluída" nos últimos 30 dias
            result[0].AverageCompletedTasksLast30Days.Should().BeApproximately(2.0 / 30.0, 0.0001);
        }

        [Fact]
        public async Task GetUserPerformanceReportAsync_ReturnsZeroAverage_WhenNoCompletedTasks()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var users = new List<User>
            {
                new User { Id = userId, Name = "User2" }
            };
            var tasks = new List<TaskEntity>
            {
                new TaskEntity { UserId = userId, Status = "Pendente", UpdatedAt = DateTime.UtcNow }
            };

            _userRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(users);
            _taskRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(tasks);

            // Act
            var result = (await _service.GetUserPerformanceReportAsync()).ToList();

            // Assert
            result.Should().HaveCount(1);
            result[0].AverageCompletedTasksLast30Days.Should().Be(0);
        }

        [Fact]
        public async Task GetUserPerformanceReportAsync_ReturnsEmpty_WhenNoUsers()
        {
            // Arrange
            _userRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<User>());
            _taskRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<TaskEntity>());

            // Act
            var result = await _service.GetUserPerformanceReportAsync();

            // Assert
            result.Should().BeEmpty();
        }
    }
}
