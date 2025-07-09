using SuperhumanAPI.Models;
using Microsoft.AspNetCore.Mvc;
using SuperhumanAPI.Repositories;

namespace SuperhumanAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperhumansController : ControllerBase
    {
        private readonly ISuperhumanRepository _superhumanRepository;

        public SuperhumansController(ISuperhumanRepository superhumanRepository)
        {
            _superhumanRepository = superhumanRepository;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<SuperhumanDTO>>> GetAllSuperhumansAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return Ok(await _superhumanRepository.GetAllSuperhumansAsync(pageNumber, pageSize));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SuperhumanDTO>> GetSuperhumanById(int id)
        {
            var superhuman = await _superhumanRepository.GetSuperhumanByIdAsync(id);

            if (superhuman == null)
            {
                return NotFound();
            }

            return Ok(superhuman);
        }

        [HttpPost]
        public async Task<ActionResult<SuperhumanDTO>> CreateSuperhuman(SuperhumanDTO superhuman)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest();
            }

            if (superhuman == null)
            {
                return BadRequest("Superhuman cannot be null");
            }

            await _superhumanRepository.AddSuperhumanAsync(superhuman);
            return CreatedAtAction(nameof(GetSuperhumanById), new {id = superhuman.SuperhumanId}, superhuman);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSuperhuman(int id)
        {
            var superhuman = await _superhumanRepository.GetSuperhumanByIdAsync(id);
            if (superhuman == null)
            {
                return NotFound();
            }
            await _superhumanRepository.DeleteSuperhumanByIdAsync(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateSuperhuman(int id, SuperhumanDTO superhuman)
        {
            if (id != superhuman.SuperhumanId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid == false)
            {
                return BadRequest();
            }

            if (superhuman == null)
            {
                return BadRequest("Superhuman cannot be null");
            }

            var existingSuperhuman = await _superhumanRepository.GetSuperhumanByIdAsync(id);
            if (existingSuperhuman == null)
            {
                return NotFound();
            }
            await _superhumanRepository.UpdateSuperhumanAsync(superhuman);
            return CreatedAtAction(nameof(GetSuperhumanById), new { id = superhuman.SuperhumanId }, superhuman);
        }
    }
}
