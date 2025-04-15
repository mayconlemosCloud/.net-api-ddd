namespace Domain.Entities
{
    public class TaskHistory
    {
        public Guid Id { get; set; }
        public string Changes { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Propriedade de navegação
        public Guid TaskEntityId { get; set; }
        public TaskEntity TaskEntity { get; set; } = null!; // Relacionamento com TaskEntity
    }
}