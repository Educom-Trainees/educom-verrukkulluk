namespace Verrukkulluk.Models.DbModels
{
    public class EventParticipant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }

    }
}
