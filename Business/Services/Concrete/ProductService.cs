using AutoMapper;
using Business.Dtos.Product;
using Business.Services.Abstract;
using Business.Validators.Product;
using Business.Wrappers;
using Common.Entities;
using Common.Exceptions;
using Data.Repositories.Abstract;
using Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Concrete
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(
            IProductRepository productRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        
        public async Task<Response<ProductInfoDto>> GetProductAsync(int id)
        {
            var product = await _productRepository.GetAsync(id);
            if (product == null)
                throw new NotFoundException("Product not found");

            var response = new Response<ProductInfoDto> 
            {
                Data = _mapper.Map<ProductInfoDto>(product),
                Message = "Product found"
            };

            return response;
        }
        
        public async Task<Response> CreateProductAsync(ProductCreateDto model)
        {
            var result = await new ProductCreateDtoValidator().ValidateAsync(model);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }

            var product = await _productRepository.GetByNameAsync(model.Name);
            if (product != null) 
                throw new ValidationException("This name already exists");
           
            product = _mapper.Map<Product>(model);
            await _productRepository.CreateAsync(product);
            await _unitOfWork.CommitAsync();

            return new Response
            {
                Message = "product created succesfully"
            };
        }

        public async Task<Response<List<ProductInfoDto>>> GetAllProductsAsync()
        {
            return new Response<List<ProductInfoDto>>
            {
                Data = _mapper.Map<List<ProductInfoDto>>(await _productRepository.GetAllAsync())
            };
        }

        public async Task<Response> UpdateProductAsync(int id, ProductUpdateDto model)
        {
            var product = await _productRepository.GetAsync(id);
            if (product == null)
            {
                throw new ValidationException("Product not found");
            }

            var result = await new ProductUpdateDtoValidator().ValidateAsync(model);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }
            _mapper.Map(model, product);
            if (model.Photo != null)
            {
                product.Photo = model.Photo;
            }
            _productRepository.Update(product);
            await _unitOfWork.CommitAsync();
            return new Response
            {
                Message = "Product updated succesfully"
            };
        }

        public async Task<Response> DeleteProductAsync(int id)
        {
            var product = await _productRepository.GetAsync(id);
            if (product == null)
            {
                throw new NotFoundException("Product Not found");
            }

            _productRepository.Delete(product);
            await _unitOfWork.CommitAsync();

            return new Response
            {
                Message = "Product has been deleted"
            };
        }
    }
}
