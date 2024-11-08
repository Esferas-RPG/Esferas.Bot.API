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

        public async Task<string> registNewCharacter(string newCharacterName)
        {
            return await googleSheetsService.addNewCharacterAsync(newCharacterName);
        }

        internal async Task registNewCharacter(Func<string> characterName)
        {
            throw new NotImplementedException();
        }
    }
}
