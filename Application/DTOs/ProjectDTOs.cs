namespace Application.DTOs
{
    public class ProjectResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public ICollection<TaskResponse> Tasks { get; set; } = new List<TaskResponse>();
    }

    public class ProjectRequest
    {
        public string Name { get; set; } = string.Empty;
        public Guid UserId { get; set; }
    }
}