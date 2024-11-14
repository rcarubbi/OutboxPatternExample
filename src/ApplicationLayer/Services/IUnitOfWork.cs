namespace ApplicationLayer.Services
{
    public interface IUnitOfWork
    {

        Task Commit(CancellationToken cancellationToken);
        Task BeginTransaction(CancellationToken cancellationToken);
    }
}
