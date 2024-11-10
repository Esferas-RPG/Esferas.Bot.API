using apiEsferas.Application.Sevices;
using apiEsferas.Application.DTO;
using apiEsferas.Domain.entities;
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
            var registerId = request.registerId;

            Console.WriteLine(CharacterName);
            if(string.IsNullOrEmpty(CharacterName))
            {
                return BadRequest("The name of the new Character is requered");
            }
            if(string.IsNullOrEmpty(playerId))
            {
                return BadRequest("Luan, cade a porra do id");
            }
            if(string.IsNullOrEmpty(registerId))
            {
                return BadRequest("O luan esqueceu de manda o id do registrador, por favor reclamar com aquele que te comeu atrás da van (luanmendes)");
            }
            //* Adicionar verificação se o jogador já existe

            try
            {
                var newSheetUrl = await spreadSheetAppService.registNewCharacter(CharacterName, playerId, registerId);
                return Ok(new{Url = newSheetUrl});
            }
            catch(Exception ex)
            {
                return StatusCode(500, new {error = ex.Message});
            }
        }

        [HttpGet("listPlayers")]
        public async Task<IActionResult> ListPlayers()
        {
            Dictionary<string,List<string>> players = new Dictionary<string,List<string>>();
            players = await spreadSheetAppService.listPlayers();
            return Ok(new{
                Players = players
            });
        }

        

        // Aguardando implementacao
        // [HttpPost("moveFile")]
        // public async Task<IActionResult> moveFiles([FromBody] moveFilesRequest request)
        // {
        //     var fileLink = request.fileLink;
        //     var destinationFolderId = request.destinationLink;
        // }

        [HttpPost("deleteCharacter")]
        public async Task<IActionResult> deleteCharacter([FromBody] CharacterLogsRequest request)
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

        [HttpGet("validateCharacter")]
        public async Task<IActionResult> validadeCharacter([FromBody] CharacterLogsRequest request)
        {
            var logsLink = request.logsLink;
            var result = "Estão faltando dados nos seguintes campos:\n";
            bool isAllRight = true;
            Character roberto = new Character();

            if(string.IsNullOrEmpty(logsLink))
            {
                return BadRequest("Atenção o link é necessario, se não como caralhos eu vou saber o que verificar");
            }

            string[] checkList =
            {
                "LOG!C6",
                "LOG!T5",
                "LOG!T7",
                "LOG!AE5",
                "LOG!AI11",
                "LOG!C13",
                "LOG!C18",
                "LOG!C23",
                "LOG!C28",
                "LOG!C33",
                "LOG!C38",
                "LOG!AI28",
                "LOG!R17",
                "Personagem!V25",
                "Personagem!S21"
            };
            string[] checkListTags=
            {
                "Nome",
                "Classe",
                "Raça",
                "Nome de jogador",
                "Antecedente",
                "Força",
                "Destreza",
                "Constituição",
                "Inteligência",
                "Sabedoria",
                "Carisma",
                "Antecedente",
                "Vida",
                "Guilda",
                "Link de imagem de personagem"
            };
            try
            {
                for(int i = 0; i < checkList.Length; i++)
                {
                    string text = await spreadSheetAppService.verifyTheDataInACell(logsLink, checkList[i]);
                    if(text == "" || text =="-" || text == "Selecione seu Antecedente")
                    {
                        result += $"\t{checkListTags[i]}: {text}\n";
                    }
                    
                    switch (checkList[i])
                    {
                        case "LOG!C6":
                            {
                                roberto.setCharacterName(text);
                            }break;
                        case "LOG!T5":
                            {

                            }break;
                        case "LOG!T7":
                            {

                            }break;
                        case "LOG!AI11":
                            {

                            }break;
                        case "Personagem!V25":
                            {

                            }break;
                        case "Personagem!S21":
                            {

                            }break;
                    }
                }

                if(isAllRight)
                {
                    result =
                        "Novo aventureiro registrado!"+
                        $"> - Nome de personagem: {roberto.getCharacterName()}"+
                        $"> - Raça:{roberto.getCharcterRace()} \n"+
                        $"> - Classe:{roberto.getCharacterClass()}\n"+
                        $"> - Antecedente: {roberto.getcharacterBackground()}\n"+
                        $"> - Guilda:{roberto.getCharacterGuild()}\n"+
                        $"> - Imagem:\n\t {roberto.getCharacterImageLink()}"+
                        "\n\n se todos os campos estiverem preenchido o resultado é aprovado luan, agora tu se vira pra resolver, tirara esse textoeu poderia mandar um json com os dados organizados, mas isso seria muito facil";

                }

                return Ok(new{Conclusion = result});
            }
            catch(Exception err)
            {
                return StatusCode(500, new{erro= " algo deu errado, o algo:" +err});
            }
        }
    }
}
