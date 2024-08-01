using DataAccess.DataContext;
using DataAccess.Entities;
using DataAccess.Interfaces;

namespace DataAccess.Repositories
{
    public class ClientRepository : GenericRepository<Client>, IClientRepository
    {
        public ClientRepository(DBContext context) : base(context)
        {
        }

    }
}
