using FluentValidation;
using Domain.Entities;

namespace Application.Validations
{
    public class ProjectValidator : AbstractValidator<Project>
    {
        public ProjectValidator()
        {
            RuleFor(project => project.Name)
                .NotEmpty().WithMessage("O nome do projeto é obrigatório.")
                .MaximumLength(100).WithMessage("O nome do projeto deve ter no máximo 100 caracteres.");

            RuleFor(project => project.Tasks)
                .Must(tasks => tasks.Count <= Project.MaxTasks)
                .WithMessage($"O projeto não pode ter mais de {Project.MaxTasks} tarefas.");

            RuleFor(project => project.Tasks)
                .Must(tasks => tasks.All(task => task.Status == "Concluída"))
                .WithMessage("Não é possível remover um projeto com tarefas pendentes. Conclua ou remova as tarefas primeiro.");
        }
    }
}
