using ApplicationLayer.Events;
using DomainLayer.Events;
using MassTransit;
using MassTransit.DependencyInjection;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace InfrastructureLayer.Database.Outbox;

internal class OutboxMessageWrapperConsumer : IConsumer<OutboxMessageWrapper>
{
    private readonly MediatR.IMediator _mediator;
    private readonly Bind<IExternalBus, IPublishEndpoint> _publishEndpoint;
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly ILogger<OutboxMessageWrapperConsumer> _logger;
    public OutboxMessageWrapperConsumer(
        MediatR.IMediator mediator,
        Bind<IExternalBus, IPublishEndpoint> publishEndpoint,
        IHubContext<NotificationHub> hubContext
        )
    {
        _mediator = mediator;
        _publishEndpoint = publishEndpoint;
        _hubContext = hubContext;
    }

    public async Task Consume(ConsumeContext<OutboxMessageWrapper> context)
    {
        await RouteEvent(context.Message);
    }

    private async Task RouteEvent(OutboxMessageWrapper eventMessage)
    {
        var messageType = Type.GetType(eventMessage.MessageType);
        var @event = JsonConvert.DeserializeObject(eventMessage.Payload, messageType!);

        try
        {
            switch (@event)
            {
                case IDomainEvent:
                    await _mediator.Publish(@event);
                    break;
                case IApplicationEvent:
                    await _publishEndpoint.Value.Publish(@event);
                    break;
                case IUiEvent:
                    await _hubContext.Clients.All.SendAsync("ReceiveMessage", @event);
                    break;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while processing event");
        }
    }
}

