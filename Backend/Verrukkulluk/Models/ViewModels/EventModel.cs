using Microsoft.AspNetCore.Identity;
using Verrukkulluk.Data;

namespace Verrukkulluk.Models
{
    public class EventModel:VerModel, IEventModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Event Event { get; set; }

        public EventModel() { }
        public EventModel(ICrud crud, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, SignInManager<User> signInManager) : base(crud, userManager, httpContextAccessor, signInManager)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<string> GetCurrentUserEmail()
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            return user.Email;
        }

    }
}
