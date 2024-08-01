using DataAccess.DataContext;
using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DBContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(DBContext context) 
        {
            _context = context;
            Clients = new ClientRepository(_context);
            States = new StateRepository(_context);
            ClientDetails = new ClientDetailsRepository(_context);
            Files = new FilesRepository(_context);
        }

        public IClientRepository Clients { get; private set; }
        public IStateRepository States { get; private set; }
        public IClientDetailsRepository ClientDetails { get; private set; }
        public IFilesRepository Files { get; private set; }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                _transaction.Dispose();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                _transaction.Dispose();
                _transaction = null;
            }
        }
    }
}
