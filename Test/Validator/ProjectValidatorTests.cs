using Xunit;
using Moq;
using Application.Validations;
using Domain.Entities;
using Domain.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;

namespace Test.Validator;
public class ProjectValidatorTests
{
    [Fact]
    public async Task Should_Pass_When_Project_Is_Valid()
    {
        var mockRepo = new Mock<IBaseRepository<TaskEntity>>();
        mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<TaskEntity>());
        var validator = new ProjectValidator(mockRepo.Object);

        var project = new Project { Name = "Projeto", Tasks = new List<TaskEntity>() };
        var result = await validator.ValidateAsync(project);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Should_Fail_When_Name_Is_Empty()
    {
        var mockRepo = new Mock<IBaseRepository<TaskEntity>>();
        mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<TaskEntity>());
        var validator = new ProjectValidator(mockRepo.Object);

        var project = new Project { Name = "", Tasks = new List<TaskEntity>() };
        var result = await validator.ValidateAsync(project);

        result.IsValid.Should().BeFalse();
    }
}
