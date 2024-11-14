using ApplicationLayer.Events;

namespace ApplicationLayer.Services
{
    public interface IEventDispatcher
    {
        Task Dispatch(IApplicationEvent applicationEvent, CancellationToken cancellationToken);
    }
}
