using Google.Apis.Sheets.v4;
using GoogleSheetsData = Google.Apis.Sheets.v4.Data;
using Google.Apis.Drive.v3;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using apiEsferas.Domain.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Google.Apis.Sheets.v4.Data;
using apiEsferas.Domain.Entities;

namespace apiEsferas.Infrastructure.Services
{
    public class GoogleApiService : IGoogleApiService
    {
        private readonly SheetsService sheetsService;
        private readonly DriveService driveService;
        private readonly string credentialFilePath;
        private readonly string templateSpreadSheetId;
        private readonly string destinationFolderId;
        private readonly string playerDataBaseId;

        public GoogleApiService()
        {
            // Carrega variáveis de ambiente
            DotNetEnv.Env.Load();

            // Inicializa variáveis de configuração
            credentialFilePath = Environment.GetEnvironmentVariable("CLIENT_CREDENTIONS_JSON_PATH");
            templateSpreadSheetId = Environment.GetEnvironmentVariable("SHEET_TEMPLATE_ID");
            destinationFolderId = Environment.GetEnvironmentVariable("FOLDER_ID");
            playerDataBaseId = Environment.GetEnvironmentVariable("PLAYER_DATA_BASE_ID");

            // Carrega credenciais para o Sheets
            var credential = GoogleCredential.FromFile(credentialFilePath)
                .CreateScoped(new[] { SheetsService.Scope.Spreadsheets, DriveService.Scope.Drive, DriveService.Scope.DriveFile });

            // Inicializa o serviço do Google Sheets e Drive
            sheetsService = new SheetsService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "Esferas API code"
            });

