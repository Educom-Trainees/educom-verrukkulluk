using Microsoft.AspNetCore.Identity;
using Org.BouncyCastle.Bcpg;
using System.ComponentModel.DataAnnotations;
using Verrukkulluk.Data;

namespace Verrukkulluk.Models
{
    public class VerModel : IVerModel
    {
        protected readonly ICrud Crud;
        protected readonly UserManager<User> UserManager;
        protected readonly IHttpContextAccessor HttpContextAccessor;
        public readonly SignInManager<User> SignInManager;
        public string Error { get; set; } = "";
        public InputModel Input { get; set; } = new InputModel();
        public VerModel()
        {
             
        }

        public VerModel(ICrud crud, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, SignInManager<User> signInManager)
        {
            Crud = crud;
            UserManager = userManager;
            HttpContextAccessor = httpContextAccessor;
            SignInManager = signInManager;
        }

        public async Task<SignInResult> Login(InputModel input)
        {
            User? theUser = await UserManager.FindByEmailAsync(Input.Email);
            if (theUser == null)
            {
                return SignInResult.Failed;
            }
            return await SignInManager.PasswordSignInAsync(theUser, input.Password, input.RememberMe, lockoutOnFailure: false);
        }
    }
}
