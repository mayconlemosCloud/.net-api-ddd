using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Infrastructure
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
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Email).IsRequired();
            });

            // Configurar a chave primária e propriedades para a entidade Project
            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
            });

            // Configurar a chave primária e propriedades para a entidade TaskEntity
            modelBuilder.Entity<TaskEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired();
                entity.Property(e => e.Description).IsRequired();
                entity.Property(e => e.Status).IsRequired();
                entity.Property(e => e.Priority).IsRequired();

                // Relacionamento com User (1:N)
                entity.HasOne(e => e.User)
                      .WithMany(u => u.Tasks)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade); // Adicionar comportamento de exclusão em cascata

                // Relacionamento com Project (1:N)
                entity.HasOne(e => e.Project)
                      .WithMany(p => p.Tasks)
                      .HasForeignKey(e => e.ProjectId)
                      .OnDelete(DeleteBehavior.Cascade); // Adicionar comportamento de exclusão em cascata
            });

            // Configurar a chave primária e propriedades para a entidade TaskHistory
            modelBuilder.Entity<TaskHistory>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Changes).IsRequired();
                entity.Property(e => e.ModifiedBy).IsRequired();

                // Relacionamento com TaskEntity (1:N)
                entity.HasOne(e => e.TaskEntity)
                      .WithMany(t => t.Histories)
                      .HasForeignKey(e => e.TaskEntityId);
            });
        }
    }
}