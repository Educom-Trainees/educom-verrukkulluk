using Microsoft.AspNetCore.Identity;
using Verrukkulluk.Data;

namespace Verrukkulluk.Models
{
    public class EventModel:VerModel, IEventModel
    {

        public Event Event { get; set; }

        public EventModel() { }
        public EventModel(ICrud crud, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, SignInManager<User> signInManager) : base(crud, userManager, httpContextAccessor, signInManager)
        {
        }

    }
}
