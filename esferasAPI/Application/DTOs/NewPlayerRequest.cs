namespace apiEsferas.Application.DTO;

public class NewPlayerRequest
{
    public string playerId {get; set;}
    public string newCharacterLink {get; set;}


    public NewPlayerRequest(string playerId)
    {
        this.playerId = playerId;
        this.newCharacterLink = newCharacterLink;
    }
}
