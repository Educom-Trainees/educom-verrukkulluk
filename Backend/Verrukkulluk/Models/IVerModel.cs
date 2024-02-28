
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Verrukkulluk.Data;

namespace Verrukkulluk.Models
{
    public interface IVerModel
    {
        string Error { get; set; }
        InputModel Input { get; set; }
        Task<SignInResult> Login(InputModel input);
        Task<User> GetLoggedInUserAsync(ClaimsPrincipal user);
    }
}