using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Producer
{
    public class ProducerService : IProducerService
    {
        private readonly string _hostName = "localhost";
        private readonly string _queueName = "products";
        public async Task ProducerAsync(string action, object data)
        {
            var factory = new ConnectionFactory { HostName = _hostName };
            using (var connection = await factory.CreateConnectionAsync())
            {
                using (var channel = await connection.CreateChannelAsync())
                {
                    await channel.QueueDeclareAsync(_queueName, exclusive: false, autoDelete: false);
                    var value = new
                    {
                        action = action,
                        data = data
                    };

                    var json = JsonConvert.SerializeObject(value);
                    var message = Encoding.UTF8.GetBytes(json);

                    await channel.BasicPublishAsync(exchange: "", routingKey: _queueName, body: message);
                }
            }
        }
    }
}
