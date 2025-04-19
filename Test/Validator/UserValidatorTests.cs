using Xunit;
using Application.Validations;
using Domain.Entities;
using FluentAssertions;
namespace Test.Validator;
public class UserValidatorTests
{
    private readonly UserValidator _validator = new UserValidator();

    [Fact]
    public void Should_Pass_When_User_Is_Valid()
    {
        var user = new User { Name = "João", Email = "joao@email.com" };
        var result = _validator.Validate(user);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Fail_When_Name_Is_Empty()
    {
        var user = new User { Name = "", Email = "joao@email.com" };
        var result = _validator.Validate(user);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Should_Fail_When_Email_Is_Invalid()
    {
        var user = new User { Name = "João", Email = "not-an-email" };
        var result = _validator.Validate(user);
        result.IsValid.Should().BeFalse();
    }
}
