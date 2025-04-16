using Application.DTOs;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;

namespace Application.Services;
public class UserService : IUserService
{
    private readonly IBaseRepository<User> _userRepository;
    private readonly IMapper _mapper;

    public UserService(IBaseRepository<User> userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserResponse>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<UserResponse>>(users);
    }

    public async Task<UserResponse?> GetByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return user == null ? null : _mapper.Map<UserResponse>(user);
    }

    public async Task<UserResponse> AddAsync(UserRequest request)
    {
        var user = _mapper.Map<User>(request);
        await _userRepository.AddAsync(user);
        return _mapper.Map<UserResponse>(user);
    }

    public async Task<UserResponse?> UpdateAsync(Guid id, UserRequest request)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return null;
        _mapper.Map(request, user);
        await _userRepository.UpdateAsync(user);
        return _mapper.Map<UserResponse>(user);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return false;
        await _userRepository.DeleteAsync(user);
        return true;
    }
}