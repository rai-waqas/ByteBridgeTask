using File = DataAccess.Entities.File;
using DataAccess.Interfaces;

namespace Business.Services
{
    public class FilesService : IFilesService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FilesService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<File?> GetFileByIdAsync(int id)
        {
            return await _unitOfWork.Files.GetByIdAsync(id);
        }

        public async Task<IEnumerable<File>> GetFilesByClientDetailsIdAsync(int clientId)
        {
            return await _unitOfWork.Files.GetFilesByClientDetailsIdAsync(clientId);
        }

        public async Task<IEnumerable<File>> AddFilesAsync(IEnumerable<File> files)
        {
            await _unitOfWork.Files.AddAllAsync(files);
            return files;
        }

        public async Task AddFileAsync(File file)
        {
            await _unitOfWork.Files.AddAsync(file);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateFileAsync(File file)
        {
            await _unitOfWork.Files.UpdateAsync(file);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteFileAsync(int id)
        {
            await _unitOfWork.Files.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteFilesByClientDetailsIdAsync(int clientDetailsId)
        {
            await _unitOfWork.Files.DeleteAllFilesByClientDetailsIdAsync(clientDetailsId);
        }

        public async Task<Boolean> FileExists(string name)
        {
            var file = await _unitOfWork.Files.GetFileByName(name);
            if (file == null) {
                return false;
            }
            return true;
        }
    }
}
