namespace apiEsferas.Domain.Entities
{
    public class Player
    {
        public string playerId {get;set;}

        public string[] characterLink {get; set;}

        public Player()
        {
            this.characterLink = new string[3];
        }

        public Player(string playerId)
        {
            this.playerId = playerId;
            this.characterLink = new string[3];
        }

        public void setPlayerCharacterLink(int position, string characterLink)
        {
            this.characterLink[position] = characterLink;
        }


    }
}