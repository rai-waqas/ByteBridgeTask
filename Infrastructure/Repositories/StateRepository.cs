using DataAccess.DataContext;
using DataAccess.Entities;
using DataAccess.Interfaces;

namespace DataAccess.Repositories
{
    public class StateRepository : GenericRepository<State>, IStateRepository
    {
        public StateRepository(DBContext context) : base(context)
        {
        }
    }
}
