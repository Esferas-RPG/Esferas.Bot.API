namespace apiEsferas.Application.DTO;

public class CharacterRequest
{
    public string playerId {get; set;}
    public string newCharacterName {get; set;}


    public CharacterRequest(string newCharacterName)
    {
        this.newCharacterName = newCharacterName;
    }

}