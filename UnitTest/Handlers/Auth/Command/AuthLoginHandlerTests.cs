using AutoMapper;
using Business.Features.Auth.Commands.AuthLogin;
using Business.Features.Auth.Commands.Dtos;
using Business.Wrappers;
using Common.Constants;
using Common.Entities;
using Common.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Handlers.Auth.Command
{
    public class AuthLoginHandlerTests
    {
        private readonly Mock<UserManager<User>> _userManager;
        private readonly Mock<IConfiguration> _configuration;
        private readonly AuthLoginCommandHandler _handler;
        public AuthLoginHandlerTests()
        {
            var userStoreMock = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);
            _configuration = new Mock<IConfiguration>();
            _handler = new AuthLoginCommandHandler(_userManager.Object, _configuration.Object);
        }

        [Fact]
        public async Task Handle_WhenEmailNotFound_ShouldthrowUnauthorizedException()
        {
            //Arrange
            var request = new AuthLoginCommand
            {
                Email = "gmail@gmail.com",
                Password = "password123!"
            };

            _userManager.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync(new User());

            //Act
            Func<Task> func = async() => await _handler.Handle(request, It.IsAny<CancellationToken>());

            //Assert
            var exception = await Assert.ThrowsAsync<UnauthorizedException>(func);
            Assert.Contains("Email or password is incorrect", exception.Errors);
        }

        [Fact]
        public async Task Handle_WhenPasswordIsIncorrect_ShouldthrowUnauthorizedException()
        {
            //Arrange
            var request = new AuthLoginCommand
            {
                Email = "gmail@gmail.com",
                Password = "password123!"
            };

            _userManager.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync(value: null);

            var user = new User { Email = request.Email };
            _userManager.Setup(x => x.CheckPasswordAsync(user, request.Password)).ReturnsAsync(value: false);

            //Act
            Func<Task> func = async () => await _handler.Handle(request, It.IsAny<CancellationToken>());

            //Assert
            var exception = await Assert.ThrowsAsync<UnauthorizedException>(func);
            Assert.Contains("Email or password is incorrect", exception.Errors);
        }

        [Fact]
        public async Task Handle_WhenFlowIssucceded_ShouldReturnResponseModel()
        {
            //Arrange
            var request = new AuthLoginCommand
            {
                Email = "gmail@gmail.com",
                Password = "password123!"
            };

            _userManager.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync(new User());

            var user = new User { Email = request.Email };
            _userManager.Setup(x => x.CheckPasswordAsync(user, request.Password)).ReturnsAsync(value: true);

            _userManager.Setup(x => x.GetRolesAsync(user)).ReturnsAsync(new List<string>());

            //Act
            var response = await _handler.Handle(request, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsType<Response<AuthLoginResponseDto>>(response);
            Assert.Equal("Successfully logined", response.Message);
        }
    }
}
