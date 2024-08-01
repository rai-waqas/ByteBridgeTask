using AutoMapper;
using Business.Dtos;
using DataAccess.Entities;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StateController : ControllerBase
    {
        private readonly IStateService _stateService;
        private readonly IMapper _mapper;

        public StateController(IStateService stateService, IMapper mapper)
        {
            _stateService = stateService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStates()
        {
            try
            {
                var states = await _stateService.GetAllStatesAsync();
                var stateDtos = _mapper.Map<IEnumerable<StateDto>>(states);
                return Ok(stateDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStateById(int id)
        {
            try
            {
                var state = await _stateService.GetStateByIdAsync(id);
                if (state == null)
                    return NotFound();

                var stateDto = _mapper.Map<StateDto>(state);
                return Ok(stateDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddState([FromBody] StateDto stateDto)
        {
            if (stateDto == null)
                return BadRequest("State data is null");

            try
            {
                var state = _mapper.Map<State>(stateDto);
                await _stateService.AddStateAsync(state);
                return CreatedAtAction(nameof(GetStateById), new { id = state.Id }, "Client Created Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateState(int id, [FromBody] StateDto stateDto)
        {
            if (id != stateDto.Id)
                return BadRequest("State ID mismatch");

            try
            {
                var state = _mapper.Map<State>(stateDto);
                await _stateService.UpdateStateAsync(state);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteState(int id)
        {
            try
            {
                await _stateService.DeleteStateAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}

