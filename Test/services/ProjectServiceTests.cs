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

namespace Test.Services;
public class ProjectServiceTests
{
    private readonly Mock<IBaseRepository<Project>> _projectRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IValidator<Project>> _validatorMock;
    private readonly ProjectService _projectService;
    private readonly Fixture _fixture;

    public ProjectServiceTests()
    {
        _projectRepositoryMock = new Mock<IBaseRepository<Project>>();
        _mapperMock = new Mock<IMapper>();
        _validatorMock = new Mock<IValidator<Project>>();
        _projectService = new ProjectService(
            _projectRepositoryMock.Object,
            _mapperMock.Object,
            _validatorMock.Object
        );
        _fixture = new Fixture();
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public async Task ObterProjetoPorId_DeveRetornarProjeto_QuandoProjetoExistir()
    {
        var projectId = _fixture.Create<Guid>();
        var fakeProject = new Faker<Project>()
            .RuleFor(p => p.Id, projectId)
            .RuleFor(p => p.Name, f => f.Company.CompanyName())
            .Generate();

        _projectRepositoryMock.Setup(repo => repo.GetByIdAsync(projectId)).ReturnsAsync(fakeProject);
        _mapperMock.Setup(mapper => mapper.Map<ProjectResponse>(fakeProject)).Returns(new ProjectResponse
        {
            Id = fakeProject.Id,
            Name = fakeProject.Name
        });

        var result = await _projectService.GetByIdAsync(projectId);

        result.Should().NotBeNull();
        result.Id.Should().Be(projectId);
        result.Name.Should().Be(fakeProject.Name);
    }

    [Fact]
    public async Task ObterProjetoPorId_DeveRetornarNulo_QuandoProjetoNaoExistir()
    {
        var projectId = _fixture.Create<Guid>();
        _projectRepositoryMock.Setup(repo => repo.GetByIdAsync(projectId)).ReturnsAsync((Project)null);

        var result = await _projectService.GetByIdAsync(projectId);

        result.Should().BeNull();
    }

    [Fact]
    public async Task AdicionarProjeto_DeveAdicionarProjeto_QuandoRequisicaoForValida()
    {
        var request = _fixture.Create<ProjectRequest>();
        var project = _fixture.Create<Project>();
        _mapperMock.Setup(m => m.Map<Project>(request)).Returns(project);
        _validatorMock.Setup(v => v.ValidateAsync(project, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _projectRepositoryMock.Setup(repo => repo.AddAsync(project)).Returns(Task.CompletedTask);
        _mapperMock.Setup(m => m.Map<ProjectResponse>(project)).Returns(new ProjectResponse { Id = project.Id, Name = project.Name });

        var result = await _projectService.AddAsync(request);

        result.Should().NotBeNull();
        result.Name.Should().Be(project.Name);
    }

    [Fact]
    public async Task AdicionarProjeto_DeveLancarExcecaoDeValidacao_QuandoValidacaoFalhar()
    {
        var request = _fixture.Create<ProjectRequest>();
        var project = _fixture.Create<Project>();
        _mapperMock.Setup(m => m.Map<Project>(request)).Returns(project);
        _validatorMock.Setup(v => v.ValidateAsync(project, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("Name", "Name is required") }));

        Func<Task> act = async () => await _projectService.AddAsync(request);

        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    [Fact]
    public async Task AtualizarProjeto_DeveAtualizarProjeto_QuandoProjetoExistir()
    {
        var id = _fixture.Create<Guid>();
        var request = _fixture.Create<ProjectRequest>();
        var project = _fixture.Create<Project>();
        _projectRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(project);
        _validatorMock.Setup(v => v.ValidateAsync(project, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mapperMock.Setup(m => m.Map(request, project));
        _projectRepositoryMock.Setup(repo => repo.UpdateAsync(project)).Returns(Task.CompletedTask);
        _mapperMock.Setup(m => m.Map<ProjectResponse>(project)).Returns(new ProjectResponse { Id = project.Id, Name = project.Name });

        var result = await _projectService.UpdateAsync(id, request);

        result.Should().NotBeNull();
        result.Name.Should().Be(project.Name);
    }

    [Fact]
    public async Task AtualizarProjeto_DeveLancarExcecaoDeValidacao_QuandoValidacaoFalhar()
    {
        var id = _fixture.Create<Guid>();
        var request = _fixture.Create<ProjectRequest>();
        var project = _fixture.Create<Project>();
        _projectRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(project);
        _validatorMock.Setup(v => v.ValidateAsync(project, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("Name", "Invalid") }));

        Func<Task> act = async () => await _projectService.UpdateAsync(id, request);

        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    [Fact]
    public async Task AtualizarProjeto_DeveRetornarNulo_QuandoProjetoNaoExistir()
    {
        var id = _fixture.Create<Guid>();
        var request = _fixture.Create<ProjectRequest>();
        _projectRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync((Project)null);

        var result = await _projectService.UpdateAsync(id, request);

        result.Should().BeNull();
    }

    [Fact]
    public async Task DeletarProjeto_DeveDeletarProjeto_QuandoProjetoExistir()
    {
        var id = _fixture.Create<Guid>();
        var project = _fixture.Create<Project>();
        _projectRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(project);
        _validatorMock.Setup(v => v.ValidateAsync(project, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _projectRepositoryMock.Setup(repo => repo.DeleteAsync(project)).Returns(Task.CompletedTask);

        var result = await _projectService.DeleteAsync(id);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task DeletarProjeto_DeveRetornarFalso_QuandoProjetoNaoExistir()
    {
        var id = _fixture.Create<Guid>();
        _projectRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync((Project)null);

        var result = await _projectService.DeleteAsync(id);

        result.Should().BeFalse();
    }
}
