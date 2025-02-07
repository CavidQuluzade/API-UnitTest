using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Producer
{
    public interface IProducerService
    {
        Task ProducerAsync(string action, object data);
    }
}
