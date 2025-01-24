using AutoMapper;
using Business.Dtos.Product;
using Business.Services.Concrete;
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

namespace UnitTest.Services.Product
{
    public class DeleteProductServiceTests
    {
        private readonly Mock<IProductRepository> _productRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly ProductService _handler;
        public DeleteProductServiceTests()
        {
            _productRepository = new Mock<IProductRepository>();
            _mapper = new Mock<IMapper>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _handler = new ProductService(_productRepository.Object, _unitOfWork.Object, _mapper.Object);
        }

        [Fact]
        public async Task Service_WhenProductNotFound_ShouldThrowNotFoundException()
        {
            var request = new ProductInfoDto
            {
                Id = 3
            };
            //Arrange
            _productRepository.Setup(x => x.GetAsync(request.Id)).ReturnsAsync(value: null);

            //Act
            Func<Task> func = async () => await _handler.DeleteProductAsync(request.Id);

            //Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(func);
            Assert.Contains("Product Not found", exception.Errors);
        }

        [Fact]
        public async Task Service_WhenFlowIsSucceded_ShouldReturnResponse()
        {
            var request = new ProductInfoDto
            {
                Id = 3
            };
            //Arrange
            _productRepository.Setup(x => x.GetAsync(request.Id)).ReturnsAsync(new Common.Entities.Product());

            //Act
            var response = await _handler.DeleteProductAsync(request.Id);

            //Assert
            Assert.IsType<Response>(response);
            Assert.Contains("Product has been deleted", response.Message);
        }
    }
}
