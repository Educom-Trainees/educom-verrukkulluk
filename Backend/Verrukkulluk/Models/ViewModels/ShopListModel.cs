using Microsoft.AspNetCore.Identity;
using Verrukkulluk.Data;

namespace Verrukkulluk.Models
{
    public class ShopListModel:VerModel, IShopListModel
    {
        public List<CartItem> ShopList { get; set; }


        public ShopListModel() { }
        public ShopListModel(ICrud crud, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, SignInManager<User> signInManager) : base(crud, userManager, httpContextAccessor, signInManager)
        { }
    }
}
