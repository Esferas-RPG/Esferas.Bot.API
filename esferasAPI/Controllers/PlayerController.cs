using System.Runtime.CompilerServices;
using apiEsferas.Application.Sevices;
using apiEsferas.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace apiEsferas.Controllers
{
    [ApiController]
    [Route("/[controller]")]

    public class PlayerController : ControllerBase
    {

        private readonly GoogleApiAppService googleApiAppService;

        public PlayerController(GoogleApiAppService googleApiAppService)
        {
            this.googleApiAppService = googleApiAppService;
        }

        [HttpGet("listPlayers")]
        public async Task<IActionResult> ListPlayers()
        {
            List<Player> playersList = new List<Player>();
            playersList = await googleApiAppService.listPlayers();
            return Ok(new{
                Players = playersList
            });
        }

        // [HttpPost("add")]
        // public async Task<IActionResult> registNewPlayer([])
    }
}
