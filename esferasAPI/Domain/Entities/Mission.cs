
namespace apiEsferas.Domain.Entities
{
    public class Mission
    {
        public DateTime missionDate {get; set;}
        public DateTime missionStart {get; set;}
        public DateTime missionEnd {get; set;}
        public List<Player> playersList {get; set;}

        public Mission()
        {
            this.playersList = new List<Player>();
        }

        public Mission(DateTime missionDate)
        {
            this.missionDate = missionDate;
            this.playersList = new List<Player>();
        }
    }
}