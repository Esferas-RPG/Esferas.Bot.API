namespace apiEsferas.Domain.entities;

public class Character
{
    private string characterName;
    private string characterRace;//because he is a racist, love race in his horse;
    private string characterClass;
    private string characterBackground;
    private string characterGuild;
    private string characterImageLink;


    public Character()
    {
    }

    public Character(
        string characterName,
        string characterRace,
        string characterClass,
        string characterBackground,
        string characterGuild,
        string characterImageLink
    )
    {
        this.characterName = characterName;
        this.characterRace = characterRace;
        this.characterClass = characterClass;
        this.characterGuild = characterGuild;
        this.characterBackground = characterBackground;
        this.characterImageLink = characterImageLink;
    }

    #region Gets
    public string getCharacterName()
    {
        return this.characterName;
    }
    public string getCharcterRace()
    {
        return this.characterRace;
    }
    public string getCharacterClass()
    {
        return this.characterClass;
    }
    public string getcharacterBackground()
    {
        return this.characterBackground;
    }
    public string getCharacterGuild()
    {
        return this.characterGuild;
    }
    public string getCharacterImageLink()
    {
        return this.characterImageLink;
    }
    #endregion

    #region Sets
    public void setCharacterName(string characterName)
    {
        this.characterName= characterName;
    }
    public void setCharacterRace(string characterRace)
    {
        this.characterRace = characterRace;
    }
    public void setCharacterClass(string characterClass)
    {
        this.characterClass= characterClass;
    }
    public void setCharacterGuild(string characterGuild)
    {
        this.characterGuild = characterGuild;
    }
    public void setcharacterBackground(string characterBackground)
    {
        this.characterBackground = characterBackground;
    }
    public void setCharacterImageLink(string characterImageLink)
    {
        this.characterImageLink = characterImageLink;
    }
    #endregion
}