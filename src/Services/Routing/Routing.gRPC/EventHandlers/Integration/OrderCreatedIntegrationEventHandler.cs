using MassTransit;
using SharedKernel.Messaging.Events;


namespace Routing.gRPC.EventHandlers.Integration
{
    public class OrderCreatedIntegrationEventHandler(ILogger<OrderCreatedIntegrationEventHandler> _logger, IRoutingApplicationService _routingService) : IConsumer<DeliveryOrderCreatedIntegrationEvent>
    {
        public async Task Consume(ConsumeContext<DeliveryOrderCreatedIntegrationEvent> context)
        {
            throw new NotImplementedException();
            //var message = context.Message;
            //_logger.LogInformation("Delivery order created event for order id {orderId} arrived", context.Message.OrderId);

            //var createRouteRequest = new  //CreateRouteRequest
            //{
            //    OrderId= context.Message.OrderId.ToString(),
            //    Origin=context.Message.Origin,
            //    Destination= context.Message.Destination
            //};
            //var response = await _routingService.CreateRoute(createRouteRequest);
            //_logger.LogInformation("Route created for  order id {orderId} ", context.Message.OrderId);


        }
    }
}
