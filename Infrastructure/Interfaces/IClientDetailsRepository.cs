using DataAccess.Entities;

namespace DataAccess.Interfaces
{
    public interface IClientDetailsRepository : IGenericRepository<Clientdetail>
    {
        IQueryable<Clientdetail> GetClientDetails();
        Task<Clientdetail?> GetClientDetailsByEmailAsync(string email);
        Task<Clientdetail?> AddClientDataAsync(Clientdetail clientdetail);
    }
}
