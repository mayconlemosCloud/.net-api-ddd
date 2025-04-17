using FluentValidation;
using Domain.Entities;
using System;

namespace Application.Validations
{
    public class TaskHistoryValidator : AbstractValidator<TaskHistory>
    {
        public TaskHistoryValidator()
        {
            RuleFor(history => history.Changes)
                .NotEmpty().WithMessage("As alterações do histórico são obrigatórias.");

            RuleFor(history => history.UserId)
                .NotEmpty().WithMessage("O autor da modificação é obrigatório.");

            RuleFor(history => history.CreatedAt)
                .LessThanOrEqualTo(DateTime.Now).WithMessage("A data de modificação não pode ser no futuro.");

            RuleFor(history => history.UpdatedAt)
                .LessThanOrEqualTo(DateTime.Now).WithMessage("A data de modificação não pode ser no futuro.");
        }
    }
}
