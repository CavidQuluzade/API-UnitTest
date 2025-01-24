using Business.Features.Product.Dtos;
using Business.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Features.Product.Queries.GetAllProducts
{
    public class GetAllProductQuery : IRequest<Response<List<ProductInfoDto>>>
    {
    }
}
