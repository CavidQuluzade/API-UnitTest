using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Features.Userrole.Commands.UserRemoveRole
{
    public class UserRemoveRoleCommandValidator : AbstractValidator<UserRemoveRoleCommand>
    {
        public UserRemoveRoleCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("User must be entered");
            RuleFor(x => x.RoleId).NotEmpty().WithMessage("Role must be entered");
        }
    }
}
