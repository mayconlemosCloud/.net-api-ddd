using Application.DTOs;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using Application.Validations;
using FluentValidation;

namespace Application.Services;
public class UserService : IUserService
{
    private readonly IBaseRepository<User> _userRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<User> _validator;

    public UserService(IBaseRepository<User> userRepository, IMapper mapper, IValidator<User> validator)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<IEnumerable<UserResponse>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<UserResponse>>(users);
    }

    public async Task<UserResponse?> GetByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return null;
        return _mapper.Map<UserResponse>(user);
    }

    public async Task<UserResponse> AddAsync(UserRequest request)
    {
        var user = _mapper.Map<User>(request);
        var validationResult = await _validator.ValidateAsync(user);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
        await _userRepository.AddAsync(user);
        return _mapper.Map<UserResponse>(user);
    }

    public async Task<UserResponse?> UpdateAsync(Guid id, UserRequest request)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return null;
        _mapper.Map(request, user);
        var validationResult = await _validator.ValidateAsync(user);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
        await _userRepository.UpdateAsync(user);
        return _mapper.Map<UserResponse>(user);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return false;
        var validationResult = await _validator.ValidateAsync(user);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
        await _userRepository.DeleteAsync(user);
        return true;
    }
}