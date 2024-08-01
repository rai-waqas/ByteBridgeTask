
using DataAccess.Entities;
using DataAccess.Interfaces;

namespace Business.Services
{
    public class ClientService : IClientService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClientService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Client>> GetAllClientsAsync()
        {
            return await _unitOfWork.Clients.GetAllAsync();
        }

        public async Task<Client?> GetClientByIdAsync(int id)
        {
            return await _unitOfWork.Clients.GetByIdAsync(id);
        }

        public async Task AddClientAsync(Client client)
        {
            await _unitOfWork.Clients.AddAsync(client);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateClientAsync(Client client)
        {
            await _unitOfWork.Clients.UpdateAsync(client);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteClientAsync(int id)
        {
            await _unitOfWork.Clients.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }
    }
}
