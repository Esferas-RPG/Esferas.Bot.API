namespace apiEsferas.Domain.Interfaces
{
    public interface IGoogleSheetsService
    {
        Task<string> addNewCharacterAsync(string newCharacterName);
    }
}
