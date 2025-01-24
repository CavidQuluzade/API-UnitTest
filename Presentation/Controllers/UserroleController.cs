using Business.Features.Userrole.Commands.UserAddRole;
using Business.Features.Userrole.Commands.UserRemoveRole;
using Business.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserroleController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserroleController(IMediator mediator)
        {
            _mediator = mediator;
        }
        #region Documentation
        /// <summary>
        /// Add role to user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        #endregion
        [HttpPost]
        public async Task<Response> UserAddRoleAsync(UserAddRoleCommand request) => await _mediator.Send(request);

        #region Documentation
        /// <summary>
        /// Remove role from user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        #endregion
        [HttpDelete]
        public async Task<Response> UserRemoveRoleAsync(UserRemoveRoleCommand request) => await _mediator.Send(request);
    }
}
