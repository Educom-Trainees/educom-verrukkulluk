namespace Verrukkulluk.Models.DbModels
{
    public class EventParticipant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        // ?Foreign Key? public int UserId { get; set; }
    }
}
