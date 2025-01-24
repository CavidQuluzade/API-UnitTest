using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Features.Userrole.Commands.UserAddRole
{
    public class UserAddRoleCommandValidator : AbstractValidator<UserAddRoleCommand>
    {
        public UserAddRoleCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("User must be entered");
            RuleFor(x => x.RoleId).NotEmpty().WithMessage("Role must be entered");
        }
    }
}
