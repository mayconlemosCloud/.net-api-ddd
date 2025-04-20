using Application.DTOs;

namespace Application.Services.Interfaces
{
    public interface IReportService
    {
        Task<IEnumerable<UserPerformanceReport>> GetUserPerformanceReportAsync();
    }
}
