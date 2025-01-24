using AutoMapper;
using Business.Features.Product.Dtos;
using Business.Wrappers;
using Common.Exceptions;
using Data.Repositories.Abstract;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Features.Product.Queries.GetProduct
{
    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, Response<ProductInfoDto>>
    {
        private readonly IProductReadRepository _productReadRepository;
        private readonly IMapper _mapper;
        public GetProductQueryHandler(IProductReadRepository productReadRepository, IMapper mapper)
        {
            _mapper = mapper;
            _productReadRepository = productReadRepository;
        }
        public async Task<Response<ProductInfoDto>> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var product = await _productReadRepository.GetAsync(request.Id);
            if (product == null)
                throw new NotFoundException("Product not found");

            var response = new Response<ProductInfoDto>
            {
                Data = _mapper.Map<ProductInfoDto>(product),
                Message = "Product found"
            };

            return response;
        }
    }
}
