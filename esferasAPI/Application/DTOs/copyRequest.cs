
namespace apiEsferas.Application.DTO;
public class CopyRequest
{
    public string newCharacterName {get; set;}


    public CopyRequest(string newCharacterName)
    {
        this.newCharacterName = newCharacterName;
    }

}