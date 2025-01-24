using AutoMapper;
using Business.Features.Auth.Commands.AuthLogin;
using Business.Features.Product.Dtos;
using Business.Features.Product.Queries.GetProduct;
using Business.Wrappers;
using Common.Entities;
using Common.Exceptions;
using Data.Repositories.Abstract;
using Data.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Handlers.Product.Query
{
    public class GetProductQueryTests
    {
        private readonly Mock<IProductReadRepository> _productReadRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly GetProductQueryHandler _handler;
        public GetProductQueryTests()
        {
            _mapper = new Mock<IMapper>();
            _productReadRepository = new Mock<IProductReadRepository>();
            _handler = new GetProductQueryHandler(_productReadRepository.Object, _mapper.Object);
        }

        [Fact]
        public async Task Service_WhenProductNotFound_ShouldReturnNotFoundException()
        {
            //Arrange
            var request = new GetProductQuery
            {
                Id = 3
            };
            _productReadRepository.Setup(x => x.GetAsync(request.Id)).ReturnsAsync(value: null);

            //Act
            Func<Task> func = async () => await _handler.Handle(request, It.IsAny<CancellationToken>());

            //Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(func);
            Assert.Contains("Product not found", exception.Errors);
        }

        [Fact]
        public async Task Service_WhenFlowIsSucceded_ShouldReturnNotFoundException()
        {
            //Arrange
            var request = new GetProductQuery
            {
                Id = 3
            };
            _productReadRepository.Setup(x => x.GetAsync(request.Id)).ReturnsAsync(new Common.Entities.Product());

            //Act
            var response = await _handler.Handle(request, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsType<Response<ProductInfoDto>>(response);
            Assert.Equal("Product found", response.Message);
        }
    }
}
