namespace Domain.Entities
{
    public class TaskHistory
    {
        public Guid Id { get; set; }
        public string Changes { get; set; } = string.Empty; // Inicializar como string vazia
        public string ModifiedBy { get; set; } = string.Empty; // Inicializar como string vazia
        public DateTime ModifiedAt { get; set; }

        // Propriedade de navegação
        public Guid TaskEntityId { get; set; }
        public TaskEntity TaskEntity { get; set; } = null!; // Relacionamento com TaskEntity
    }
}