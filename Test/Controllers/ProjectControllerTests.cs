using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Controllers;
using Application.DTOs;
using Application.Services.Interfaces;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;

namespace Tests.Controllers
{
    public class ProjectControllerTests
    {
        private readonly Mock<IProjectService> _serviceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ProjectController _controller;

        public ProjectControllerTests()
        {
            _serviceMock = new Mock<IProjectService>();
            _mapperMock = new Mock<IMapper>();
            _controller = new ProjectController(_serviceMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllProjects_ReturnsOk()
        {
            // Arrange
            var projects = new List<ProjectResponse> { new ProjectResponse { Id = Guid.NewGuid(), Name = "Test", UserId = Guid.NewGuid() } };
            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(projects);

            // Act
            var result = await _controller.GetAllProjects();

            // Assert
            var ok = result as OkObjectResult;
            ok.Should().NotBeNull();
            ok!.Value.Should().BeEquivalentTo(projects);
        }

        [Fact]
        public async Task GetAllProjects_ValidationException_ReturnsBadRequest()
        {
            _serviceMock.Setup(s => s.GetAllAsync()).ThrowsAsync(new ValidationException("fail", new List<ValidationFailure>()));
            var result = await _controller.GetAllProjects();
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task GetAllProjects_Exception_ReturnsStatus500()
        {
            _serviceMock.Setup(s => s.GetAllAsync()).ThrowsAsync(new Exception("error"));
            var result = await _controller.GetAllProjects();
            result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(500);
        }

        [Fact]
        public async Task GetProjectById_Found_ReturnsOk()
        {
            var id = Guid.NewGuid();
            var project = new ProjectResponse { Id = id, Name = "Test", UserId = Guid.NewGuid() };
            _serviceMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(project);

            var result = await _controller.GetProjectById(id);

            var ok = result as OkObjectResult;
            ok.Should().NotBeNull();
            ok!.Value.Should().BeEquivalentTo(project);
        }

        [Fact]
        public async Task GetProjectById_NotFound_ReturnsNotFound()
        {
            var id = Guid.NewGuid();
            _serviceMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((ProjectResponse?)null);

            var result = await _controller.GetProjectById(id);

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task GetProjectById_ValidationException_ReturnsBadRequest()
        {
            var id = Guid.NewGuid();
            _serviceMock.Setup(s => s.GetByIdAsync(id)).ThrowsAsync(new ValidationException("fail", new List<ValidationFailure>()));

            var result = await _controller.GetProjectById(id);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task GetProjectById_Exception_ReturnsStatus500()
        {
            var id = Guid.NewGuid();
            _serviceMock.Setup(s => s.GetByIdAsync(id)).ThrowsAsync(new Exception("error"));

            var result = await _controller.GetProjectById(id);

            result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(500);
        }

        [Fact]
        public async Task CreateProject_ModelStateInvalid_ReturnsBadRequest()
        {
            _controller.ModelState.AddModelError("Name", "Required");
            var result = await _controller.CreateProject(new ProjectRequest());
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task CreateProject_Success_ReturnsCreatedAtAction()
        {
            var req = new ProjectRequest { Name = "Test", UserId = Guid.NewGuid() };
            var resp = new ProjectResponse { Id = Guid.NewGuid(), Name = "Test", UserId = req.UserId };
            _serviceMock.Setup(s => s.AddAsync(req)).ReturnsAsync(resp);

            var result = await _controller.CreateProject(req);

            var created = result as CreatedAtActionResult;
            created.Should().NotBeNull();
            created!.Value.Should().BeEquivalentTo(resp);
        }

        [Fact]
        public async Task CreateProject_ValidationException_ReturnsBadRequest()
        {
            var req = new ProjectRequest();
            _serviceMock.Setup(s => s.AddAsync(req)).ThrowsAsync(new ValidationException("fail", new List<ValidationFailure>()));

            var result = await _controller.CreateProject(req);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task CreateProject_Exception_ReturnsStatus500()
        {
            var req = new ProjectRequest();
            _serviceMock.Setup(s => s.AddAsync(req)).ThrowsAsync(new Exception("error"));

            var result = await _controller.CreateProject(req);

            result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(500);
        }

        [Fact]
        public async Task UpdateProject_ModelStateInvalid_ReturnsBadRequest()
        {
            _controller.ModelState.AddModelError("Name", "Required");
            var result = await _controller.UpdateProject(Guid.NewGuid(), new ProjectRequest());
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task UpdateProject_NotFound_ReturnsNotFound()
        {
            var id = Guid.NewGuid();
            var req = new ProjectRequest();
            _serviceMock.Setup(s => s.UpdateAsync(id, req)).ReturnsAsync((ProjectResponse?)null);

            var result = await _controller.UpdateProject(id, req);

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task UpdateProject_Success_ReturnsOk()
        {
            var id = Guid.NewGuid();
            var req = new ProjectRequest();
            var resp = new ProjectResponse { Id = id, Name = "Test", UserId = Guid.NewGuid() };
            _serviceMock.Setup(s => s.UpdateAsync(id, req)).ReturnsAsync(resp);

            var result = await _controller.UpdateProject(id, req);

            var ok = result as OkObjectResult;
            ok.Should().NotBeNull();
            ok!.Value.Should().BeEquivalentTo(resp);
        }

        [Fact]
        public async Task UpdateProject_ValidationException_ReturnsBadRequest()
        {
            var id = Guid.NewGuid();
            var req = new ProjectRequest();
            _serviceMock.Setup(s => s.UpdateAsync(id, req)).ThrowsAsync(new ValidationException("fail", new List<ValidationFailure>()));

            var result = await _controller.UpdateProject(id, req);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task UpdateProject_Exception_ReturnsStatus500()
        {
            var id = Guid.NewGuid();
            var req = new ProjectRequest();
            _serviceMock.Setup(s => s.UpdateAsync(id, req)).ThrowsAsync(new Exception("error"));

            var result = await _controller.UpdateProject(id, req);

            result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(500);
        }

        [Fact]
        public async Task DeleteProject_NotFound_ReturnsNotFound()
        {
            var id = Guid.NewGuid();
            _serviceMock.Setup(s => s.DeleteAsync(id)).ReturnsAsync(false);

            var result = await _controller.DeleteProject(id);

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task DeleteProject_Success_ReturnsNoContent()
        {
            var id = Guid.NewGuid();
            _serviceMock.Setup(s => s.DeleteAsync(id)).ReturnsAsync(true);

            var result = await _controller.DeleteProject(id);

            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteProject_ValidationException_ReturnsBadRequest()
        {
            var id = Guid.NewGuid();
            _serviceMock.Setup(s => s.DeleteAsync(id)).ThrowsAsync(new ValidationException("fail", new List<ValidationFailure>()));

            var result = await _controller.DeleteProject(id);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task DeleteProject_Exception_ReturnsStatus500()
        {
            var id = Guid.NewGuid();
            _serviceMock.Setup(s => s.DeleteAsync(id)).ThrowsAsync(new Exception("error"));

            var result = await _controller.DeleteProject(id);

            result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(500);
        }
    }
}