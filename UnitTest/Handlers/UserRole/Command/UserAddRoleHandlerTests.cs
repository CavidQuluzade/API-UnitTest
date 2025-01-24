using Business.Features.Userrole.Commands.UserAddRole;
using Business.Wrappers;
using Common.Entities;
using Common.Exceptions;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Handlers.UserRole.Command
{
    public class UserAddRoleHandlerTests
    {
        private readonly Mock<UserManager<User>> _userManager;
        private readonly Mock<RoleManager<IdentityRole>> _roleManager;
        private readonly UserAddRoleCommandHandler _handler;

        public UserAddRoleHandlerTests()
        {
            var userStoreMock = new Mock<IUserStore<User>>();
            var roleStoreMock = new Mock<IRoleStore<IdentityRole>>();
            _userManager = new Mock<UserManager<User>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);
            _roleManager = new Mock<RoleManager<IdentityRole>>(
                roleStoreMock.Object, null, null, null, null);
            _handler = new UserAddRoleCommandHandler(_userManager.Object, _roleManager.Object);
        }

        [Fact]
        public async Task Handler_WhenValidationFailed_ShouldThrowValidationException()
        {
            //Arrange
            var request = new UserAddRoleCommand
            {
                UserId = "",
                RoleId = "ffmbfb231wc"
            };

            //Act
            Func<Task> func = async() => await _handler.Handle(request, It.IsAny<CancellationToken>());
            //Assert
            var exception = await Assert.ThrowsAnyAsync<ValidationException>(func);
            Assert.Contains("User must be entered", exception.Errors);
        }

        [Fact]
        public async Task Handler_WhenUserNotFound_ShouldThrowNotFoundException()
        {
            //Arrange
            var request = new UserAddRoleCommand
            {
                UserId = "bdgboinbb",
                RoleId = "ffmbfb231wc"
            };

            _userManager.Setup(x => x.FindByIdAsync(request.UserId)).ReturnsAsync(value: null);
            //Act
            Func<Task> func = async () => await _handler.Handle(request, It.IsAny<CancellationToken>());
            //Assert
            var exception = await Assert.ThrowsAnyAsync<NotFoundException>(func);
            Assert.Contains("User not found", exception.Errors);
        }

        [Fact]
        public async Task Handler_WhenRoleNotFound_ShouldThrowNotFoundException()
        {
            //Arrange
            var request = new UserAddRoleCommand
            {
                UserId = "bdfbfdbfbn",
                RoleId = "ffmbfb231wc"
            };

            _userManager.Setup(x => x.FindByIdAsync(request.UserId)).ReturnsAsync(new User());

            _roleManager.Setup(x => x.FindByIdAsync(request.RoleId)).ReturnsAsync(value: null);

            //Act
            Func<Task> func = async () => await _handler.Handle(request, It.IsAny<CancellationToken>());
            //Assert
            var exception = await Assert.ThrowsAnyAsync<NotFoundException>(func);
            Assert.Contains("Role not found", exception.Errors);
        }

        [Fact]
        public async Task Handler_WhenUserAlreadyInRole_ShouldThrowValidationException()
        {
            //Arrange
            var request = new UserAddRoleCommand
            {
                UserId = "bdfbfdbfbn",
                RoleId = "ffmbfb231wc"
            };

            var role = new IdentityRole
            {
                Id = "ffmbfb231wc",
                Name = "Test"
            };

            var user = new User
            {
                Id = "bdfbfdbfbn"
            };

            _userManager.Setup(x => x.FindByIdAsync(request.UserId)).ReturnsAsync(user);

            _roleManager.Setup(x => x.FindByIdAsync(request.RoleId)).ReturnsAsync(role);

            _userManager.Setup(x => x.IsInRoleAsync(user, role.Name)).ReturnsAsync(value: true);
            //Act
            Func<Task> func = async () => await _handler.Handle(request, It.IsAny<CancellationToken>());
            //Assert
            var exception = await Assert.ThrowsAnyAsync<ValidationException>(func);
            Assert.Contains("User already in this role", exception.Errors);
        }

        [Fact]
        public async Task Handler_WhenAddToRoleFailed_ShouldThrowValidationException()
        {
            //Arrange
            var request = new UserAddRoleCommand
            {
                UserId = "bdfbfdbfbn",
                RoleId = "ffmbfb231wc"
            };

            var role = new IdentityRole
            {
                Id = "ffmbfb231wc",
                Name = "Test"
            };

            var user = new User
            {
                Id = "bdfbfdbfbn"
            };

            _userManager.Setup(x => x.FindByIdAsync(request.UserId)).ReturnsAsync(user);

            _roleManager.Setup(x => x.FindByIdAsync(request.RoleId)).ReturnsAsync(role);

            _userManager.Setup(x => x.IsInRoleAsync(user, role.Name)).ReturnsAsync(value: false);

            _userManager.Setup(x => x.AddToRoleAsync(user, role.Name)).ReturnsAsync(IdentityResult.Failed());
            
            //Act
            Func<Task> func = async () => await _handler.Handle(request, It.IsAny<CancellationToken>());
            
            //Assert
            var exception = await Assert.ThrowsAnyAsync<ValidationException>(func);
            Assert.Equal(IdentityResult.Success.Errors.Select(x => x.Description), exception.Errors);
        }

        [Fact]
        public async Task Handler_WhenFlowIsSucceded_ShouldReturnResponse()
        {
            //Arrange
            var request = new UserAddRoleCommand
            {
                UserId = "bdfbfdbfbn",
                RoleId = "ffmbfb231wc"
            };

            var role = new IdentityRole
            {
                Id = "ffmbfb231wc",
                Name = "Test"
            };

            var user = new User
            {
                Id = "bdfbfdbfbn"
            };

            _userManager.Setup(x => x.FindByIdAsync(request.UserId)).ReturnsAsync(user);

            _roleManager.Setup(x => x.FindByIdAsync(request.RoleId)).ReturnsAsync(role);

            _userManager.Setup(x => x.IsInRoleAsync(user, role.Name)).ReturnsAsync(value: false);

            _userManager.Setup(x => x.AddToRoleAsync(user, role.Name)).ReturnsAsync(IdentityResult.Success);

            //Act
            var response = await _handler.Handle(request, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsType<Response>(response);
            Assert.Equal("ROle successfully added to user", response.Message);
        }
    }
}
