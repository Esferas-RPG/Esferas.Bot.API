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
        public async Task<IActionResult> addNewCharacter([FromBody] CopyRequest request)
        {
            var CharacterName = request.newCharacterName;
            Console.WriteLine(CharacterName);
            if(string.IsNullOrEmpty(CharacterName))
            {
                return BadRequest("The name of the new Character is requered");
            }
            //* Adicionar verificação se o jogador já existe
            try
            {
                var newSheetUrl = await spreadSheetAppService.registNewCharacter(CharacterName);
                return Ok(new{Url = newSheetUrl});
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Error:{ex.Message}");
            }
        }
    }

    
}