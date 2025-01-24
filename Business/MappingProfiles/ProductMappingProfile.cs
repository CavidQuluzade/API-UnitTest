using AutoMapper;
using Business.Dtos.Product;
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
            CreateMap<ProductCreateDto, Product>();
            CreateMap<Product, ProductInfoDto>();
            CreateMap<ProductUpdateDto, Product>()
                .ForMember(dest => dest.Photo, opt =>
                {
                    opt.Condition(src => src.Photo is not null);
                    opt.MapFrom(src => src.Photo);
                });
        }
    }
}
