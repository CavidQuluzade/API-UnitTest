using Business.Wrappers;
using Common.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Features.Userrole.Commands.UserAddRole
{
    public class UserAddRoleCommandHandler : IRequestHandler<UserAddRoleCommand, Response>
    {
        private readonly UserManager<Common.Entities.User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserAddRoleCommandHandler(UserManager<Common.Entities.User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<Response> Handle(UserAddRoleCommand request, CancellationToken cancellationToken)
        {
            var result = await new UserAddRoleCommandValidator().ValidateAsync(request);
            
            if(!result.IsValid) 
                throw new ValidationException(result.Errors);
            
            var user = await _userManager.FindByIdAsync(request.UserId);
            
            if(user == null)
                throw new NotFoundException("User not found");
            
            var role = await _roleManager.FindByIdAsync(request.RoleId);
            
            if(role == null)
                throw new NotFoundException("Role not found");

            var isAlreadyExist = await _userManager.IsInRoleAsync(user, role.Name);
            if (isAlreadyExist)
                throw new ValidationException("User already in this role");

            var addToRoleResult = await _userManager.AddToRoleAsync(user, role.Name);
            if(!addToRoleResult.Succeeded)
                throw new ValidationException(addToRoleResult.Errors.Select(x => x.Description));

            return new Response
            {
                Message = "ROle successfully added to user"
            };
        }
    }
}
