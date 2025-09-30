using SuperhumanAPI.Models;
using Microsoft.AspNetCore.Mvc;
using SuperhumanAPI.Repositories;

namespace SuperhumanAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
       private readonly ITeams _teamsRepository;

        public TeamsController(ITeams teamsRepository)
        {
            _teamsRepository = teamsRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Teams>>> GetAllTeamsAsync()
        {
            
        }
        // Additional methods for creating, updating, and deleting teams can be added here
    }
}
