using Business.Features.Product.Commands.DeleteProduct;
using Business.Features.Product.Commands.ProductCreate;
using Business.Features.Product.Commands.ProductUpdate;
using Business.Features.Product.Dtos;
using Business.Features.Product.Queries.GetAllProducts;
using Business.Features.Product.Queries.GetProduct;
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
    [Authorize(Roles = "Seller", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region Documentation
        /// <summary>
        /// To get product
        /// </summary>
        /// <remarks>
        /// <ul>
        /// <li><b>Type</b></li>
        /// <li>0 - new</li>
        /// <li>1 - sold</li>
        /// </ul>
        /// </remarks>
        /// <param name="id"></param>
        [ProducesResponseType(typeof(Response<ProductInfoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        #endregion
        [HttpGet("{id}")]
        public async Task<Response<ProductInfoDto>> GetProductAsync(int id) => await _mediator.Send(new GetProductQuery { Id = id});

        #region Documentation
        /// <summary>
        /// Get All Products
        /// </summary>
        /// <remarks>
        /// <ul>
        /// <li><b>Type</b></li>
        /// <li>0 - new</li>
        /// <li>1 - sold</li>
        /// </ul>
        /// </remarks>
        [ProducesResponseType(typeof(Response<List<ProductInfoDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        #endregion
        [HttpGet]
        public async Task<Response<List<ProductInfoDto>>> GetAllProductsAsync() => await _mediator.Send(new GetAllProductQuery());

        #region Documentation
        /// <summary>
        /// To create product
        /// </summary>
        /// <remarks>
        /// <ul>
        /// <li><b>Type</b></li>
        /// <li>0 - new</li>
        /// <li>1 - sold</li>
        /// </ul>
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        #endregion
        [HttpPost]
        public async Task<Response> CreateProductAsync(ProductCreateCommand request)=> await _mediator.Send(request);

        #region Documentation
        /// <summary>
        /// To update product
        /// </summary>
        /// <remarks>
        /// <ul>
        /// <li><b>Type</b></li>
        /// <li>0 - new</li>
        /// <li>1 - sold</li>
        /// </ul>
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        #endregion
        [HttpPut("{id}")]
        public async Task<Response> ProductUpdateAsync(int id, ProductUpdateCommand request)
        {
            request.Id = id;
            return await _mediator.Send(request);
        }

        #region Documentation
        /// <summary>
        /// To update product
        /// </summary>
        /// <remarks>
        /// <ul>
        /// <li><b>Type</b></li>
        /// <li>0 - new</li>
        /// <li>1 - sold</li>
        /// </ul>
        /// </remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        #endregion
        [HttpDelete("{id}")]
        public async Task<Response> ProductDeleteAsync(int id) => await _mediator.Send(new DeleteProductCommand { Id = id});
    }
}
