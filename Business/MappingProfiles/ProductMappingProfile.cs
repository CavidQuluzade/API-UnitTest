using AutoMapper;
using Business.Features.Product.Commands.ProductCreate;
using Business.Features.Product.Commands.ProductUpdate;
using Business.Features.Product.Dtos;
using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.MappingProfiles
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<ProductCreateCommand, Product>();
            CreateMap<Product, ProductInfoDto>();
            CreateMap<ProductUpdateCommand, Product>()
                .ForMember(dest => dest.Photo, opt =>
                {
                    opt.Condition(src => src.Photo is not null);
                    opt.MapFrom(src => src.Photo);
                });
        }
    }
}
