using SuperhumanAPI.Models;
using Microsoft.AspNetCore.Mvc;
using SuperhumanAPI.Repositories.Interfaces;

namespace SuperhumanAPI.Controllers
{
    [Route("SuperhumanAPI/[controller]")]
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
            return Ok(await _mutantRepository.GetAllMutantsAsync(pageNumber, pageSize));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Mutant>> GetMutantById(int id)
        {
            var mutant = await _mutantRepository.GetMutantByIdAsync(id);

            if (mutant == null)
            {
                return NotFound();
            }
            
            return Ok(mutant);
        }

        [HttpPost]
        public async Task<ActionResult<Mutant>> CreateMutant(Mutant mutant)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest();
            }

            if (mutant == null)
            {
                return BadRequest("Mutant cannot be null");
            }

            await _mutantRepository.AddMutantAsync(mutant);
            return CreatedAtAction(nameof(GetMutantById), new {id = mutant.MutantId}, mutant);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMutant(int id)
        {
            var mutant = await _mutantRepository.GetMutantByIdAsync(id);
            if (mutant == null)
            {
                return NotFound();
            }
            
            await _mutantRepository.DeleteMutantByMutantIdAsync(id);            
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateMutant(int id, Mutant mutant)
        {
            if (id != mutant.MutantId)
            {
                return BadRequest("Mutant ID mismatch");
            }
            if (ModelState.IsValid == false)
            {
                return BadRequest();
            }
            var existingMutant = await _mutantRepository.GetMutantByIdAsync(id);
            if (existingMutant == null)
            {
                return NotFound();
            }
            await _mutantRepository.UpdateMutantAsync(mutant);
            return NoContent();
        }
    }
}
