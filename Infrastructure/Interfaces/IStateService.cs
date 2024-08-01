
using DataAccess.Entities;

namespace DataAccess.Interfaces
{
    public interface IStateService
    {
        Task<IEnumerable<State>> GetAllStatesAsync();
        Task<State?> GetStateByIdAsync(int id);
        Task AddStateAsync(State state);
        Task UpdateStateAsync(State state);
        Task DeleteStateAsync(int id);
    }
}
