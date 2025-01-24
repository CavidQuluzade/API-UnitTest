using AutoMapper;
using Business.Features.Product.Dtos;
using Business.Wrappers;
using Data.Repositories.Abstract;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Features.Product.Queries.GetAllProducts
{
    public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQuery, Response<List<ProductInfoDto>>>
    {
        private readonly IProductReadRepository _productReadRepository;
        private readonly IMapper _mapper;
        public GetAllProductQueryHandler(IMapper mapper, IProductReadRepository productReadRepository)
        {
            _mapper = mapper;
            _productReadRepository = productReadRepository;
        }
        public async Task<Response<List<ProductInfoDto>>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
        {
            return new Response<List<ProductInfoDto>>
            {
                Data = _mapper.Map<List<ProductInfoDto>>(await _productReadRepository.GetAllAsync())
            };
        }
    }
}
