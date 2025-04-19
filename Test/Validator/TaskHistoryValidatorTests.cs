using Xunit;
using Application.Validations;
using Domain.Entities;
using System;
using FluentAssertions;

namespace Test.Validator;
public class TaskHistoryValidatorTests
{
    private readonly TaskHistoryValidator _validator = new TaskHistoryValidator();

    [Fact]
    public void Should_Pass_When_TaskHistory_Is_Valid()
    {
        var history = new TaskHistory
        {
            Changes = "Alteração",
            UserId = Guid.NewGuid(),
            CreatedAt = DateTime.Now.AddSeconds(-1),
            UpdatedAt = DateTime.Now.AddSeconds(-1)
        };
        var result = _validator.Validate(history);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Fail_When_Changes_Is_Empty()
    {
        var history = new TaskHistory
        {
            Changes = "",
            UserId = Guid.NewGuid(),
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        var result = _validator.Validate(history);
        result.IsValid.Should().BeFalse();
    }
}
