namespace apiEsferas.Domain.Interfaces
{
    public interface IGoogleSheetsService
    {
        Task<string> addNewCharacterAsync(string newCharacterName, string plaierId, string resgisterID);

        Task<bool> IsPlayerRegisteredAsync(string playerId);

        Task<Dictionary<string,List<string>>> listPlayersAsync();

        Task<string> deletCharacterSheet(string logsLink);

        Task<string> verifyTheDataInACell(string linkSheet, string cellPosition);

    }
}
