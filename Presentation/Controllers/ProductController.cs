using Business.Dtos.Product;
using Business.Services.Abstract;
using Business.Wrappers;
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
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
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
        public async Task<Response<ProductInfoDto>> GetProductAsync(int id) => await _productService.GetProductAsync(id);

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
        public async Task<Response<List<ProductInfoDto>>> GetAllProductsAsync() => await _productService.GetAllProductsAsync();

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
        /// <param name="model"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        #endregion
        [HttpPost]
        public async Task<Response> CreateProductAsync(ProductCreateDto model)=> await _productService.CreateProductAsync(model);

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
        /// <param name="model"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        #endregion
        [HttpPut("{id}")]
        public async Task<Response> ProductUpdateAsync(int id, ProductUpdateDto model) => await _productService.UpdateProductAsync(id, model);

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
        public async Task<Response> ProductDeleteAsync(int id) => await _productService.DeleteProductAsync(id);
    }
}
