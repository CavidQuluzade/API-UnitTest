using Business.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Features.Product.Commands.DeleteProduct
{
    public class DeleteProductCommand : IRequest<Response>
    {
        public int Id { get; set; }
    }
}
