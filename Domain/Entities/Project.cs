namespace Domain.Entities
{
    public class Project
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid UserId { get; set; }
        public ICollection<TaskEntity> Tasks { get; set; } = new List<TaskEntity>();

        public const int MaxTasks = 20; // Limite máximo de tarefas por projeto
    }
}