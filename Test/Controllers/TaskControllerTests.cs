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
using FluentValidation;
using FluentValidation.Results;

namespace Test.Controllers
{
    public class TaskControllerTests
    {
        private readonly Mock<ITaskService> _serviceMock;
        private readonly TaskController _controller;

        public TaskControllerTests()
        {
            _serviceMock = new Mock<ITaskService>();
            _controller = new TaskController(_serviceMock.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOk()
        {
            var tasks = new List<TaskResponse> { new TaskResponse { Id = Guid.NewGuid(), Title = "Task" } };
            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(tasks);

            var result = await _controller.GetAll();

            var ok = result as OkObjectResult;
            ok.Should().NotBeNull();
            ok!.Value.Should().BeEquivalentTo(tasks);
        }

        [Fact]
        public async Task GetAll_ValidationException_ReturnsBadRequest()
        {
            _serviceMock.Setup(s => s.GetAllAsync()).ThrowsAsync(new ValidationException(new List<ValidationFailure>()));
            var result = await _controller.GetAll();
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task GetAll_Exception_ReturnsStatus500()
        {
            _serviceMock.Setup(s => s.GetAllAsync()).ThrowsAsync(new Exception("error"));
            var result = await _controller.GetAll();
            result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(500);
        }

        [Fact]
        public async Task GetById_Found_ReturnsOk()
        {
            var id = Guid.NewGuid();
            var task = new TaskResponse { Id = id, Title = "Task" };
            _serviceMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(task);

            var result = await _controller.GetById(id);

            var ok = result as OkObjectResult;
            ok.Should().NotBeNull();
            ok!.Value.Should().BeEquivalentTo(task);
        }

        [Fact]
        public async Task GetById_NotFound_ReturnsNotFound()
        {
            var id = Guid.NewGuid();
            _serviceMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((TaskResponse?)null);

            var result = await _controller.GetById(id);

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task GetById_ValidationException_ReturnsBadRequest()
        {
            var id = Guid.NewGuid();
            _serviceMock.Setup(s => s.GetByIdAsync(id)).ThrowsAsync(new ValidationException(new List<ValidationFailure>()));

            var result = await _controller.GetById(id);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task GetById_Exception_ReturnsStatus500()
        {
            var id = Guid.NewGuid();
            _serviceMock.Setup(s => s.GetByIdAsync(id)).ThrowsAsync(new Exception("error"));

            var result = await _controller.GetById(id);

            result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(500);
        }

        [Fact]
        public async Task Create_ModelStateInvalid_ReturnsBadRequest()
        {
            _controller.ModelState.AddModelError("Name", "Required");
            var result = await _controller.Create(new TaskRequest());
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Create_Success_ReturnsCreatedAtAction()
        {
            var req = new TaskRequest { Title = "Task" };
            var resp = new TaskResponse { Id = Guid.NewGuid(), Title = "Task" };
            _serviceMock.Setup(s => s.AddAsync(req)).ReturnsAsync(resp);

            var result = await _controller.Create(req);

            var created = result as CreatedAtActionResult;
            created.Should().NotBeNull();
            created!.Value.Should().BeEquivalentTo(resp);
        }

        [Fact]
        public async Task Create_ValidationException_ReturnsBadRequest()
        {
            var req = new TaskRequest();
            _serviceMock.Setup(s => s.AddAsync(req)).ThrowsAsync(new ValidationException(new List<ValidationFailure>()));

            var result = await _controller.Create(req);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Create_Exception_ReturnsStatus500()
        {
            var req = new TaskRequest();
            _serviceMock.Setup(s => s.AddAsync(req)).ThrowsAsync(new Exception("error"));

            var result = await _controller.Create(req);

            result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(500);
        }

        [Fact]
        public async Task Update_ModelStateInvalid_ReturnsBadRequest()
        {
            _controller.ModelState.AddModelError("Name", "Required");
            var result = await _controller.Update(Guid.NewGuid(), new TaskRequest());
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Update_NotFound_ReturnsNotFound()
        {
            var id = Guid.NewGuid();
            var req = new TaskRequest();
            _serviceMock.Setup(s => s.UpdateAsync(id, req)).ReturnsAsync((TaskResponse?)null);

            var result = await _controller.Update(id, req);

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task Update_Success_ReturnsOk()
        {
            var id = Guid.NewGuid();
            var req = new TaskRequest();
            var resp = new TaskResponse { Id = id, Title = "Task" };
            _serviceMock.Setup(s => s.UpdateAsync(id, req)).ReturnsAsync(resp);

            var result = await _controller.Update(id, req);

            var ok = result as OkObjectResult;
            ok.Should().NotBeNull();
            ok!.Value.Should().BeEquivalentTo(resp);
        }

        [Fact]
        public async Task Update_ValidationException_ReturnsBadRequest()
        {
            var id = Guid.NewGuid();
            var req = new TaskRequest();
            _serviceMock.Setup(s => s.UpdateAsync(id, req)).ThrowsAsync(new ValidationException(new List<ValidationFailure>()));

            var result = await _controller.Update(id, req);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Update_Exception_ReturnsStatus500()
        {
            var id = Guid.NewGuid();
            var req = new TaskRequest();
            _serviceMock.Setup(s => s.UpdateAsync(id, req)).ThrowsAsync(new Exception("error"));

            var result = await _controller.Update(id, req);

            result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(500);
        }

        [Fact]
        public async Task Delete_NotFound_ReturnsNotFound()
        {
            var id = Guid.NewGuid();
            _serviceMock.Setup(s => s.DeleteAsync(id)).ReturnsAsync(false);

            var result = await _controller.Delete(id);

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task Delete_Success_ReturnsNoContent()
        {
            var id = Guid.NewGuid();
            _serviceMock.Setup(s => s.DeleteAsync(id)).ReturnsAsync(true);

            var result = await _controller.Delete(id);

            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task Delete_ValidationException_ReturnsBadRequest()
        {
            var id = Guid.NewGuid();
            _serviceMock.Setup(s => s.DeleteAsync(id)).ThrowsAsync(new ValidationException(new List<ValidationFailure>()));

            var result = await _controller.Delete(id);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Delete_Exception_ReturnsStatus500()
        {
            var id = Guid.NewGuid();
            _serviceMock.Setup(s => s.DeleteAsync(id)).ThrowsAsync(new Exception("error"));

            var result = await _controller.Delete(id);

            result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(500);
        }

        [Fact]
        public void CommentResponse_Properties_SetAndGet()
        {
            var id = Guid.NewGuid();
            var taskId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var createdAt = DateTime.UtcNow.AddHours(-1);
            var updatedAt = DateTime.UtcNow;

            var comment = new CommentResponse
            {
                Id = id,
                Content = "Test comment",
                TaskEntityId = taskId,
                CreatedAt = createdAt,
                UpdatedAt = updatedAt,
                UserId = userId
            };

            comment.Id.Should().Be(id);
            comment.Content.Should().Be("Test comment");
            comment.TaskEntityId.Should().Be(taskId);
            comment.CreatedAt.Should().Be(createdAt);
            comment.UpdatedAt.Should().Be(updatedAt);
            comment.UserId.Should().Be(userId);
        }
    }
}
