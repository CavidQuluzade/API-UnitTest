using Business.Features.Auth.Commands.AuthRegister;
using Business.Features.Auth.Commands.Dtos;
using Business.Wrappers;
using Common.Constants;
using Common.Entities;
using Common.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Business.Features.Auth.Commands.AuthLogin
{
    public class AuthLoginCommandHandler : IRequestHandler<AuthLoginCommand, Response<AuthLoginResponseDto>>
    {
        private readonly UserManager<Common.Entities.User> _userManager;
        private readonly IConfiguration _configuration;

        public AuthLoginCommandHandler(UserManager<Common.Entities.User> userManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<Response<AuthLoginResponseDto>> Handle(AuthLoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                throw new UnauthorizedException("Email or password is incorrect");
            
            var isSucceded = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isSucceded)
                throw new UnauthorizedException("Email or password is incorrect");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Email)
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
            var token = new JwtSecurityToken
            (
                claims: claims,
                issuer: _configuration.GetSection("JWT:Issuer").Value,
                audience: _configuration.GetSection("JWT:Audience").Value,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );
            return new Response<AuthLoginResponseDto>
            {
                Data = new AuthLoginResponseDto
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token)
                }
            };
        }
    }
}
