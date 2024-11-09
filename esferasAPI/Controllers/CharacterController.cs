using apiEsferas.Application.Sevices;
using apiEsferas.Application.DTO;
using Microsoft.AspNetCore.Mvc;

namespace apiEsferas.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class CharacterController : ControllerBase
    {
        private readonly SpreadSheetAppService spreadSheetAppService;

        public CharacterController(SpreadSheetAppService spreadSheetAppService)
        {
            this.spreadSheetAppService = spreadSheetAppService;
        }


        //* Route post to regist a new CharacterLog
        [HttpPost("newCharacter")]
        public async Task<IActionResult> addNewCharacter([FromBody] CharacterRequest request)
        {
            var CharacterName = request.newCharacterName;
            var playerId = request.playerId;
            
            Console.WriteLine(CharacterName);
            if(string.IsNullOrEmpty(CharacterName))
            {
                return BadRequest("The name of the new Character is requered");
            }
            if(string.IsNullOrEmpty(playerId))
            {
                return BadRequest("Luan, cade a porra do id");
            }

            //* Adicionar verificação se o jogador já existe

            try
            {
                var newSheetUrl = await spreadSheetAppService.registNewCharacter(CharacterName, playerId);
                return Ok(new{Url = newSheetUrl});
            }
            catch(Exception ex)
            {
                return StatusCode(500, new {error = ex.Message});
            }
        }

        //* listar players
        [HttpGet("listPlayers")]
        public async Task<IActionResult> ListPlayers()
        {
            Dictionary<string,List<string>> players = new Dictionary<string,List<string>>();
            players = await spreadSheetAppService.listPlayers();
            return Ok(new{
                Players = players
            });
        }


        //* Não mexer nesta caralha - to fazendo ainda luan
        // [HttpPost("moveFile")]
        // public async Task<IActionResult> moveFiles([FromBody] moveFilesRequest request)
        // {
        //     var fileLink = request.fileLink;
        //     var destinationFolderId = request.destinationLink;
        // }


        //* delete the log
        [HttpPost("deleteSheets")]
        public async Task<IActionResult> deleteCharacter([FromBody] CharacterDeleteRequest request)
        {
            var logsLink = request.logsLink;

            if(string.IsNullOrEmpty(logsLink))
            {
                return BadRequest("The logs link is required");
            }

            string result = await spreadSheetAppService.deleteCharacterSheets(logsLink);

            if(result != "Spreadsheet deleted successfully.")
            {
                return StatusCode(500, result);
            }

            return Ok(new{message = result});
        }
    }
