using FluentValidation;
using Domain.Entities;

namespace Domain.Validations
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(user => user.Name)
                .NotEmpty().WithMessage("O nome do usuário é obrigatório.")
                .MaximumLength(100).WithMessage("O nome do usuário deve ter no máximo 100 caracteres.");

            RuleFor(user => user.Email)
                .NotEmpty().WithMessage("O e-mail do usuário é obrigatório.")
                .EmailAddress().WithMessage("O e-mail deve ser válido.");
        }
    }
}