using Xunit;
using Moq;
using FluentAssertions;
using AutoFixture;
using Bogus;
using Domain.Entities;
using Application.Services;
using Domain.Repositories;
using AutoMapper;
using FluentValidation;
using Application.DTOs;

public class TaskServiceTests
{
    private readonly Mock<IBaseRepository<TaskEntity>> _taskRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IValidator<TaskEntity>> _validatorMock;
    private readonly TaskService _taskService;
    private readonly Fixture _fixture;

    public TaskServiceTests()
    {
        _taskRepositoryMock = new Mock<IBaseRepository<TaskEntity>>();
        _mapperMock = new Mock<IMapper>();
        _validatorMock = new Mock<IValidator<TaskEntity>>();
        _taskService = new TaskService(
            _taskRepositoryMock.Object,
            _mapperMock.Object,
            _validatorMock.Object
        );
        _fixture = new Fixture();
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public async Task ObterTarefaPorId_DeveRetornarTarefa_QuandoTarefaExistir()
    {
        var taskId = _fixture.Create<Guid>();
        var fakeTask = new Faker<TaskEntity>()
            .RuleFor(t => t.Id, taskId)
            .RuleFor(t => t.Title, f => f.Lorem.Sentence())
            .Generate();

        _taskRepositoryMock.Setup(repo => repo.GetByIdAsync(taskId)).ReturnsAsync(fakeTask);
        _mapperMock.Setup(mapper => mapper.Map<TaskResponse>(fakeTask)).Returns(new TaskResponse
        {
            Id = fakeTask.Id,
            Title = fakeTask.Title
        });

        var result = await _taskService.GetByIdAsync(taskId);

        result.Should().NotBeNull();
        result.Id.Should().Be(taskId);
        result.Title.Should().Be(fakeTask.Title);
    }

    [Fact]
    public async Task ObterTarefaPorId_DeveRetornarNulo_QuandoTarefaNaoExistir()
    {
        var taskId = _fixture.Create<Guid>();
        _taskRepositoryMock.Setup(repo => repo.GetByIdAsync(taskId)).ReturnsAsync((TaskEntity)null);

        var result = await _taskService.GetByIdAsync(taskId);

        result.Should().BeNull();
    }

    [Fact]
    public async Task AdicionarTarefa_DeveAdicionarTarefa_QuandoRequisicaoForValida()
    {
        var request = _fixture.Create<TaskRequest>();
        var task = _fixture.Create<TaskEntity>();
        _mapperMock.Setup(m => m.Map<TaskEntity>(request)).Returns(task);
        _validatorMock.Setup(v => v.ValidateAsync(task, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _taskRepositoryMock.Setup(repo => repo.AddAsync(task)).Returns(Task.CompletedTask);
        _mapperMock.Setup(m => m.Map<TaskResponse>(task)).Returns(new TaskResponse { Id = task.Id, Title = task.Title });

        var result = await _taskService.AddAsync(request);

        result.Should().NotBeNull();
        result.Title.Should().Be(task.Title);
    }

    [Fact]
    public async Task AdicionarTarefa_DeveLancarExcecaoDeValidacao_QuandoValidacaoFalhar()
    {
        var request = _fixture.Create<TaskRequest>();
        var task = _fixture.Create<TaskEntity>();
        _mapperMock.Setup(m => m.Map<TaskEntity>(request)).Returns(task);
        _validatorMock.Setup(v => v.ValidateAsync(task, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("Title", "Title is required") }));

        Func<Task> act = async () => await _taskService.AddAsync(request);

        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    [Fact]
    public async Task AtualizarTarefa_DeveAtualizarTarefa_QuandoTarefaExistir()
    {
        var id = _fixture.Create<Guid>();
        var request = _fixture.Create<TaskRequest>();
        var task = _fixture.Create<TaskEntity>();
        _taskRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(task);
        _validatorMock.Setup(v => v.ValidateAsync(task, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mapperMock.Setup(m => m.Map(request, task));
        _taskRepositoryMock.Setup(repo => repo.UpdateAsync(task)).Returns(Task.CompletedTask);
        _mapperMock.Setup(m => m.Map<TaskResponse>(task)).Returns(new TaskResponse { Id = task.Id, Title = task.Title });

        var result = await _taskService.UpdateAsync(id, request);

        result.Should().NotBeNull();
        result.Title.Should().Be(task.Title);
    }

    [Fact]
    public async Task AtualizarTarefa_DeveLancarExcecaoDeValidacao_QuandoValidacaoFalhar()
    {
        var id = _fixture.Create<Guid>();
        var request = _fixture.Create<TaskRequest>();
        var task = _fixture.Create<TaskEntity>();
        _taskRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(task);
        _validatorMock.Setup(v => v.ValidateAsync(task, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("Title", "Invalid") }));

        Func<Task> act = async () => await _taskService.UpdateAsync(id, request);

        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    [Fact]
    public async Task AtualizarTarefa_DeveRetornarNulo_QuandoTarefaNaoExistir()
    {
        var id = _fixture.Create<Guid>();
        var request = _fixture.Create<TaskRequest>();
        _taskRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync((TaskEntity)null);

        var result = await _taskService.UpdateAsync(id, request);

        result.Should().BeNull();
    }

    [Fact]
    public async Task DeletarTarefa_DeveDeletarTarefa_QuandoTarefaExistir()
    {
        var id = _fixture.Create<Guid>();
        var task = _fixture.Create<TaskEntity>();
        _taskRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(task);
        _validatorMock.Setup(v => v.ValidateAsync(task, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _taskRepositoryMock.Setup(repo => repo.DeleteAsync(task)).Returns(Task.CompletedTask);

        var result = await _taskService.DeleteAsync(id);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task DeletarTarefa_DeveRetornarFalso_QuandoTarefaNaoExistir()
    {
        var id = _fixture.Create<Guid>();
        _taskRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync((TaskEntity)null);

        var result = await _taskService.DeleteAsync(id);

        result.Should().BeFalse();
    }
}
