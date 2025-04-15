using System.Collections.Generic;

namespace Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public ICollection<TaskEntity> Tasks { get; set; } = new List<TaskEntity>();
    }
}