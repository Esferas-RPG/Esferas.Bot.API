using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Drive.v3;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using apiEsferas.Domain.Interfaces;

namespace apiEsferas.Infrastructure.Services
{
    public class GoogleSheetsService : IGoogleSheetsService
    {
        private readonly SheetsService sheetsService;
        private readonly DriveService driveService;
        private readonly string credentialFilePath;
        private readonly string templateSpreadSheetId;
        private readonly string destinationFolderId;
        private readonly string playerDataBaseId;

        public GoogleSheetsService()
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

        public async Task<string> addNewCharacterAsync(string newCharacterName, string playerId)
        {
            // Cria uma nova cópia da planilha de template no Google Drive
            var requestBody = new Google.Apis.Drive.v3.Data.File
            {
                Name = newCharacterName,
                Parents = new List<string> { destinationFolderId }
            };

            var request = driveService.Files.Copy(requestBody, templateSpreadSheetId);
            var file = await request.ExecuteAsync();

            //* because of solicitations the notes are being copied throw this function don't delete pls
            // await CopyData(file.Id, "LOG!A1:Z1000");
            //* add the player id to the new sheet //
            await updateCellValueAsync(file.Id,$"LOG:C133", playerId);

            return $"https://docs.google.com/spreadsheets/d/{file.Id}";
        }
        
        //* continue after the server opening
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
            string spreadsheetId = ExtractIdFromUrl(logsLink);
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

        public async Task<Dictionary<string,List<string>>> listPlayersAsync()
        {
            Dictionary<string, List<string>> players = new Dictionary<string, List<string>>();
            List<string> LogsList;
            
            var range = $"ListaDeJogadores!B6:G1000";
            var request = sheetsService.Spreadsheets.Values.Get(playerDataBaseId, range);
            var response = await request.ExecuteAsync();
            Console.WriteLine(request);
            Console.WriteLine(response);
            var values = response.Values;
            Console.WriteLine(values);

            foreach(IList<object> player in values)
            {
                Console.WriteLine(player);
                string key = player[0].ToString();
                LogsList = new List<string>();
                for(int i = 1 ; i< player.Count-1 ; i++)
                {
                    if(!string.IsNullOrEmpty(player[i].ToString()))
                    {
                        LogsList.Add(player[i].ToString());
                    }
                }

                players.Add(key,LogsList);

            }
            return players;
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
        private string ExtractIdFromUrl(string sheetUrl)
        {
            var urlParts = sheetUrl.Split('/');

            if (urlParts.Length > 0)
            {
                return urlParts[5];
            }

            return string.Empty;
        }

        //* copy the notes of the sheets template and past to the new sheet
        // private async Task CopyData(string newSheetId, string range)
        // {

        //     var getDataRequest = sheetsService.Spreadsheets.Values.Get(templateSpreadSheetId, "A1:Z1000");
        //     var dataResponse = await getDataRequest.ExecuteAsync();
        //     var values = dataResponse.Values;

        //     var data = new List<ValueRange>
        //     {
        //         new ValueRange
        //         {
        //             Range = range,
        //             Values = values
        //         }
        //     };

        //     var batchUpdateRequest = new BatchUpdateValuesRequest
        //     {
        //         Data = data,
        //         ValueInputOption = "RAW"
        //     };

        //     var batchUpdate = sheetsService.Spreadsheets.Values.BatchUpdate(batchUpdateRequest, newSheetId);
        //     await batchUpdate.ExecuteAsync();
        // }

        #endregion
    }
}
