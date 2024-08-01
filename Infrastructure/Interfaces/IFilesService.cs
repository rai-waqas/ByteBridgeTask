using File = DataAccess.Entities.File;

namespace DataAccess.Interfaces
{
    public interface IFilesService
    {
        Task<File?> GetFileByIdAsync(int id);
        Task<IEnumerable<File>> GetFilesByClientDetailsIdAsync(int clientId);
        Task<IEnumerable<File>> AddFilesAsync(IEnumerable<File> files);
        Task AddFileAsync(File file);
        Task UpdateFileAsync(File file);
        Task DeleteFileAsync(int id);
        Task DeleteFilesByClientDetailsIdAsync(int clientDetailsId);
    }
}