            driveService = new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "Esferas API code"
            });
        }

        public async Task<string> addNewCharacterAsync(string newCharacterName, string playerId, string resgisterID)
        {
            // Cria uma nova cópia da planilha de template no Google Drive
            var requestBody = new Google.Apis.Drive.v3.Data.File
            {
                Name = newCharacterName,
                Parents = new List<string> { destinationFolderId }
            };

            var request = driveService.Files.Copy(requestBody, templateSpreadSheetId);
            var file = await request.ExecuteAsync();

            await updateCellValueAsync(file.Id, $"LOG:C133", playerId);
            await updateCellValueAsync(file.Id, $"LOG:I133", resgisterID);

            // Ajustar permissao da ficha
            var permission = new Google.Apis.Drive.v3.Data.Permission
            {
                Type = "anyone",
                Role = "writer"
            };
            await driveService.Permissions.Create(permission, file.Id).ExecuteAsync();

            return $"https://docs.google.com/spreadsheets/d/{file.Id}";
        }

        public async Task<bool> IsPlayerRegisteredAsync(string playerId)
        {
            string range = $"ListaDeJogadores!B6:B1000";

            var request = sheetsService.Spreadsheets.Values.Get(playerDataBaseId, range);
            var response = await request.ExecuteAsync();
            Console.WriteLine(request);
            Console.WriteLine(response);
            var values = response.Values;

            if(values == null || values.Count == 0)
            {
                return false;
            }

            foreach(var cell in values)
            {
                Console.WriteLine(cell);
                if(cell[0].ToString() == playerId)
                {
                    return true;
                }
            }

            return false;

        }

        public async Task<string> deletCharacterSheet(string logsLink)
        {
            // 1. Extrair o Spreadsheet ID do link.
            string spreadsheetId = ExtractIdFromUrl(logsLink, "spreadSheet");
            if (string.IsNullOrEmpty(spreadsheetId))
            {
                return "Error: Invalid Google Sheets URL.";
            }

            try
            {
                // 2. Deletar o arquivo da planilha no Google Drive.
                await driveService.Files.Delete(spreadsheetId).ExecuteAsync();
                return "Spreadsheet deleted successfully.";
            }
            catch (Exception ex)
            {
                return $"Error deleting spreadsheet: {ex.Message}";
            }
        }

        public async Task<List<Player>> listPlayersAsync()
        {
            List<Player> playersList = new List<Player>();
            List<string> LogsList;

            var range = $"ListaDeJogadores!B6:G1000";
            var request = sheetsService.Spreadsheets.Values.Get(playerDataBaseId, range);
            var response = await request.ExecuteAsync();
            var values = response.Values;
            Console.WriteLine(values);

           if(values != null)
           {
                foreach(IList<object> player in values)
                {
                    var roberto = new Player(player[0].ToString());
                    
                    LogsList = new List<string>();
                    for(int i = 0 ; i< player.Count-1; i++)
                    {
                        if(!string.IsNullOrEmpty(player[i].ToString()))
                        {
                            roberto.setPlayerCharacterLink(i, player[i+1].ToString());
                        }
                    }

                    playersList.Add(roberto);

                }
           }
            return playersList;
        }

        public async Task appendNewDataToSheet(string spreadSheetLink, List<object> newData, string range)
        {
            string spreadSheetId;

            if(spreadSheetLink.Contains('/'))
            {
                spreadSheetId = ExtractIdFromUrl(spreadSheetLink, "spreadSheet");
            }
            else
            {
                spreadSheetId = spreadSheetLink;
            }

            var valueRange = new ValueRange
            {
                Values = new List<IList<object>>{newData}
            };

            var request = sheetsService.Spreadsheets.Values.Append(valueRange, spreadSheetId, range);

            try{
                request.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.RAW;

                var response = await request.ExecuteAsync();
                

            }
            catch(Exception ex)
            {
                Console.WriteLine($"Erro ao adicionar Linha: {ex.Message}");
            }
        }

        //* returns the content of a cell in excell
        public async Task<string> getDataInACell(string linkSheet, string cellPosition)
        {
            var sheetId = ExtractIdFromUrl(linkSheet, "spreadSheet");
            var request = sheetsService.Spreadsheets.Values.Get(sheetId, cellPosition);
            var cellValue = "";

            try{
                var response = await request.ExecuteAsync();

                if (response.Values != null && response.Values.Count > 0 && response.Values[0].Count > 0)
                {
                    cellValue = response.Values[0][0].ToString();
                    Console.WriteLine(cellValue);
                }
               
            }
            catch(Exception err)
            {
                Console.WriteLine(err);
            }
             return cellValue;

        }

        public async Task changeSpreadSheetsName(string spreadsheetURL, string spreadsheetName)
        {
            var sheetId = ExtractIdFromUrl(spreadsheetURL, "spreadSheet");

            var updateRequest = new Request
            {
                UpdateSpreadsheetProperties = new UpdateSpreadsheetPropertiesRequest
                {
                    Properties = new SpreadsheetProperties
                    {
                        Title= spreadsheetName
                    },
                    Fields= "title"
                }
            };

            var batchUpdateRequest = new BatchUpdateSpreadsheetRequest
            {
                Requests = new[] {updateRequest}
            };

            try
            {
                var request = sheetsService.Spreadsheets.BatchUpdate(batchUpdateRequest, sheetId);
                var response = await request.ExecuteAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Quem quebro o codigo??? a pera fui eu e essa aqui é o erro: {ex.Message}");
            }
        }

        public async Task changeFilePosition(string fileUrl, string folderURL)
        {
            try{
                var fileId = ExtractIdFromUrl(fileUrl, "spreadSheet");
                var folderId = ExtractIdFromUrl(folderURL, "folder");

                var request = driveService.Files.Get(fileId);
                request.Fields = "parents";
                var response = request.ExecuteAsync();

                var updateRequest = driveService.Files.Update(new Google.Apis.Drive.v3.Data.File(), fileId);

                updateRequest.Fields = "id, params";

                updateRequest.AddParents = folderId;
                response = updateRequest.ExecuteAsync();
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Something go wrong here error: {ex.Message}");
            }




        }

        #region aux functions
        //* update a expesific value from a cell
        private async Task updateCellValueAsync(string sheetsId, string cellPostion, string newValue)
        {
            var valueRange = new Google.Apis.Sheets.v4.Data.ValueRange
            {
                Range = cellPostion,
                    Values = new List<IList<object>>{new List<object>{newValue}}
            };

            var updateRequest = sheetsService.Spreadsheets.Values.Update(valueRange, sheetsId,cellPostion);
            updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;

            await updateRequest.ExecuteAsync();

        }

        //* extract the id of the spreadsheets of the url
        private string ExtractIdFromUrl(string sheetUrl, string urlType)
        {
            int position=0;
            switch(urlType)
            {
                case "spreadSheet":
                {
                    position = 5;
                }
                break;
                case "folder":
                {
                    position = 7;
                }
                break;
            }
            var urlParts = sheetUrl.Split('/');

            if (urlParts.Length > 0)
            {
                return urlParts[position];
            }

            return string.Empty;
        }



        #endregion
    }
}
