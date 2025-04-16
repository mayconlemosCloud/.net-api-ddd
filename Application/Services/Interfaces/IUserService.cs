using Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponse>> GetAllAsync();
        Task<UserResponse?> GetByIdAsync(Guid id);
        Task<UserResponse> AddAsync(UserRequest request);
        Task<UserResponse?> UpdateAsync(Guid id, UserRequest request);
        Task<bool> DeleteAsync(Guid id);
    }
}