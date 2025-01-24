using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Features.Auth.Commands.AuthRegister
{
    public class AuthRegisterCommandValidator : AbstractValidator<AuthRegisterCommand>
    {
        public AuthRegisterCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Email format is incorrect");
            RuleFor(x => x.Password.Length)
                .GreaterThanOrEqualTo(8).WithMessage("Password must contain at least 8 character");
            RuleFor(x => x.Password)
                .Equal(x => x.ConfirmPassword).WithMessage("ConfirmPassword is incorrect");
        }
    }
}
