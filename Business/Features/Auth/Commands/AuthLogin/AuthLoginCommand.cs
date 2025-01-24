using Business.Features.Auth.Commands.Dtos;
using Business.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Features.Auth.Commands.AuthLogin
{
    public class AuthLoginCommand : IRequest<Response<AuthLoginResponseDto>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
