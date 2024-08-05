using DataAccess.DataContext;
using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using File = DataAccess.Entities.File;

namespace DataAccess.Repositories
{
    public class FilesRepository : GenericRepository<File>, IFilesRepository
    {
        public FilesRepository(DBContext context) : base(context)
        {
        }

        public async Task<IEnumerable<File>> GetFilesByClientDetailsIdAsync(int clientDetailsId)
        {
            return await _context.Files
                .Where(f => f.ClientDetailsId == clientDetailsId)
                .ToListAsync();
        }

        public async Task<IEnumerable<File>> AddAllAsync(IEnumerable<File> files)
        {
            _context.Set<File>().AddRange(files);
            return await Task.FromResult(files);
        }
        public async Task DeleteAllFilesByClientDetailsIdAsync(int clientDetailsId)
        {
            var filesToDelete = await _context.Set<File>()
                .Where(file => file.ClientDetailsId == clientDetailsId)
                .ToListAsync();

            if (filesToDelete.Any())
            {
                _context.Set<File>().RemoveRange(filesToDelete);
            }
        }

        public async Task<File?> GetFileByName(string name)
        {
            return await _context.Files.FirstOrDefaultAsync(f => f.Filename == name);
        }
    }
}
