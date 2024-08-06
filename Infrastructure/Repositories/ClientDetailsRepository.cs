
using DataAccess.DataContext;
using DataAccess.Dtos;
using DataAccess.Entities;
using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class ClientDetailsRepository : GenericRepository<Clientdetail>, IClientDetailsRepository
    {
        public ClientDetailsRepository(DBContext context) : base(context)
        {
        }
        public IQueryable<Clientdetail> GetClientDetails()
        {
            return _context.Clientdetails.AsQueryable();
        }
        public async Task<Clientdetail?> GetClientDetailsByEmailAsync(string email)
        {
            return await _context.Clientdetails.FirstOrDefaultAsync(x => x.Email == email);
        }
        public async Task<Clientdetail?> AddClientDataAsync(Clientdetail clientdetail)
        {
            if (clientdetail == null)
            {
                throw new ArgumentNullException(nameof(clientdetail));
            }
            try
            {
                _context.Clientdetails.Add(clientdetail);
                await _context.SaveChangesAsync();
                return clientdetail;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while adding client data.", ex);
            }
        }

        public Task<bool> EmailExists(string email)
        {
            return _context.Clientdetails.AnyAsync(x => x.Email == email);
        }
    }
}
