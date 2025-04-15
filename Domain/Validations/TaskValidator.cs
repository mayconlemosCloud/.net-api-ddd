using FluentValidation;
using Domain.Entities;

namespace Domain.Validations
{
    public class TaskValidator : AbstractValidator<TaskEntity>
    {
        public TaskValidator()
        {
            RuleFor(task => task.Title)
                .NotEmpty().WithMessage("O título da tarefa é obrigatório.")
                .MaximumLength(100).WithMessage("O título da tarefa deve ter no máximo 100 caracteres.");

            RuleFor(task => task.Description)
                .MaximumLength(500).WithMessage("A descrição da tarefa deve ter no máximo 500 caracteres.");

            RuleFor(task => task.DueDate)
                .GreaterThanOrEqualTo(DateTime.Now).WithMessage("A data de vencimento deve ser no futuro.");

            RuleFor(task => task.Priority)
                .NotEmpty().WithMessage("A prioridade da tarefa é obrigatória.")
                .Must(priority => new[] { "Baixa", "Média", "Alta" }.Contains(priority))
                .WithMessage("A prioridade deve ser 'Baixa', 'Média' ou 'Alta'.");

            RuleFor(task => task.Priority)
                .NotEmpty().WithMessage("A prioridade da tarefa é obrigatória.")
                .Must((task, priority) => string.IsNullOrEmpty(task.Priority) || task.Priority == priority)
                .WithMessage("Não é permitido alterar a prioridade de uma tarefa depois que ela foi criada.");
        }
    }
}