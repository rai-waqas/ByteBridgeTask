using AutoMapper;
using Business.Dtos;
using DataAccess.Entities;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;
        private readonly IMapper _mapper;

        public ClientController(IClientService clientService, IMapper mapper)
        {
            _clientService = clientService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllClients()
        {
            try
            {
                var clients = await _clientService.GetAllClientsAsync();
                var clientDtos = _mapper.Map<IEnumerable<ClientDto>>(clients);
                return Ok(clientDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClientById(int id)
        {
            try
            {
                var client = await _clientService.GetClientByIdAsync(id);
                if (client == null)
                    return NotFound();

                var clientDto = _mapper.Map<ClientDto>(client);
                return Ok(clientDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddClient([FromBody] ClientDto clientDto)
        {
            if (clientDto == null)
                return BadRequest("Client data is null");

            try
            {
                var client = _mapper.Map<Client>(clientDto);
                await _clientService.AddClientAsync(client);
                return CreatedAtAction(nameof(GetClientById), new { id = client.Id }, "Client Created Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClient(int id, [FromBody] ClientDto clientDto)
        {
            if (id != clientDto.Id)
                return BadRequest("Client ID mismatch");

            try
            {
                var client = _mapper.Map<Client>(clientDto);
                await _clientService.UpdateClientAsync(client);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            try
            {
                await _clientService.DeleteClientAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
