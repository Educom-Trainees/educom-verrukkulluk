using Verrukkulluk.Models.DbModels;

namespace Verrukkulluk
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public ICollection<EventParticipant> Participants { get; set; } = new List<EventParticipant>();
        public string Place { get; set; }
        public decimal Price { get; set; }
        public int MaxParticipants { get; set; }
    }
}



