using System.Collections.Generic;

namespace Domain.Entities
{
    public class TaskEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }

        // Propriedades de navegação
        public Guid UserId { get; set; }
        public User User { get; set; } = null!; // Relacionamento com User
        public Guid ProjectId { get; set; }
        public Project Project { get; set; } = null!; // Relacionamento com Project
        public ICollection<TaskHistory> Histories { get; set; } = new List<TaskHistory>(); // Relacionamento com TaskHistory
        public ICollection<Comment> Comments { get; set; } = new List<Comment>(); // Relacionamento com Comment

    }
}