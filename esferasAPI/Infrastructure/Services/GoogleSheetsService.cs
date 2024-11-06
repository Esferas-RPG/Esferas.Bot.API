using apiEsferas.Domain.Interfaces;
using Google.Apis.Drive.v3;
using DotNetEnv;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Drive.v3.Data;

namespace apiEsferas.Infrastructure.Services
{
    public class GoogleSheetsService : IGoogleSheetsService
    {
        private readonly DriveService driveService;
        private readonly string templateSpreadSheetId;
        private readonly string credentialFilePath;
        private readonly string destinationFolderId;

        public GoogleSheetsService()
        {
            DotNetEnv.Env.Load();

            credentialFilePath = Environment.GetEnvironmentVariable("CLIENT_CREDENTIONS_JSON_PATH");
            templateSpreadSheetId = Environment.GetEnvironmentVariable("SHEET_TEMPLATE_ID");
            destinationFolderId = Environment.GetEnvironmentVariable("FOLDER_ID");

            var credential = GoogleCredential.FromFile(credentialFilePath)
                .CreateScoped(new[] {DriveService.Scope.Drive, DriveService.Scope.DriveFile});

            driveService = new DriveService(new BaseClientService.Initializer{
                HttpClientInitializer = credential,
                ApplicationName = "Esferas API code"
            });
        }

        public async Task<string> addNewCharacterAsync(string newCharacterName)
        {
            //* definition of the request body and the parent folde where the log will be placed
            var requestBody = new Google.Apis.Drive.v3.Data.File
            {
                Name = newCharacterName,
                Parents = new List<string>{destinationFolderId}
            };

            var request = driveService.Files.Copy(requestBody, templateSpreadSheetId);
            var file = await request.ExecuteAsync();

            //* Define as permissões para que qualquer pessoa com o link possa editar
            var permission = new Permission
            {
                Type = "anyone",
                Role = "writer"  //* Altere de "reader" para "writer" para permitir edição
            };
            
            await driveService.Permissions.Create(permission, file.Id).ExecuteAsync();

            return $"https://docs.google.com/spreadsheets/d/{file.Id}";
        }
    }
}