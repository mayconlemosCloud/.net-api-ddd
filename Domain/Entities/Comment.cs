using System;

namespace Domain.Entities
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public Guid TaskEntityId { get; set; }
        public TaskEntity TaskEntity { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public Guid UserId { get; set; }
    }
}