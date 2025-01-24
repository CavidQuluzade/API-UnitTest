using AutoMapper;
using Business.Features.User.Dtos;
using Business.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Features.User.Queries
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, Response<List<UserDto>>>
    {
        private readonly UserManager<Common.Entities.User> _userManager;
        private readonly IMapper _mapper;
        public GetAllUsersQueryHandler(UserManager<Common.Entities.User> userManager, IMapper mapper)
        {
            _mapper = mapper;
            _userManager = userManager;
        }
        public async Task<Response<List<UserDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            return new Response<List<UserDto>>
            {
                Data = _mapper.Map<List<UserDto>>(await _userManager.Users.ToListAsync())
            };
        }
    }
}
