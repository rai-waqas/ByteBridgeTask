using DataAccess.Dtos;
using DataAccess.Entities;

namespace DataAccess.Interfaces
{
    public interface IClientDetailsService
    {
        Task<ClientDetailsDto?> GetClientDetailsByIdAsync(int id);
        Task<(IEnumerable<ClientDetailsDto> data, int totalCount)> GetAllClientDetailsAsync(
            string? searchTerm, int pageSize, 
            int pageNumber, 
            string? sortColumn = null,
            string? sortDirection = "asc");
        Task<Clientdetail?> GetClientDetailsByEmailAsync(string email);
        Task AddClientDetailsAsync(AddClientDetailsDto clientDetailsDto);
        Task UpdateClientDetailsAsync(AddClientDetailsDto clientDetailsDto);
        Task DeleteClientDetailsAsync(int id);
        Task<bool> EmailExists(string email);
    }
}
