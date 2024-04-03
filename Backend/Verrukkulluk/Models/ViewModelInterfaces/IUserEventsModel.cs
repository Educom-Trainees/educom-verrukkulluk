using Verrukkulluk;

namespace Verrukkulluk.Models
{
    public interface IUserEventsModel
    {

        public string userEmail { get; set; }

        public List<Event> SignedUpEvents { get; set; }
    }
}


