using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperhumanAPI.Models;
using SuperhumanAPI.Repositories.Interfaces;

namespace SuperhumanAPI.Controllers
{
    [Route("SuperhumanAPI/[controller]")]
    [ApiController]
    [Authorize]
    public class CosmicBeingController : ControllerBase
    {
        private readonly ICosmicBeingRepository _cosmicBeingRepository;
        public CosmicBeingController(ICosmicBeingRepository cosmicBeingRepository)
        {
            _cosmicBeingRepository = cosmicBeingRepository;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<CosmicBeing>>> GetAllCosmicBeingsAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return Ok(await _cosmicBeingRepository.GetAllCosmicBeingsAsync(pageNumber, pageSize));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CosmicBeing>> GetCosmicBeingById(int id)
        {
            var cosmicBeing = await _cosmicBeingRepository.GetCosmicBeingByIdAsync(id);
            if (cosmicBeing == null)
            {
                return NotFound();
            }
            return Ok(cosmicBeing);
        }

        [HttpPost]
        public async Task<ActionResult<CosmicBeing>> CreateCosmicBeing(CosmicBeing cosmicBeing)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest();
            }
            if (cosmicBeing == null)
            {
                return BadRequest("Cosmic Being cannot be null");
            }
            await _cosmicBeingRepository.AddCosmicBeingAsync(cosmicBeing);
            return CreatedAtAction(nameof(GetCosmicBeingById), new { id = cosmicBeing.CosmicId }, cosmicBeing);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCosmicBeing(int id)
        {
            var cosmicBeing = await _cosmicBeingRepository.GetCosmicBeingByIdAsync(id);
            if (cosmicBeing == null)
            {
                return NotFound();
            }

            await _cosmicBeingRepository.DeleteCosmicBeingByCosmicIdAsync(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCosmicBeing(int id, CosmicBeing cosmicBeing)
        {
            if (id != cosmicBeing.CosmicId)
            {
                return BadRequest("Cosmic ID mismatch");
            }
            var existingCosmicBeing = await _cosmicBeingRepository.GetCosmicBeingByIdAsync(id);
            if (existingCosmicBeing == null)
            {
                return NotFound();
            }
            await _cosmicBeingRepository.UpdateCosmicBeingAsync(cosmicBeing);
            return NoContent();
        }
    }
}
