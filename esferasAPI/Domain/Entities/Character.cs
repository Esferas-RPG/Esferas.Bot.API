namespace apiEsferas.Domain.Entities
{
    public class Character
    {
        public string CharacterName { get; set; }
        public string CharacterRace { get; set; }
        public string CharacterClass { get; set; }
        public string CharacterBackground { get; set; }
        public string CharacterGuild { get; set; }
        public string CharacterImageLink { get; set; }

        // Default constructor
        public Character() { }

        // Parameterized constructor
        public Character(
            string characterName,
            string characterRace,
            string characterClass,
            string characterBackground,
            string characterGuild,
            string characterImageLink)
        {
            CharacterName = characterName;
            CharacterRace = characterRace;
            CharacterClass = characterClass;
            CharacterBackground = characterBackground;
            CharacterGuild = characterGuild;
            CharacterImageLink = characterImageLink;
        }
    }
}
