using apiEsferas.Domain.Entities;

namespace apiEsferas.Domain.Interfaces
{
    public interface IGoogleApiService
    {
        Task<string> addNewCharacterAsync(string newCharacterName, string plaierId, string resgisterID);

        Task<bool> IsPlayerRegisteredAsync(string playerId);

        Task<List<Player>> listPlayersAsync();

        Task<string> deletCharacterSheet(string logsLink);

        Task<string> getDataInACell(string linkSheet, string cellPosition);

        Task changeSpreadSheetsName(string spreadsheetURL, string spreadsheetName);

        Task changeFilePosition(string fileId, string folderId);
       Task appendNewDataToSheet(string spreadSheetLink, List<object> newData, string range);

    }
}
