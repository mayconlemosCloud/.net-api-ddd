using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Services.Interfaces;
using API.Controllers;
using FluentValidation;
using FluentValidation.Results;

namespace Test.Controllers;

public class UserControllerTests
{
    private readonly Mock<IUserService> _userServiceMock;
    private readonly UserController _controller;

    public UserControllerTests()
    {
        _userServiceMock = new Mock<IUserService>();
        _controller = new UserController(_userServiceMock.Object);
    }

    [Fact]
    public async Task GetAllUsers_ReturnsOk_WithUsers()
    {
        var users = new List<UserResponse> { new UserResponse { Id = Guid.NewGuid(), Name = "Test", Email = "test@test.com" } };
        _userServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(users);

        var result = await _controller.GetAllUsers();

        var ok = result as OkObjectResult;
        ok.Should().NotBeNull();
        ok!.Value.Should().BeEquivalentTo(users);
    }

    [Fact]
    public async Task GetAllUsers_ReturnsBadRequest_OnValidationException()
    {
        _userServiceMock.Setup(s => s.GetAllAsync()).ThrowsAsync(new ValidationException(new List<ValidationFailure>()));

        var result = await _controller.GetAllUsers();

        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task GetAllUsers_ReturnsServerError_OnException()
    {
        _userServiceMock.Setup(s => s.GetAllAsync()).ThrowsAsync(new Exception("fail"));

        var result = await _controller.GetAllUsers();

        var obj = result as ObjectResult;
        obj.Should().NotBeNull();
        obj!.StatusCode.Should().Be(500);
    }

    [Fact]
    public async Task GetUserById_ReturnsOk_WhenFound()
    {
        var user = new UserResponse { Id = Guid.NewGuid(), Name = "Test", Email = "test@test.com" };
        _userServiceMock.Setup(s => s.GetByIdAsync(user.Id)).ReturnsAsync(user);

        var result = await _controller.GetUserById(user.Id);

        var ok = result as OkObjectResult;
        ok.Should().NotBeNull();
        ok!.Value.Should().BeEquivalentTo(user);
    }

    [Fact]
    public async Task GetUserById_ReturnsNotFound_WhenNull()
    {
        _userServiceMock.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((UserResponse?)null);

        var result = await _controller.GetUserById(Guid.NewGuid());

        var notFound = result as NotFoundObjectResult;
        notFound.Should().NotBeNull();
    }

    [Fact]
    public async Task GetUserById_ReturnsBadRequest_OnValidationException()
    {
        _userServiceMock.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ThrowsAsync(new ValidationException(new List<ValidationFailure>()));

        var result = await _controller.GetUserById(Guid.NewGuid());

        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task GetUserById_ReturnsServerError_OnException()
    {
        _userServiceMock.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ThrowsAsync(new Exception("fail"));

        var result = await _controller.GetUserById(Guid.NewGuid());

        var obj = result as ObjectResult;
        obj.Should().NotBeNull();
        obj!.StatusCode.Should().Be(500);
    }

    [Fact]
    public async Task CreateUser_ReturnsCreated_WhenValid()
    {
        var req = new UserRequest { Name = "Test", Email = "test@test.com" };
        var resp = new UserResponse { Id = Guid.NewGuid(), Name = req.Name, Email = req.Email };
        _userServiceMock.Setup(s => s.AddAsync(req)).ReturnsAsync(resp);

        var result = await _controller.CreateUser(req);

        var created = result as CreatedAtActionResult;
        created.Should().NotBeNull();
        created!.Value.Should().BeEquivalentTo(resp);
    }

    [Fact]
    public async Task CreateUser_ReturnsBadRequest_WhenModelInvalid()
    {
        _controller.ModelState.AddModelError("Name", "Required");

        var result = await _controller.CreateUser(new UserRequest());

        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task CreateUser_ReturnsBadRequest_OnValidationException()
    {
        _userServiceMock.Setup(s => s.AddAsync(It.IsAny<UserRequest>())).ThrowsAsync(new ValidationException(new List<ValidationFailure>()));

        var result = await _controller.CreateUser(new UserRequest());

        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task CreateUser_ReturnsServerError_OnException()
    {
        _userServiceMock.Setup(s => s.AddAsync(It.IsAny<UserRequest>())).ThrowsAsync(new Exception("fail"));

        var result = await _controller.CreateUser(new UserRequest());

        var obj = result as ObjectResult;
        obj.Should().NotBeNull();
        obj!.StatusCode.Should().Be(500);
    }

    [Fact]
    public async Task UpdateUser_ReturnsOk_WhenUpdated()
    {
        var id = Guid.NewGuid();
        var req = new UserRequest { Name = "Test", Email = "test@test.com" };
        var resp = new UserResponse { Id = id, Name = req.Name, Email = req.Email };
        _userServiceMock.Setup(s => s.UpdateAsync(id, req)).ReturnsAsync(resp);

        var result = await _controller.UpdateUser(id, req);

        var ok = result as OkObjectResult;
        ok.Should().NotBeNull();
        ok!.Value.Should().BeEquivalentTo(resp);
    }

    [Fact]
    public async Task UpdateUser_ReturnsNotFound_WhenNull()
    {
        _userServiceMock.Setup(s => s.UpdateAsync(It.IsAny<Guid>(), It.IsAny<UserRequest>())).ReturnsAsync((UserResponse?)null);

        var result = await _controller.UpdateUser(Guid.NewGuid(), new UserRequest());

        var notFound = result as NotFoundObjectResult;
        notFound.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateUser_ReturnsBadRequest_WhenModelInvalid()
    {
        _controller.ModelState.AddModelError("Name", "Required");

        var result = await _controller.UpdateUser(Guid.NewGuid(), new UserRequest());

        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task UpdateUser_ReturnsBadRequest_OnValidationException()
    {
        _userServiceMock.Setup(s => s.UpdateAsync(It.IsAny<Guid>(), It.IsAny<UserRequest>())).ThrowsAsync(new ValidationException(new List<ValidationFailure>()));

        var result = await _controller.UpdateUser(Guid.NewGuid(), new UserRequest());

        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task UpdateUser_ReturnsServerError_OnException()
    {
        _userServiceMock.Setup(s => s.UpdateAsync(It.IsAny<Guid>(), It.IsAny<UserRequest>())).ThrowsAsync(new Exception("fail"));

        var result = await _controller.UpdateUser(Guid.NewGuid(), new UserRequest());

        var obj = result as ObjectResult;
        obj.Should().NotBeNull();
        obj!.StatusCode.Should().Be(500);
    }

    [Fact]
    public async Task DeleteUser_ReturnsNoContent_WhenDeleted()
    {
        _userServiceMock.Setup(s => s.DeleteAsync(It.IsAny<Guid>())).ReturnsAsync(true);

        var result = await _controller.DeleteUser(Guid.NewGuid());

        var noContent = result as NoContentResult;
        noContent.Should().NotBeNull();
    }

    [Fact]
    public async Task DeleteUser_ReturnsNotFound_WhenNotFound()
    {
        _userServiceMock.Setup(s => s.DeleteAsync(It.IsAny<Guid>())).ReturnsAsync(false);

        var result = await _controller.DeleteUser(Guid.NewGuid());

        var notFound = result as NotFoundObjectResult;
        notFound.Should().NotBeNull();
    }

    [Fact]
    public async Task DeleteUser_ReturnsBadRequest_OnValidationException()
    {
        _userServiceMock.Setup(s => s.DeleteAsync(It.IsAny<Guid>())).ThrowsAsync(new ValidationException(new List<ValidationFailure>()));

        var result = await _controller.DeleteUser(Guid.NewGuid());

        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task DeleteUser_ReturnsServerError_OnException()
    {
        _userServiceMock.Setup(s => s.DeleteAsync(It.IsAny<Guid>())).ThrowsAsync(new Exception("fail"));

        var result = await _controller.DeleteUser(Guid.NewGuid());

        var obj = result as ObjectResult;
        obj.Should().NotBeNull();
        obj!.StatusCode.Should().Be(500);
    }
}
