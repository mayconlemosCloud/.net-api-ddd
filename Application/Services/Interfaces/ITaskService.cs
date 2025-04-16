using Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskResponse>> GetAllAsync();
        Task<TaskResponse?> GetByIdAsync(Guid id);
        Task<TaskResponse> AddAsync(TaskRequest request);
        Task<TaskResponse?> UpdateAsync(Guid id, TaskRequest request);
        Task<bool> DeleteAsync(Guid id);
    }
}