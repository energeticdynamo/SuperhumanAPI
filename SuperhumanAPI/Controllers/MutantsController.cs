using SuperhumanAPI.Models;
using Microsoft.AspNetCore.Mvc;
using SuperhumanAPI.Repositories;

namespace SuperhumanAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MutantsController : ControllerBase
    {
        private readonly IMutantRepository _mutantRepository;

        public MutantsController(IMutantRepository mutantRepository)
        {
            _mutantRepository = mutantRepository;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<Mutant>>> GetAllMutantsAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return Ok(await _mutantRepository.GetAllAsync(pageNumber, pageSize));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Mutant>> GetMutantById(int id)
        {
            var mutant = await _mutantRepository.GetByIdAsync(id);

            if (mutant == null)
            {
                return NotFound();
            }
            
            return Ok(mutant);
        }

        [HttpPost]
        public async Task<ActionResult<Mutant>> CreateMutant(Mutant mutant)
        {
            if (!ModelState.IsValid || mutant == null)
            {
                return BadRequest();
            }

            // Call the standard method
            await _mutantRepository.AddAsync(mutant);
            // Use the standard 'Id' property
            return CreatedAtAction(nameof(GetMutantById), new { id = mutant.Id }, mutant);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMutant(int id)
        {
            // Use the standard 'DeleteByIdAsync'
            var success = await _mutantRepository.DeleteByIdAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateMutant(int id, Mutant mutant)
        {
            // Use the standard 'Id' property
            if (id != mutant.Id)
            {
                return BadRequest("Mutant ID mismatch");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            // Call the standard method
            await _mutantRepository.UpdateAsync(mutant);
            return NoContent();
        }
    }
}
