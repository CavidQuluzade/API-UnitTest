
using ConsumingAPI.Services.Consumer;

namespace ConsumingAPI.BackgroundServices
{
    public class ConsumerBackgroundService : BackgroundService
    {
        private readonly IConsumerService _consumerService;

        public ConsumerBackgroundService(IConsumerService consumerService)
        {
            _consumerService = consumerService;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _consumerService.ConsumeAsync();
                await Task.Delay(100);
            }
        }
    }
}
