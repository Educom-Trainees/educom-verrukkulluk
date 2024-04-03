using Verrukkulluk.Models.DbModels;

namespace Verrukkulluk.Models.DTOModels
{
    public class EventDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public List<string> EventParticipantName { get; set; }
        public string Place { get; set; }
        public decimal Price { get; set; }
        public int MaxParticipants { get; set; }
    }
}
