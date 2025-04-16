using Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectResponse>> GetAllAsync();
        Task<ProjectResponse?> GetByIdAsync(Guid id);
        Task<ProjectResponse> AddAsync(ProjectRequest request);
        Task<ProjectResponse?> UpdateAsync(Guid id, ProjectRequest request);
        Task<bool> DeleteAsync(Guid id);
    }
}