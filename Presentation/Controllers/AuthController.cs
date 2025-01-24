using Business.Features.Auth.Commands.AuthRegister;
using Business.Features.Auth.Commands.Dtos;
using Business.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region Documentation
        /// <summary>
        /// User registration
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        #endregion
        [HttpPost("register")]
        public async Task<Response> RegisterAsync(AuthRegisterCommand request) => await _mediator.Send(request);

        #region Documentation
        /// <summary>
        /// User login
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<AuthLoginResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        #endregion
        [HttpPost("login")]
        public async Task<Response<AuthLoginResponseDto>> LoginAsync(Business.Features.Auth.Commands.AuthLogin.AuthLoginCommand request) => await _mediator.Send(request);
    }
}
