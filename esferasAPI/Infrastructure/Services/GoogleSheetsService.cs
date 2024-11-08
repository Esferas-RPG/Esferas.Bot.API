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

        public GoogleSheetsService()
        {
            // Carrega variáveis de ambiente
            DotNetEnv.Env.Load();

            // Inicializa variáveis de configuração
            credentialFilePath = Environment.GetEnvironmentVariable("CLIENT_CREDENTIONS_JSON_PATH");
            templateSpreadSheetId = Environment.GetEnvironmentVariable("SHEET_TEMPLATE_ID");
            destinationFolderId = Environment.GetEnvironmentVariable("FOLDER_ID");

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

        // Método para criar nova planilha copiando um template e copiar valores
        public async Task<string> addNewCharacterAsync(string newCharacterName)
        {
            // Cria uma nova cópia da planilha de template no Google Drive
            var requestBody = new Google.Apis.Drive.v3.Data.File
            {
                Name = newCharacterName,
                Parents = new List<string> { destinationFolderId }
            };

            var request = driveService.Files.Copy(requestBody, templateSpreadSheetId);
            var file = await request.ExecuteAsync();

            // Copia os dados da planilha template
            await CopyData(file.Id);

            return $"https://docs.google.com/spreadsheets/d/{file.Id}";
        }

        // Método para copiar os dados da planilha template para a nova planilha
        private async Task CopyData(string newSheetId)
        {
            // Pega os dados da planilha template (valores)
            var getDataRequest = sheetsService.Spreadsheets.Values.Get(templateSpreadSheetId, "A1:Z1000"); // Ajuste o intervalo conforme necessário
            var dataResponse = await getDataRequest.ExecuteAsync();
            var values = dataResponse.Values;

            // Configura o valor a ser copiado para o novo documento usando uma requisição em lote
            var data = new List<ValueRange>
            {
                new ValueRange
                {
                    Range = "LOG!A1:Z1000", // Certifique-se de que a aba e o intervalo estão corretos
                    Values = values
                }
            };

            // Usa o método BatchUpdate para evitar múltiplas requisições
            var batchUpdateRequest = new BatchUpdateValuesRequest
            {
                Data = data,
                ValueInputOption = "RAW"
            };

            // Executa o BatchUpdate
            var batchUpdate = sheetsService.Spreadsheets.Values.BatchUpdate(batchUpdateRequest, newSheetId);
            await batchUpdate.ExecuteAsync();
        }

    }
}
