using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using apiEsferas.Domain.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

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

            return $"https://docs.google.com/spreadsheets/d/{file.Id}";
        }
    }
}
