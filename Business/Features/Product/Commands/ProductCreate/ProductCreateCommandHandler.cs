using AutoMapper;
using Business.Services.Producer;
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

namespace Business.Features.Product.Commands.ProductCreate
{
    public class ProductCreateCommandHandler : IRequestHandler<ProductCreateCommand, Response>
    {
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IProductReadRepository _productReadRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IProducerService _producerService;
        public ProductCreateCommandHandler(IProductWriteRepository productWriteRepository, 
                                           IProductReadRepository productReadRepository, 
                                           IUnitOfWork unitOfWork,
                                           IMapper mapper,
                                           IProducerService producerService)
        {
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _producerService = producerService;
        }
        public async Task<Response> Handle(ProductCreateCommand request, CancellationToken cancellationToken)
        {
            var result = await new ProductCreateCommandValidator().ValidateAsync(request);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }

            var product = await _productReadRepository.GetByNameAsync(request.Name);
            if (product != null)
                throw new ValidationException("This name already exists");

            product = _mapper.Map<Common.Entities.Product>(request);
            await _productWriteRepository.CreateAsync(product);
            await _unitOfWork.CommitAsync();

            await _producerService.ProducerAsync("Create", product);

            return new Response
            {
                Message = "product created succesfully"
            };
        }
    }
}
