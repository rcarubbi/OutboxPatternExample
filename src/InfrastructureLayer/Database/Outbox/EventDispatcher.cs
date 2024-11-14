using ApplicationLayer.Events;
using ApplicationLayer.Services;
using MassTransit;
using Newtonsoft.Json;

namespace InfrastructureLayer.Database.Outbox
{
    internal class EventDispatcher(IPublishEndpoint publishEndpoint) : IEventDispatcher
    {
        public async Task Dispatch(IApplicationEvent applicationEvent, CancellationToken cancellationToken)
        {
            await publishEndpoint.Publish(new OutboxMessageWrapper
            {
                MessageType = applicationEvent.GetType().AssemblyQualifiedName!,
                Payload = JsonConvert.SerializeObject(applicationEvent)
            }, cancellationToken);
        }
    }
}
