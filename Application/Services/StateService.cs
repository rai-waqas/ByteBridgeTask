
using DataAccess.Entities;
using DataAccess.Interfaces;

namespace Business.Services
{
    public class StateService : IStateService
    {
        private readonly IUnitOfWork _unitOfWork;

        public StateService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<State>> GetAllStatesAsync()
        {
            return await _unitOfWork.States.GetAllAsync();
        }

        public async Task<State?> GetStateByIdAsync(int id)
        {
            return await _unitOfWork.States.GetByIdAsync(id);
        }

        public async Task AddStateAsync(State state)
        {
            await _unitOfWork.States.AddAsync(state);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateStateAsync(State state)
        {
            await _unitOfWork.States.UpdateAsync(state);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteStateAsync(int id)
        {
            await _unitOfWork.States.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }
    }
}
