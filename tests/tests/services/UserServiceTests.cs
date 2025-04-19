using Xunit; // Importa o framework de testes Xunit.
using Moq; // Importa a biblioteca Moq para criar mocks.
using FluentAssertions; // Importa FluentAssertions para facilitar as asserções nos testes.
using AutoFixture; // Importa AutoFixture para gerar dados de teste automaticamente.
using Bogus; // Importa Bogus para criar dados falsos realistas.
using Snapshooter.Xunit; // Importa Snapshooter para capturar snapshots nos testes.
using Domain.Entities; // Importa o namespace correto para a entidade User.
using Application.Services; // Importa o namespace correto para o serviço UserService.
using Domain.Repositories;
using AutoMapper;
using FluentValidation;
using Application.DTOs; // Importa o namespace correto para a interface IBaseRepository.

public class UserServiceTests
{
    private readonly Mock<IBaseRepository<User>> _userRepositoryMock; // Mock para a interface de repositório de usuários.
    private readonly Mock<IMapper> _mapperMock; // Mock para o IMapper.
    private readonly Mock<IValidator<User>> _validatorMock; // Mock para o IValidator<User>.
    private readonly UserService _userService; // Instância do serviço de usuários sendo testado.
    private readonly Fixture _fixture; // Instância do AutoFixture para gerar dados de teste.

    public UserServiceTests()
    {
        _userRepositoryMock = new Mock<IBaseRepository<User>>(); // Inicializa o mock do repositório de usuários.
        _mapperMock = new Mock<IMapper>(); // Inicializa o mock do IMapper.
        _validatorMock = new Mock<IValidator<User>>(); // Inicializa o mock do IValidator<User>.

        _userService = new UserService(
            _userRepositoryMock.Object,
            _mapperMock.Object, // Passa o mock do IMapper.
            _validatorMock.Object // Passa o mock do IValidator<User>.
        );

        _fixture = new Fixture(); // Inicializa o AutoFixture.

        // Configura o AutoFixture para ignorar referências circulares.
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public async Task ObterUsuarioPorId_DeveRetornarUsuario_QuandoUsuarioExistir()
    {
        // Arrange
        var userId = _fixture.Create<Guid>();
        var fakeUser = new Faker<User>()
            .RuleFor(u => u.Id, userId)
            .RuleFor(u => u.Name, f => f.Person.FullName)
            .RuleFor(u => u.Email, f => f.Person.Email)
            .Generate();

        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(fakeUser);
        _mapperMock.Setup(mapper => mapper.Map<UserResponse>(fakeUser)).Returns(new UserResponse
        {
            Id = fakeUser.Id,
            Name = fakeUser.Name,
            Email = fakeUser.Email
        });

        // Act
        var result = await _userService.GetByIdAsync(userId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(userId);
        result.Name.Should().Be(fakeUser.Name);
        result.Email.Should().Be(fakeUser.Email);
    }

    [Fact]
    public async Task ObterUsuarioPorId_DeveRetornarNulo_QuandoUsuarioNaoExistir()
    {
        // Arrange
        var userId = _fixture.Create<Guid>();
        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync((User)null);

        // Act
        var result = await _userService.GetByIdAsync(userId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task AdicionarUsuario_DeveAdicionarUsuario_QuandoRequisicaoForValida()
    {
        // Arrange
        var request = _fixture.Create<UserRequest>();
        var user = _fixture.Create<User>();
        _mapperMock.Setup(m => m.Map<User>(request)).Returns(user);
        _validatorMock.Setup(v => v.ValidateAsync(user, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _userRepositoryMock.Setup(repo => repo.AddAsync(user)).Returns(Task.CompletedTask);
        _mapperMock.Setup(m => m.Map<UserResponse>(user)).Returns(new UserResponse { Id = user.Id, Name = user.Name, Email = user.Email });

        // Act
        var result = await _userService.AddAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(user.Name);
        result.Email.Should().Be(user.Email);
    }

    [Fact]
    public async Task AdicionarUsuario_DeveLancarExcecaoDeValidacao_QuandoValidacaoFalhar()
    {
        // Arrange
        var request = _fixture.Create<UserRequest>();
        var user = _fixture.Create<User>();
        _mapperMock.Setup(m => m.Map<User>(request)).Returns(user);
        _validatorMock.Setup(v => v.ValidateAsync(user, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("Name", "Name is required") }));

        // Act
        Func<Task> act = async () => await _userService.AddAsync(request);

        // Assert
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    [Fact]
    public async Task AtualizarUsuario_DeveAtualizarUsuario_QuandoUsuarioExistir()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var request = _fixture.Create<UserRequest>();
        var user = _fixture.Create<User>();
        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(user);
        _validatorMock.Setup(v => v.ValidateAsync(user, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mapperMock.Setup(m => m.Map(request, user));
        _userRepositoryMock.Setup(repo => repo.UpdateAsync(user)).Returns(Task.CompletedTask);
        _mapperMock.Setup(m => m.Map<UserResponse>(user)).Returns(new UserResponse { Id = user.Id, Name = user.Name, Email = user.Email });

        // Act
        var result = await _userService.UpdateAsync(id, request);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(user.Name);
        result.Email.Should().Be(user.Email);
    }

    [Fact]
    public async Task AtualizarUsuario_DeveLancarExcecaoDeValidacao_QuandoValidacaoFalhar()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var request = _fixture.Create<UserRequest>();
        var user = _fixture.Create<User>();
        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(user);
        _validatorMock.Setup(v => v.ValidateAsync(user, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("Email", "Email is invalid") }));

        // Act
        Func<Task> act = async () => await _userService.UpdateAsync(id, request);

        // Assert
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    [Fact]
    public async Task AtualizarUsuario_DeveRetornarNulo_QuandoUsuarioNaoExistir()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var request = _fixture.Create<UserRequest>();
        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync((User)null);

        // Act
        var result = await _userService.UpdateAsync(id, request);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task DeletarUsuario_DeveDeletarUsuario_QuandoUsuarioExistir()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var user = _fixture.Create<User>();
        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(user);
        _validatorMock.Setup(v => v.ValidateAsync(user, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _userRepositoryMock.Setup(repo => repo.DeleteAsync(user)).Returns(Task.CompletedTask);

        // Act
        var result = await _userService.DeleteAsync(id);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task DeletarUsuario_DeveRetornarFalso_QuandoUsuarioNaoExistir()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync((User)null);

        // Act
        var result = await _userService.DeleteAsync(id);

        // Assert
        result.Should().BeFalse();
    }
}
