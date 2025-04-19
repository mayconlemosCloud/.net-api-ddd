using Domain.Entities;

namespace Application.DTOs
{
    public class TaskResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }
        public List<CommentResponse>? Comments { get; set; }
    }

    public class TaskRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }
        public Guid UserId { get; set; }
        public List<string>? Comments { get; set; } // Apenas o conteúdo dos comentários
    }

    public class CommentResponse
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public Guid TaskEntityId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public Guid UserId { get; set; }
    }
}