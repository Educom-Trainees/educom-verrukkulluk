
using Microsoft.AspNetCore.Mvc;

namespace Verrukkulluk.Data
{
    public interface ICrud
    {
        List<Product> ReadAllProducts();
        List<Recipe>? ReadAllRecipesByUserId(int userId);
        Product? ReadProductById(int id);
    }
}