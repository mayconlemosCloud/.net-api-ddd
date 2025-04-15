using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure
{
    public class TaskManagementDbContext : DbContext
    {
        public TaskManagementDbContext(DbContextOptions<TaskManagementDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TaskEntity> Tasks { get; set; }
        public DbSet<TaskHistory> TaskHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar a chave primária e propriedades para a entidade User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
            });

            // Configurar a chave primária e propriedades para a entidade Project
            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);

                // Relacionamento com User (1:N)
                entity.HasOne<User>()
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configurar a chave primária e propriedades para a entidade TaskEntity
            modelBuilder.Entity<TaskEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Priority).IsRequired().HasMaxLength(20);

                // Relacionamento com User (1:N)
                entity.HasOne(e => e.User)
                      .WithMany(u => u.Tasks)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Relacionamento com Project (1:N)
                entity.HasOne(e => e.Project)
                      .WithMany(p => p.Tasks)
                      .HasForeignKey(e => e.ProjectId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configurar a chave primária e propriedades para a entidade TaskHistory
            modelBuilder.Entity<TaskHistory>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Changes).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.UpdatedAt).IsRequired();
                entity.Property(e => e.UserId).IsRequired();

                // Relacionamento com TaskEntity (1:N)
                entity.HasOne(e => e.TaskEntity)
                      .WithMany(t => t.Histories)
                      .HasForeignKey(e => e.TaskEntityId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configurar a chave primária e propriedades para a entidade Comment
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Content).IsRequired().HasMaxLength(500);
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.UpdatedAt).IsRequired();
                entity.Property(e => e.UserId).IsRequired();

                // Relacionamento com TaskEntity (1:N)
                entity.HasOne(e => e.TaskEntity)
                      .WithMany(t => t.Comments)
                      .HasForeignKey(e => e.TaskEntityId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }

        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries<TaskEntity>())
            {
                if (entry.State == EntityState.Modified)
                {
                    var task = entry.Entity;
                    var changes = string.Join(", ", entry.Properties
                        .Where(p => p.IsModified)
                        .Select(p => $"{p.Metadata.Name}: {p.OriginalValue} -> {p.CurrentValue}"));

                    TaskHistories.Add(new TaskHistory
                    {
                        TaskEntityId = task.Id,
                        Changes = changes,
                        UserId = Guid.NewGuid(), // Substituir pelo usuário autenticado futuramente
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    });
                }
            }

            foreach (var entry in ChangeTracker.Entries<Comment>())
            {
                if (entry.State == EntityState.Added)
                {
                    var comment = entry.Entity;
                    TaskHistories.Add(new TaskHistory
                    {
                        TaskEntityId = comment.TaskEntityId,
                        Changes = $"Comentário adicionado: {comment.Content}",
                        UserId = comment.UserId,
                        CreatedAt = comment.CreatedAt,
                        UpdatedAt = comment.UpdatedAt
                    });
                }
            }

            return base.SaveChanges();
        }
    }
}