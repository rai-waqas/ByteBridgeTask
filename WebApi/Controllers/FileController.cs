using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using DataAccess.Interfaces;
using DataAccess.Dtos;
using File = DataAccess.Entities.File;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private readonly IFilesService _filesService;
        private readonly IMapper _mapper;

        public FileController(IFilesService filesService, IMapper mapper)
        {
            _filesService = filesService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFileById(int id)
        {
            try
            {
                var file = await _filesService.GetFileByIdAsync(id);
                if (file == null)
                {
                    return NotFound(new { message = "File not found" });
                }

                var fileDto = _mapper.Map<FileDto>(file);
                return Ok(fileDto);
            }
            catch (Exception ex)
            {
                // Log the exception details here
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while retrieving the file", details = ex.Message });
            }
        }

        [HttpGet("clientDetails/{clientId}")]
        public async Task<IActionResult> GetFilesByClientDetailsId(int clientId)
        {
            try
            {
                var files = await _filesService.GetFilesByClientDetailsIdAsync(clientId);
                var fileDtos = _mapper.Map<IEnumerable<FileDto>>(files);
                return Ok(fileDtos);
            }
            catch (Exception ex)
            {
                // Log the exception details here
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while retrieving files", details = ex.Message });
            }
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFiles([FromForm] IList<IFormFile> files, [FromForm] int clientDetailsId)
        {
            if (files == null || files.Count == 0)
            {
                return BadRequest(new { message = "No files were uploaded" });
            }

            try
            {
                var fileEntities = new List<File>();

                foreach (var file in files)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await file.CopyToAsync(memoryStream);
                        var fileEntity = new File
                        {
                            Filename = file.FileName,
                            Filedata = memoryStream.ToArray(),
                            ClientDetailsId = clientDetailsId
                        };
                        fileEntities.Add(fileEntity);
                    }
                }

                await _filesService.AddFilesAsync(fileEntities);
                return Ok(new { message = "Files uploaded successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while uploading files", details = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFile(int id)
        {
            try
            {
                var file = await _filesService.GetFileByIdAsync(id);
                if (file == null)
                {
                    return NotFound(new { message = "File not found" });
                }

                await _filesService.DeleteFileAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while deleting the file", details = ex.Message });
            }
        }

        [HttpDelete("clientDetails/{clientDetailsId}")]
        public async Task<IActionResult> DeleteFilesByClientDetailsId(int clientDetailsId)
        {
            try
            {
                await _filesService.DeleteFilesByClientDetailsIdAsync(clientDetailsId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while deleting files", details = ex.Message });
            }
        }
    }
}
