
using DataAccess.Dtos;
using DataAccess.Entities;
using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using File = DataAccess.Entities.File;
using Microsoft.AspNetCore.Http;

namespace Business.Services
{
    public class ClientDetailsService : IClientDetailsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFilesService _filesService;

        public ClientDetailsService(IUnitOfWork unitOfWork, IMapper mapper, IFilesService filesService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            this._filesService = filesService;
        }
        
        public async Task<ClientDetailsDto?> GetClientDetailsByIdAsync(int id)
        {
            var clientDetail = await _unitOfWork.ClientDetails.GetClientDetails()
                .Include(cd => cd.Client) 
                .Include(cd => cd.Files)  
                .FirstOrDefaultAsync(cd => cd.Id == id);

            if (clientDetail == null)
            {
                return null;
            }
            
            var stateIds = clientDetail.StateId.Split(',').Select(int.Parse).ToArray();

            var filesDto = clientDetail.Files.Select(file => new FileDto
            {
                Id = file.Id,
                Filename = file.Filename,
                Filedata = file.Filedata
            }).ToList();

            var responseDto = new ClientDetailsDto
            {
                Id = clientDetail.Id,
                Name = clientDetail.Name,
                Email = clientDetail.Email,
                ClientId = clientDetail.ClientId,
                ClientName = clientDetail.Client.Name,
                StateId = stateIds,
                Dob = clientDetail.Dob,
                ExpStart = clientDetail.ExpStart,
                ExpEnd = clientDetail.ExpEnd,
                PayValue = clientDetail.PayValue,
                PayType = clientDetail.PayType,
                Gender = clientDetail.Gender,
                Files = filesDto
            };

            return responseDto;
        }

        public async Task<(IEnumerable<ClientDetailsDto> data, int totalCount)> GetAllClientDetailsAsync(
            string? searchTerm, 
            int pageSize, 
            int pageNumber, 
            string? sortColumn = null, 
            string? sortDirection = "asc")
        {
            IQueryable<Clientdetail> query = _unitOfWork.ClientDetails.GetClientDetails()
                .Include(cd => cd.Client)
                .Include(cd => cd.Files);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(cd => cd.Name.ToLower().Contains(searchTerm.ToLower()) || 
                                            cd.Email.ToLower().Contains(searchTerm.ToLower()) || 
                                            cd.Client.Name.ToLower().Contains(searchTerm.ToLower()));
            }
            // Apply sorting
            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortDirection))
            {
                bool isAscending = sortDirection.ToLower() == "asc";

                switch (sortColumn.ToLower())
                {
                    case "name":
                        query = isAscending ? query.OrderBy(cd => cd.Name.ToLower()) : query.OrderByDescending(cd => cd.Name.ToLower());
                        break;
                    case "email":
                        query = isAscending ? query.OrderBy(cd => cd.Email) : query.OrderByDescending(cd => cd.Email);
                        break;
                    case "client":
                        query = isAscending ? query.OrderBy(cd => cd.Client.Name.ToLower()) : query.OrderByDescending(cd => cd.Client.Name.ToLower());
                        break;          
                    case "gender":
                        query = isAscending ? query.OrderBy(cd => cd.Gender) : query.OrderByDescending(cd => cd.Gender);
                        break;
                    case "rate":
                        query = isAscending ? query.OrderBy(cd => cd.PayValue) : query.OrderByDescending(cd => cd.PayValue);
                        break;
                    default:
                        // Default sorting if the column is not recognized
                        query = query.OrderBy(cd => cd.Id);
                        break;
                }
            }
            var totalCount = await query.CountAsync();

            var clientDetails = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var data = clientDetails.Select(cd => new ClientDetailsDto
            {
                Id = cd.Id,
                Name = cd.Name,
                Email = cd.Email,
                ClientId = cd.ClientId,
                ClientName = cd.Client.Name,
                StateId = cd.StateId.Split(',').Select(int.Parse).ToArray(),
                Dob = cd.Dob,
                ExpStart = cd.ExpStart,
                ExpEnd = cd.ExpEnd,
                PayValue = cd.PayValue,
                PayType = cd.PayType,
                Gender = cd.Gender,
                Files = cd.Files.Select(file => new FileDto
                {
                    Id = file.Id,
                    Filename = file.Filename,
                    Filedata = file.Filedata
                }).ToList()
            }).ToList();

            return (data, totalCount);
        }

        public async Task AddClientDetailsAsync(AddClientDetailsDto clientDetailsDto)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var clientDetails = _mapper.Map<Clientdetail>(clientDetailsDto);
                await _unitOfWork.ClientDetails.AddAsync(clientDetails);
                await _unitOfWork.CompleteAsync();

                int clientDetailsId = clientDetails.Id;
                if (clientDetailsDto.Files.Any())
                {
                    var fileEntities = clientDetailsDto.Files.Select(file => new File
                    {
                        Filename = file.FileName,
                        Filedata = GetFileData(file),
                        ClientDetailsId = clientDetailsId
                    }).ToList();

                    await _filesService.AddFilesAsync(fileEntities);
                }

                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

        }

        #region Helper Methods

        private static byte[] GetFileData(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        #endregion

        public async Task UpdateClientDetailsAsync(AddClientDetailsDto clientDetailsDto)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var clientDetails = _mapper.Map<Clientdetail>(clientDetailsDto);
                await _unitOfWork.ClientDetails.UpdateAsync(clientDetails);
                await _unitOfWork.CompleteAsync();

                int clientDetailsId = clientDetails.Id;
                if (clientDetailsDto.Files.Any())
                {
                    var fileEntities = clientDetailsDto.Files.Select(file => new File
                    {
                        Filename = file.FileName,
                        Filedata = GetFileData(file),
                        ClientDetailsId = clientDetailsId
                    }).ToList();
                    await _filesService.DeleteFilesByClientDetailsIdAsync(clientDetailsId);
                    await _filesService.AddFilesAsync(fileEntities);
                }

                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task DeleteClientDetailsAsync(int id)
        {
            await _unitOfWork.ClientDetails.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<Clientdetail?> GetClientDetailsByEmailAsync(string email)
        {
            return await _unitOfWork.ClientDetails.GetClientDetailsByEmailAsync(email);
        }
    }
}
