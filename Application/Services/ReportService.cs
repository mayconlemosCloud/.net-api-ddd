using Application.DTOs;
using Domain.Entities;
using Domain.Repositories;
using Application.Services.Interfaces;

namespace Application.Services
{
    public class ReportService : IReportService
    {
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<TaskEntity> _taskRepository;

        public ReportService(IBaseRepository<User> userRepository, IBaseRepository<TaskEntity> taskRepository)
        {
            _userRepository = userRepository;
            _taskRepository = taskRepository;
        }

        public async Task<IEnumerable<UserPerformanceReport>> GetUserPerformanceReportAsync()
        {
            var users = await _userRepository.GetAllAsync();
            var now = DateTime.UtcNow;
            var last30Days = now.AddDays(-30);

            var tasks = await _taskRepository.GetAllAsync();

            var report = users.Select(user =>
            {
                var completedTasks = tasks
                    .Where(t => t.UserId == user.Id
                        && string.Equals(t.Status?.Trim(), "Concluída", StringComparison.OrdinalIgnoreCase)
                        && t.UpdatedAt >= last30Days
                        && t.UpdatedAt <= now)
                    .Count();

                var avg = completedTasks / 30.0;

                return new UserPerformanceReport
                {
                    UserId = user.Id,
                    UserName = user.Name,
                    AverageCompletedTasksLast30Days = avg
                };
            });

            return report;
        }
    }
}
