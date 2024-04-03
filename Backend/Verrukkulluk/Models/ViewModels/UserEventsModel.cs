using Microsoft.AspNetCore.Identity;
using Verrukkulluk.Data;

namespace Verrukkulluk.Models
{
    public class UserEventsModel:VerModel, IUserEventsModel
    {
        public string userEmail { get; set; }

        public List<Event> SignedUpEvents { get; set; }

    }
}
