using Xunit;
using Moq;
using Application.Validations;
using Domain.Entities;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;

namespace Test.Validator;
public class TaskValidatorTests
{
    [Fact]
    public async Task Should_Pass_When_Task_Is_Valid()
    {
        var mockRepo = new Mock<IBaseRepository<TaskEntity>>();
        mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<TaskEntity>());
        var validator = new TaskValidator(mockRepo.Object);

        var task = new TaskEntity
        {
            Title = "Tarefa",
            Description = "Descrição",
            DueDate = DateTime.Now.AddDays(1),
            Priority = "Alta",
            ProjectId = Guid.NewGuid()
        };
        var result = await validator.ValidateAsync(task);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Should_Fail_When_Title_Is_Empty()
    {
        var mockRepo = new Mock<IBaseRepository<TaskEntity>>();
        mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<TaskEntity>());
        var validator = new TaskValidator(mockRepo.Object);

        var task = new TaskEntity { Title = "", Priority = "Alta", DueDate = DateTime.Now.AddDays(1), ProjectId = Guid.NewGuid() };
        var result = await validator.ValidateAsync(task);

        result.IsValid.Should().BeFalse();
    }
}
