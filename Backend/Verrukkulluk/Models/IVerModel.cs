
using Microsoft.AspNetCore.Identity;
using Verrukkulluk.Data;

namespace Verrukkulluk.Models
{
    public interface IVerModel
    {
        string Error { get; set; }
        InputModel Input { get; set; }

        List<Product> GetAllProducts();
        Product? GetProductById(int productId);
        Task<SignInResult> Login(InputModel input);
    }
}