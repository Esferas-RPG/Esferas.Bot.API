namespace apiEsferas.Domain.Interfaces
{
    public interface IGoogleSheetsService
    {
        Task<string> addNewCharacterAsync(string newCharacterName, string plaierId);

        Task<bool> IsPlayerRegisteredAsync(string playerId);

        Task<Dictionary<string,List<string>>> listPlayersAsync();

        Task<string> deletCharacterSheet(string logsLink);

    }
}
