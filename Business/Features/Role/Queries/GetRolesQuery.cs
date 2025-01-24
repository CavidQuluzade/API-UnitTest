using Business.Features.Role.Dtos;
using Business.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Features.Role.Queries
{
    public class GetRolesQuery : IRequest<Response<List<RoleDto>>> 
    {
    }
}
