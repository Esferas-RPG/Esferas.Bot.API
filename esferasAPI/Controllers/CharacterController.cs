using apiEsferas.Application.Sevices;
using apiEsferas.Application.DTO;
using apiEsferas.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Google;



namespace apiEsferas.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class CharacterController : ControllerBase
    {
        private readonly GoogleApiAppService googleApiAppService;
        private readonly string folderLink;

        public CharacterController(GoogleApiAppService googleApiAppService)
        {
            this.googleApiAppService = googleApiAppService;

            this.folderLink = Environment.GetEnvironmentVariable("LOGS_ACTIVE_PLAYERS_FOLDER_URL");
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
                var newSheetUrl = await googleApiAppService.registNewCharacter(CharacterName, playerId, registerId);
                return Ok(new{Url = newSheetUrl});
            }
            catch(Exception ex)
            {
                return StatusCode(500, new {error = ex.Message});
            }
        }

       

        

        // Aguardando implementacao
        [HttpPost("moveFile")]
        public async Task<IActionResult> moveFiles([FromBody] moveFilesRequest request)
        {
            string fileLink = request.fileLink;
            string destinationFolderId = request.destinationLink;

            if(string.IsNullOrEmpty(fileLink))
            {
                return BadRequest("The file link is required because if i dont have the link how i should know what move");
            }

            if(string.IsNullOrEmpty(destinationFolderId))
            {
                return BadRequest("The destination folder link is required because, where the f* i gonna place the file");
            }

            try{
                await googleApiAppService.changeFilePosition(fileLink,destinationFolderId);

                return Ok(new{message = "File in the new place "});
            }
            catch(Exception ext)
            {
                return StatusCode(500, $"something goes wrong: {ext.Message}");
            }



        }

        [HttpPost("deleteCharacter")]
        public async Task<IActionResult> deleteCharacter([FromBody] CharacterLogsRequest request)
        {
            var logsLink = request.logsLink;

            if(string.IsNullOrEmpty(logsLink))
            {
                return BadRequest("The logs link is required");
            }

            string result = await googleApiAppService.deleteCharacterSheets(logsLink);

            if(result != "Spreadsheet deleted successfully.")
            {
                return StatusCode(500, result);
            }

            return Ok(new{message = result});
        }

        [HttpPost("validateCharacter")]
        public async Task<IActionResult> validadeCharacter([FromBody] CharacterLogsRequest request)
        {
            var logsLink = request.logsLink;
            var result = "Estão faltando dados nos seguintes campos:\n";
            bool isAllRight = true;
            var roberto = new Character();

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
                    string text = await googleApiAppService.getDataInACell(logsLink, checkList[i]);
                    if(text == "" || text =="-" || text == "Selecione seu Antecedente")
                    {
                        result += $"\t{checkListTags[i]}: {text}\n";
                        isAllRight = false;
                    }

                    switch (checkList[i])
                    {
                        case "LOG!C6":
                            {
                                roberto.CharacterName = text;
                            }
                            break;
                        case "LOG!T5":
                            {
                                roberto.CharacterClass = text;
                            }
                            break;
                        case "LOG!T7":
                            {
                                roberto.CharacterRace = text;
                            }
                            break;
                        case "LOG!AI11":
                            {
                                roberto.CharacterBackground = text;
                            }
                            break;
                        case "Personagem!V25":
                            {
                                roberto.CharacterGuild = text;
                            }break;
                        case "Personagem!S21":
                            {
                                roberto.CharacterImageLink = text;
                            }
                            break;
                    }
                }

                if(isAllRight)
                {
                    result =
                        "Novo aventureiro registrado!"+
                        $"> - Nome de personagem: {roberto.CharacterName}"+
                        $"> - Raça:{roberto.CharacterRace} \n"+
                        $"> - Classe:{roberto.CharacterClass}\n"+
                        $"> - Antecedente: {roberto.CharacterBackground}\n"+
                        $"> - Guilda:{roberto.CharacterGuild}\n"+
                        $"> - Imagem:\n\t {roberto.CharacterImageLink}"+
                        "\n\n se todos os campos estiverem preenchido o resultado é aprovado luan, agora tu se vira pra resolver, tirara esse texto, eu poderia mandar um json com os dados organizados, mas isso seria muito facil";
                    await googleApiAppService.changeSpreadSheetsName(logsLink, roberto.CharacterName);
                    await googleApiAppService.changeFilePosition(logsLink,folderLink);

                }

                return Ok(new{
                    Conclusion = result,
                    PlayerData = roberto
                    });
            }
            catch(Exception err)
            {
                return StatusCode(500, new{erro= " algo deu errado, o algo:" +err});
            }
        }
    }
}
