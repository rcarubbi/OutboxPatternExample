using ApplicationLayer.Services;

namespace InfrastructureLayer.Database
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            
            _dbContext = dbContext;
        }

        public async Task BeginTransaction(CancellationToken cancellationToken)
        {
            await _dbContext.Database.BeginTransactionAsync();
        }

        public async Task Commit(CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
            if (_dbContext.Database.CurrentTransaction is not null)
            {
                await _dbContext.Database.CurrentTransaction.CommitAsync(cancellationToken);
            }
        }
    }
}
