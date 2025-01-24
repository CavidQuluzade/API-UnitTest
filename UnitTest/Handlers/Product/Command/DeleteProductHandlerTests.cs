using AutoMapper;
using Business.Features.Product.Commands.DeleteProduct;
using Business.Wrappers;
using Common.Exceptions;
using Data.Repositories.Abstract;
using Data.UnitOfWork;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Handlers.Product.Command
{
    public class DeleteProductHandlerTests
    {
        private readonly Mock<IProductWriteRepository> _productWriteRepository;
        private readonly Mock<IProductReadRepository> _productReadRepository;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly DeleteProductCommandHandler _handler;
        public DeleteProductHandlerTests()
        {
            _productWriteRepository = new Mock<IProductWriteRepository>();
            _productReadRepository = new Mock<IProductReadRepository>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _handler = new DeleteProductCommandHandler(_productWriteRepository.Object, _productReadRepository.Object, _unitOfWork.Object);
        }

        [Fact]
        public async Task Service_WhenProductNotFound_ShouldThrowNotFoundException()
        {
            var request = new DeleteProductCommand
            {
                Id = 3
            };
            //Arrange
            _productReadRepository.Setup(x => x.GetAsync(request.Id)).ReturnsAsync(value: null);

            //Act
            Func<Task> func = async () => await _handler.Handle(request, It.IsAny<CancellationToken>());

            //Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(func);
            Assert.Contains("Product Not found", exception.Errors);
        }

        [Fact]
        public async Task Service_WhenFlowIsSucceded_ShouldReturnResponse()
        {
            var request = new DeleteProductCommand
            {
                Id = 3
            };
            //Arrange
            _productReadRepository.Setup(x => x.GetAsync(request.Id)).ReturnsAsync(new Common.Entities.Product());

            //Act
            var response = await _handler.Handle(request, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsType<Response>(response);
            Assert.Contains("Product has been deleted", response.Message);
        }
    }
}
