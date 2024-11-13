using apiEsferas.Domain.Interfaces;
using apiEsferas.Domain.Entities;

namespace apiEsferas.Application.Sevices
{
    public class GoogleApiAppService
    {
        //* atribut
        private readonly IGoogleApiService googleApiService;
        //* constructure
        public GoogleApiAppService(IGoogleApiService googleApiService)
        {
            //yes, this, because: This nutts works
            this.googleApiService = googleApiService;
        }
        public async Task<string> registNewCharacter(string newCharacterName, string playerId, string registerIs)
        {
            return await googleApiService.addNewCharacterAsync(newCharacterName, playerId, registerIs);
        }
        public async Task<bool> verifyIfPlayerAlreadyRegist(string playerId)
        {
            return await googleApiService.IsPlayerRegisteredAsync(playerId);
        }
        public async Task<string> deleteCharacterSheets(string logsLink)
        {
            return await googleApiService.deletCharacterSheet(logsLink);
        }
        public async Task<List<Player>> listPlayers()
        {
            return await googleApiService.listPlayersAsync();
        }

        public async Task<string> getDataInACell(string linkSheet, string cellPosition)
        {
            return await googleApiService.getDataInACell( linkSheet, cellPosition);
        }

        public async Task changeSpreadSheetsName(string spreadsheetURL, string spreadsheetName)
        {
            await googleApiService.changeSpreadSheetsName( spreadsheetURL,  spreadsheetName);
        }


        public async Task changeFilePosition(string fileId, string folderId)
        {
            await googleApiService.changeFilePosition(fileId, folderId);
        }
    }
}
