namespace Application.DTOs
{
    public class UserPerformanceReport
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public double AverageCompletedTasksLast30Days { get; set; }
    }
}
