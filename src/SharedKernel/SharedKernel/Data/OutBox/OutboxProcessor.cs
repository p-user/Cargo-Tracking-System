using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SharedKernel.Data.DbContext;
using System.Text.Json;

namespace SharedKernel.Data.OutBox
{
    public class OutboxProcessor<TContext> : BackgroundService
        where TContext : IApplicationDbContext
    {

        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IBus _bus;
        private readonly ILogger<OutboxProcessor<TContext>> _logger;

        public OutboxProcessor(IServiceScopeFactory serviceScopeFactory, IBus bus, ILogger<OutboxProcessor<TContext>> logger)
        {
            _serviceScopeFactory=serviceScopeFactory;
            _bus=bus;
            _logger=logger;
        }

        protected override async  Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();


                    var outboxMessages = await dbContext.OutboxMessages
                        .Where(m => m.ProcessedOn == null)
                        .ToListAsync(stoppingToken);

                    foreach (var message in outboxMessages)
                    {
                        var eventType = Type.GetType(message.Type);
                        if (eventType == null)
                        {
                            _logger.LogWarning("Could not resolve type: {Type}", message.Type);
                            continue;
                        }
                        var eventMessage = JsonSerializer.Deserialize(message.Content, eventType);
                        if (eventMessage == null)
                        {
                            _logger.LogWarning("Could not deserialize message: {Content}", message.Content);
                            continue;
                        }
                        await _bus.Publish(eventMessage, stoppingToken);

                        message.ProcessedOn = DateTime.UtcNow;

                        _logger.LogInformation("Successfully processed outbox message with ID: {Id}", message.Id);
                    }

                    await dbContext.SaveChangesAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing outbox messages");
                }

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
    }
    
}
