using File = DataAccess.Entities.File;
namespace DataAccess.Interfaces
{
    public interface IFilesRepository : IGenericRepository<File>
    {
        Task<IEnumerable<File>> GetFilesByClientDetailsIdAsync(int clientDetailsId);
        Task<IEnumerable<File>> AddAllAsync(IEnumerable<File> files);
        Task DeleteAllFilesByClientDetailsIdAsync(int clientDetailsId);
        Task<File?> GetFileByName(string name);
    }
}
