
namespace DataAccess.Interfaces
{
    public interface IUnitOfWork
    {
        IClientRepository Clients { get; }
        IStateRepository States { get; }
        IClientDetailsRepository ClientDetails { get; }
        IFilesRepository Files { get; }

        Task<int> CompleteAsync();
        Task BeginTransactionAsync(); 
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
