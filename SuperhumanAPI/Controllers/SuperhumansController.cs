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
        public async Task<ActionResult<PagedResult<Superhuman>>> GetAllSuperhumansAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return Ok(await _superhumanRepository.GetAllAsync(pageNumber, pageSize));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Superhuman>> GetSuperhumanById(int id)
        {
            var superhuman = await _superhumanRepository.GetByIdAsync(id);

            if (superhuman == null)
            {
                return NotFound();
            }

            return Ok(superhuman);
        }

        [HttpPost]
        public async Task<ActionResult<Superhuman>> CreateSuperhuman(Superhuman superhuman)
        {
            if (!ModelState.IsValid == false || superhuman == null)
            {
                return BadRequest();
            }

            

            await _superhumanRepository.AddAsync(superhuman);
            return CreatedAtAction(nameof(GetSuperhumanById), new {id = superhuman.Id}, superhuman);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSuperhuman(int id)
        {
            var success = await _superhumanRepository.DeleteByIdAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateSuperhuman(int id, Superhuman superhuman)
        {
            if (id != superhuman.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            await _superhumanRepository.UpdateAsync(superhuman);
            return CreatedAtAction(nameof(GetSuperhumanById), new { id = superhuman.Id }, superhuman);
        }
    }
}
