
using ConsumingAPI.Database;
using ConsumingAPI.Entities;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json.Serialization;

namespace ConsumingAPI.Services.Consumer
{
    public class ConsumerService : IConsumerService
    {
        private readonly string _hostName = "localhost";
        private readonly string _queueName = "products";
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private IConnection? _connection;
        private IChannel? _channel;

        public ConsumerService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }
        
        public async Task InitializeAsync()
        {
            var factory = new ConnectionFactory { HostName = _hostName };
            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();
            await _channel.QueueDeclareAsync(_queueName, exclusive: false, autoDelete: false);
        }

        public async Task ConsumeAsync()
        {
            if (_connection is null || _channel is null)
                await InitializeAsync();

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (sender, e) =>
            {
                var body = e.Body.ToArray();
                var jsonMessage = Encoding.UTF8.GetString(body);
                var message = JsonConvert.DeserializeObject<Message>(jsonMessage);

                await ProcessMessageAsync(message);
            };

            await _channel.BasicConsumeAsync(queue: _queueName, autoAck: true, consumer: consumer);

        }

        private async Task ProcessMessageAsync(Message message)
        {
            switch (message.Action.ToLower())
            {
                case "create":
                    await CreateProductAsync(message.Data);
                    break;
                case "update":
                    await UpdateProductAsync(message.Data);
                    break;
                case "delete":
                    await DeleteProductAsync(message.Data);
                    break;
                default:
                    break;
            }
        }

        private async Task CreateProductAsync(Product product)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await dbContext.Products.AddAsync(product);
                await dbContext.SaveChangesAsync();
            }
        }


        private async Task UpdateProductAsync(Product product)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var existProduct = dbContext.Products.Find(product.Id);
                if (existProduct is not null)
                {
                    existProduct.Name = product.Name;
                    existProduct.Price = product.Price;
                    existProduct.Description = product.Description;
                    existProduct.Photo = product.Photo;

                    dbContext.Products.Update(existProduct);
                    await dbContext.SaveChangesAsync();
                }
            }
        }


        private async Task DeleteProductAsync(Product product)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var existProduct = dbContext.Products.Find(product.Id);
                if (existProduct is not null)
                {
                    dbContext.Products.Remove(existProduct);
                    await dbContext.SaveChangesAsync();
                }
            }
        }

    }

    public class Message
    {
        public string Action { get; set; }
        public Product Data { get; set; }
    }
}
