using FluentValidation;
using Domain.Entities;
using Domain.Repositories;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Validations
{
    public class ProjectValidator : AbstractValidator<Project>
    {
        private readonly IBaseRepository<TaskEntity> _taskRepository;

        public ProjectValidator(IBaseRepository<TaskEntity> taskRepository)
        {
            _taskRepository = taskRepository;

            RuleFor(project => project.Name)
                .NotEmpty().WithMessage("O nome do projeto é obrigatório.")
                .MaximumLength(100).WithMessage("O nome do projeto deve ter no máximo 100 caracteres.");

            RuleFor(project => project.Tasks)
                .Must(tasks => tasks.Count <= Project.MaxTasks)
                .WithMessage($"O projeto não pode ter mais de {Project.MaxTasks} tarefas.");

            RuleFor(project => project.Id)
                .MustAsync(async (projectId, cancellation) =>
                {
                    var tasks = await _taskRepository.GetAllAsync();
                    return tasks.Where(t => t.ProjectId == projectId).All(t => t.Status == "Concluída");
                })
                .WithMessage("Não é possível remover um projeto com tarefas pendentes. Conclua ou remova as tarefas primeiro.");
        }
    }
}
