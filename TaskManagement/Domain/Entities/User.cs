using System.Collections.Generic;

namespace TaskManagement.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // Propriedade de navegação para o relacionamento com TaskEntity
        public ICollection<TaskEntity> Tasks { get; set; } = new List<TaskEntity>();
    }
}