using AutoMapper;
using Business.Services;
using DataAccess.Dtos;
using DataAccess.Entities;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientDetailsController : ControllerBase
    {
        private readonly IClientDetailsService _clientDetailsService;
        private readonly IMapper _mapper;
        private readonly ILogger<ClientController> _logger;

        public ClientDetailsController(IClientDetailsService clientDetailsService, IMapper mapper, ILogger<ClientController> logger)
        {
            _clientDetailsService = clientDetailsService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllClientDetails(
            [FromQuery] string? searchTerm = null,
            [FromQuery] int pageSize = 10,
            [FromQuery] int pageNumber = 1,
            [FromQuery] string? sortColumn = null,
            [FromQuery] string? sortDirection = "asc")
        {
            try
            {
                var (clientDetails, totalCount) = await _clientDetailsService.GetAllClientDetailsAsync(searchTerm, pageSize, pageNumber, sortColumn, sortDirection);
                var clientDetailsDtos = _mapper.Map<IEnumerable<ClientDetailsDto>>(clientDetails);
                var response = new
                {
                    Data = clientDetailsDtos,
                    TotalCount = totalCount
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClientDetailsById(int id)
        {
            try
            {
                var clientDetails = await _clientDetailsService.GetClientDetailsByIdAsync(id);
                if (clientDetails == null)
                    return NotFound();

                var clientDetailsDto = _mapper.Map<ClientDetailsDto>(clientDetails);
                return Ok(clientDetailsDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetClientDetailsByEmail(string email)
        {
            try
            {
                var clientDetails = await _clientDetailsService.GetClientDetailsByEmailAsync(email);
                if (clientDetails == null)
                    return NotFound();

                var clientDetailsDto = _mapper.Map<ClientDetailsDto>(clientDetails);
                return Ok(clientDetailsDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddClientDetails([FromForm] AddClientDetailsDto clientDetailsDto)
        {
            if (clientDetailsDto == null)
                return BadRequest("Client details data is null");

            if (string.IsNullOrEmpty(clientDetailsDto.Email))
            {
                return BadRequest("Email cannot be null or empty");
            }

            if (_clientDetailsService.EmailExists(clientDetailsDto.Email).Result)
            {
                return Conflict("Email already exists");
            }
            try
            {
                await _clientDetailsService.AddClientDetailsAsync(clientDetailsDto);

                return Ok(new { Message = "Client details added successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpGet("emailExists/{email}")]
        public async Task<IActionResult> EmailExists(string email)
        {
            try
            {
                var emailExists = await _clientDetailsService.EmailExists(email);
                return Ok(emailExists);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClientDetails(int id, [FromForm] AddClientDetailsDto clientDetailsDto)
        {
            if (id != clientDetailsDto.Id)
                return BadRequest("Client details ID mismatch");

            try
            {
                await _clientDetailsService.UpdateClientDetailsAsync(clientDetailsDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClientDetails(int id)
        {
            try
            {
                await _clientDetailsService.DeleteClientDetailsAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
