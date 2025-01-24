using AutoMapper;
using Business.Features.Auth.Commands.AuthRegister;
using Business.Wrappers;
using Common.Constants;
using Common.Entities;
using Common.Exceptions;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Handlers.Auth.Command
{
    public class AuthRegisterHandlerTests
    {
        private readonly Mock<UserManager<User>> _userManager;
        private readonly Mock<IMapper> _mapper;
        private readonly AuthRegisterCommandHandler _handler;

        public AuthRegisterHandlerTests()
        {
            var userStoreMock = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);
            _mapper = new Mock<IMapper>();
            _handler = new AuthRegisterCommandHandler(_userManager.Object, _mapper.Object);
        }
        [Fact]
        public async Task Handler_WhenValidatorFailed_ShouldReturnValidationException()
        {
            //Arrange
            var request = new AuthRegisterCommand
            {
                Email = "gmail@gmail.com",
                Password = "password123!",
                ConfirmPassword = "password12!"
            };

            //Act
            Func<Task> func = async () => await _handler.Handle(request, It.IsAny<CancellationToken>());

            //Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(func);
            Assert.Contains("ConfirmPassword is incorrect", exception.Errors);
        }

        [Fact]
        public async Task Handler_WhenUserExists_ShouldReturnValidationException()
        {
            //Arrange
            var request = new AuthRegisterCommand
            {
                Email = "gmail@gmail.com",
                Password = "password123!",
                ConfirmPassword = "password123!"
            };

            _userManager.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync(new User());
            //Act
            Func<Task> func = async () => await _handler.Handle(request, It.IsAny<CancellationToken>());

            //Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(func);
            Assert.Contains("User already exists", exception.Errors);
        }

        [Fact]
        public async Task Handler_WhenRegisterFailed_ShouldReturnValidationException()
        {
            //Arrange
            var request = new AuthRegisterCommand
            {
                Email = "gmail@gmail.com",
                Password = "password123!",
                ConfirmPassword = "password123!"
            };

            _userManager.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync(value: null);

            var user = new User { Email = request.Email };
            _mapper.Setup(x => x.Map<User>(request)).Returns(user);
            var result = _userManager.Setup(x => x.CreateAsync(user, request.Password)).ReturnsAsync(IdentityResult.Failed());

            //Act
            Func<Task> func = async () => await _handler.Handle(request, It.IsAny<CancellationToken>());

            //Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(func);
            Assert.Equal(IdentityResult.Success.Errors.Select(x => x.Description), exception.Errors);
        }
        
        [Fact]
        public async Task Handler_WhenAddToRoleFailed_ShouldReturnValidationException()
        {
            //Arrange
            var request = new AuthRegisterCommand
            {
                Email = "gmail@gmail.com",
                Password = "password123!",
                ConfirmPassword = "password123!"
            };

            _userManager.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync(value: null);

            var user = new User { Email = request.Email };
            _mapper.Setup(x => x.Map<User>(request)).Returns(user);
            var result = _userManager.Setup(x => x.CreateAsync(user, request.Password)).ReturnsAsync(IdentityResult.Success);
            var addToRoleResult = _userManager.Setup(x => x.AddToRoleAsync(user, UserRoles.User.ToString())).ReturnsAsync(IdentityResult.Failed());

            //Act
            Func<Task> func = async () => await _handler.Handle(request, It.IsAny<CancellationToken>());

            //Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(func);
            Assert.Equal(IdentityResult.Success.Errors.Select(x => x.Description), exception.Errors);
        }

        [Fact]
        public async Task Handler_WhenFlowIsSucceded_ShouldReturnValidationException()
        {
            //Arrange
            var request = new AuthRegisterCommand
            {
                Email = "gmail@gmail.com",
                Password = "password123!",
                ConfirmPassword = "password123!"
            };

            _userManager.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync(value: null);

            var user = new User { Email = request.Email };
            _mapper.Setup(x => x.Map<User>(request)).Returns(user);
            var result = _userManager.Setup(x => x.CreateAsync(user, request.Password)).ReturnsAsync(IdentityResult.Success);
            var addToRoleResult = _userManager.Setup(x => x.AddToRoleAsync(user, UserRoles.User.ToString())).ReturnsAsync(IdentityResult.Success);

            //Act
            var response = await _handler.Handle(request, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsType<Response>(response);
            Assert.Equal("User successfully added", response.Message);
        }
    }
}
