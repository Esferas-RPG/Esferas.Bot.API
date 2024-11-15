using System.Runtime.CompilerServices;
using apiEsferas.Application.DTO;
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
        private readonly string playerDataBaseId;

        public PlayerController(GoogleApiAppService googleApiAppService)
        {
            this.googleApiAppService = googleApiAppService;
            playerDataBaseId = Environment.GetEnvironmentVariable("PLAYER_DATA_BASE_ID");
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

        [HttpPost("addNewPlayer")]
        public async Task<IActionResult> addNewPlawer([FromBody]NewPlayerRequest request)
        {
            string playerId = request.playerId;
            string newCharacterLink = request.newCharacterLink;

            if(string.IsNullOrEmpty(playerId))
            {
                return BadRequest("Falta o link");
            }

            if(string.IsNullOrEmpty(newCharacterLink))
            {
                return BadRequest("falta o link do personagem do novo jogador");

            }

            var newData = new List<object> {playerId, newCharacterLink};
            var range = "ListaDeJogadores";

            try
            {
                await googleApiAppService.appendNewDataToSheet(playerDataBaseId,newData,range);
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Something goes wrong: {ex.Message}");
            }

            return Ok(new{
                message = "registrado"
            });
        }
    }
}
