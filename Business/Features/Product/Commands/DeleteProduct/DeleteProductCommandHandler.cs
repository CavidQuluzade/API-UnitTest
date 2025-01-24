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

namespace Business.Features.Product.Commands.DeleteProduct
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Response>
    {
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IProductReadRepository _productReadRepository;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteProductCommandHandler(IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository, IUnitOfWork unitOfWork)
        {
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Response> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productReadRepository.GetAsync(request.Id);
            if (product == null)
            {
                throw new NotFoundException("Product Not found");
            }

            _productWriteRepository.Delete(product);
            await _unitOfWork.CommitAsync();

            return new Response
            {
                Message = "Product has been deleted"
            };
        }
    }
}
