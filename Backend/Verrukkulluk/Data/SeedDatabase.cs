using Microsoft.AspNetCore.Identity;
using Verrukkulluk.Models;

namespace Verrukkulluk.Data
{
    public static class SeedDatabase
    {
        public static async Task InitializeDatabase(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

                User[] users = {    new User("jan@jan.jan", "Jan", "Utrecht"),
                                    new User("bert@bert.bert", "Bert", "Arnhem"),
                                    new User("els@els.els", "Els", "Sittard")
                                };
                string password = "Test890!";

                foreach (var user in users)
                {
                    if (await userManager.FindByEmailAsync(user.Email) == null)
                    {
                        await userManager.CreateAsync(user, password);
                    }
                }

                
                var dbContext = scope.ServiceProvider.GetService<VerrukkullukContext>();

                if (!dbContext.Recipes.Any())
                {
                    DishType dishType = new DishType("Vlees");
                    List<DishType> dishTypes = [dishType];

                    KitchenType kitchenType = new KitchenType("Amerikaanse");
                    string instructions = "Doe boter in de pan. Bak de hamburger. Snij sla, tomaten en een bolletje. Doe de hamburger in het bolletje met de sla en tomaten.";

                    Product tomaat = new Product("Tomaten", 1.32m, 96, 6, IngredientType.Quantity, "location unknown");
                    if (!dbContext.Products.Any())
                    {
                        dbContext.Products.Add(tomaat);
                        dbContext.SaveChanges();
                    }

                    Ingredient ingredient = new Ingredient("Tomaat", 1, IngredientType.Quantity, tomaat);
                    List<Ingredient> ingredients = [ingredient];

                    Recipe recipe = new Recipe("Hamburger", dishTypes, kitchenType, instructions, 5, users[0], "location unknown", ingredients);
                    dbContext.Recipes.Add(recipe);
                    dbContext.SaveChanges();
                }
                
            }
        }
    }
}
