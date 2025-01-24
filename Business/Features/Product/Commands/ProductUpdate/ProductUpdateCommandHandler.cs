using AutoMapper;
using Business.Wrappers;
using Common.Exceptions;
using Data.Repositories.Abstract;
using Data.UnitOfWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Features.Product.Commands.ProductUpdate
{
    public class ProductUpdateCommandHandler : IRequestHandler<ProductUpdateCommand, Response>
    {
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IProductReadRepository _productReadRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProductUpdateCommandHandler(IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response> Handle(ProductUpdateCommand request, CancellationToken cancellationToken)
        {
            var product = await _productReadRepository.GetAsync(request.Id);
            if (product == null)
            {
                throw new ValidationException("Product not found");
            }

            var result = await new ProductUpdateCommandValidator().ValidateAsync(request);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }
            _mapper.Map(request, product);
            if (request.Photo != null)
            {
                product.Photo = request.Photo;
            }
            _productWriteRepository.Update(product);
            await _unitOfWork.CommitAsync();
            return new Response
            {
                Message = "Product updated succesfully"
            };
        }
    }
}
