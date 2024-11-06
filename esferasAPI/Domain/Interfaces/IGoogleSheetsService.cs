using Google.Apis.Sheets.v4.Data;

namespace apiEsferas.Domain.Interfaces
{
    public interface IGoogleSheetsService
    {
        Task<string> addNewCharacterAsync(string newCharacterName);
    }
}
