using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperhumanAPI.Models;
using SuperhumanAPI.Repositories.Interfaces;

namespace SuperhumanAPI.Controllers
{
    [Route("SuperhumanAPI/[controller]")]
    [ApiController]
    [Authorize]
    public class TeamsController : ControllerBase
    {
       private readonly ITeamsRepository _teamsRepository;

        public TeamsController(ITeamsRepository teamsRepository)
        {
            _teamsRepository = teamsRepository;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<Teams>>> GetAllTeamsAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return Ok(await _teamsRepository.GetAllActiveTeamsAsync(pageNumber, pageSize));
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Teams>> GetTeamById(int id)
        {
            var team = await _teamsRepository.GetTeamByIdAsync(id);
            if (team == null)
            {
                return NotFound();
            }
            
            return Ok(team);
        }

        [HttpGet("former")]
        public async Task<ActionResult<IEnumerable<Teams>>> GetAllTeamsFormerAsync()
        {
            return Ok(await _teamsRepository.GetAllTeamsAsync());
        }

        [HttpPost]
        public async Task<ActionResult<Teams>> CreateTeam(Teams team)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest();
            }
            if (team == null)
            {
                return BadRequest("Team cannot be null");
            }
            await _teamsRepository.AddTeamAsync(team);
            return CreatedAtAction(nameof(GetTeamById), new {id = team.TeamId}, team);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTeam(int id)
        {
            var team = await _teamsRepository.GetTeamByIdAsync(id);
            if (team == null)
            {
                return NotFound();
            }
            
            await _teamsRepository.DeleteTeamByIdAsync(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTeam(int id, Teams team)
        {
            if (id != team.TeamId)
            {
                return BadRequest("Team ID mismatch");
            }
            if (ModelState.IsValid == false)
            {
                return BadRequest();
            }
            var existingTeam = await _teamsRepository.GetTeamByIdAsync(id);
            if (existingTeam == null)
            {
                return NotFound();
            }
            await _teamsRepository.UpdateTeamAsync(team);
            return NoContent();
        }
    }
}
