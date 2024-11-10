using apiEsferas.Domain.Interfaces;

namespace apiEsferas.Application.Sevices
{
    public class SpreadSheetAppService
    {
        private readonly IGoogleSheetsService googleSheetsService;
    
        public SpreadSheetAppService(IGoogleSheetsService googleSheetsService)
        {
            //yes, this, because: This nutts works
            this.googleSheetsService = googleSheetsService;
        }

        public async Task<string> registNewCharacter(string newCharacterName, string playerId, string registerIs)
        {
            return await googleSheetsService.addNewCharacterAsync(newCharacterName, playerId, registerIs);
        }

        public async Task<bool> verifyIfPlayerAlreadyRegist(string playerId)
        {
            return await googleSheetsService.IsPlayerRegisteredAsync(playerId);
        }

        public async Task<string> deleteCharacterSheets(string logsLink)
        {
            return await googleSheetsService.deletCharacterSheet(logsLink);
        }
        public async Task<Dictionary<string,List<string>>> listPlayers()
        {
            return await googleSheetsService.listPlayersAsync();
        }

        public async Task<string> verifyTheDataInACell(string linkSheet, string cellPosition)
        {
            return await verifyTheDataInACell( linkSheet, cellPosition);
        }
    }
}
