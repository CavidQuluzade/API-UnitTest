using Business.Dtos.Product;
using Business.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Abstract
{
    public interface IProductService
    {
        Task<Response> CreateProductAsync(ProductCreateDto model);
        Task<Response<ProductInfoDto>> GetProductAsync(int id);
        Task<Response<List<ProductInfoDto>>> GetAllProductsAsync();
        Task<Response> UpdateProductAsync(int id, ProductUpdateDto model);
        Task<Response> DeleteProductAsync(int id);
    }
}
