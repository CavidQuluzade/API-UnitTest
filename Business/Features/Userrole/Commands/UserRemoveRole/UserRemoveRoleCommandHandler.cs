using Business.Features.Userrole.Commands.UserAddRole;
using Business.Wrappers;
using Common.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Features.Userrole.Commands.UserRemoveRole
{
    public class UserRemoveRoleCommandHandler : IRequestHandler<UserRemoveRoleCommand, Response>
    {
        private readonly UserManager<Common.Entities.User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserRemoveRoleCommandHandler(UserManager<Common.Entities.User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task<Response> Handle(UserRemoveRoleCommand request, CancellationToken cancellationToken)
        {
            var result = await new UserRemoveRoleCommandValidator().ValidateAsync(request);

            if (!result.IsValid)
                throw new ValidationException(result.Errors);

            var user = await _userManager.FindByIdAsync(request.UserId);

            if (user == null)
                throw new NotFoundException("User not found");

            var role = await _roleManager.FindByIdAsync(request.RoleId);

            if (role == null)
                throw new NotFoundException("Role not found");

            var isInRole = await _userManager.IsInRoleAsync(user, role.Name);
            if (!isInRole)
                throw new ValidationException("User don't have this role");

            var removeResult = await _userManager.RemoveFromRoleAsync(user, role.Name);
            if (!removeResult.Succeeded)
                throw new ValidationException(removeResult.Errors.Select(x => x.Description));

            return new Response
            {
                Message = "Role successfully deleted from user"
            };
        }
    }
}
