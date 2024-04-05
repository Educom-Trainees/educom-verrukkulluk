using System.ComponentModel.DataAnnotations;
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
        public List<ParticipantDTO>? Participants { get; set; }
        public string Place { get; set; }
        public decimal Price { get; set; }

        [Range(1, int.MaxValue)]
        public int MaxParticipants { get; set; }
    }
}
