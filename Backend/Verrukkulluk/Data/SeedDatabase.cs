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
                
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();

                var roles = new[] { "VerUser", "Admin" };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole<int>(role));
                    }
                }
                

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

                User[] users =
                {
                    new User("jan@jan.jan", "Jan", "Utrecht", User.ReadImageFile("jan.jpg")),
                    new User("bert@bert.bert", "Bert", "Arnhem", User.ReadImageFile("bert.png")),
                    new User("els@els.els", "Els", "Sittard", User.ReadImageFile("els.jpg"))
                };
                User adminUser = new User("admin@admin.admin", "Admin", "Admindam", User.ReadImageFile("admin.png"));

                string password = "Test890!";

                foreach (User user in users)
                {
#pragma warning disable CS8604 // Possible null reference argument.
                    if (await userManager.FindByEmailAsync(user.Email) == null)
                    {
                        await userManager.CreateAsync(user, password);
                        await userManager.AddToRoleAsync(user, "VerUser");
                    }
                }

                if (await userManager.FindByEmailAsync(adminUser.Email) == null)
                {
                    await userManager.CreateAsync(adminUser, password);
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
#pragma warning restore CS8604 // Possible null reference argument.

                VerrukkullukContext dbContext = scope.ServiceProvider.GetRequiredService<VerrukkullukContext>();

                if (!dbContext.Products.Any() && !dbContext.Ingredients.Any() && !dbContext.KitchenTypes.Any() && !dbContext.DishTypes.Any() && !dbContext.Recipes.Any())
                {
                    DishType[] dishTypes =
                    {
                        new DishType("Vlees"),
                        new DishType("Vis"),
                        new DishType("Vegetarisch"),
                        new DishType("Vegan"),
                        new DishType("Glutenvrij"),
                        new DishType("Lactosevrij")
                    };
                    dbContext.DishTypes.AddRange(dishTypes);
                    await dbContext.SaveChangesAsync();

                    Product[] products =
                    {
                        new Product("Witte Bol", 1.59m, 759, 6, IngredientType.stuks, "location unknown"),
                        new Product("Avocado", 1.39m, 335, 1, IngredientType.stuks, "location unknown"),
                        new Product("Vegan Burgersaus", 7.29m, 906D, 300, IngredientType.gram, "location unknown"),
                        new Product("Hamburger", 3.39m, 655, 2, IngredientType.stuks, "location unknown"),
                        new Product("Tomaten", 1.39m, 105, 6, IngredientType.stuks, "location unknown"),
                        new Product("Ijsbergsla", 1.09m, 25, 200, IngredientType.gram, "location unknown"),
                        new Product("Boter", 3.79m, 1674, 225, IngredientType.gram, "location unknown")
                    };

                    KitchenType[] kitchenTypes =
                    {
                        new KitchenType("Aziatisch"),
                        new KitchenType("Amerikaans"),
                        new KitchenType("Turks"),
                        new KitchenType("Frans"),
                        new KitchenType("Grieks"),
                        new KitchenType("Hollands"),
                        new KitchenType("Italiaans"),
                        new KitchenType("Mexicaans"),
                        new KitchenType("Indisch"),
                        new KitchenType("Spaans"),
                        new KitchenType("Marokkaans"),
                        new KitchenType("Overig")
                    };

                    Ingredient[] ingredients =
                    {
                        new Ingredient("Wit Bolletje", 4, products[0]),
                        new Ingredient("Avocado", 2, products[1]),
                        new Ingredient("Vegan Burgersaus", 40, products[2]),
                        new Ingredient("Hamburger", 4, products[3]),
                        new Ingredient("Tomaten", 1, products[4]),
                        new Ingredient("Ijsbergsla", 100, products[5]),
                        new Ingredient("Boter", 10, products[6])
                    };


                    List<RecipeDishType> recipeDishTypes = new List<RecipeDishType>();
                    recipeDishTypes.Add(new RecipeDishType { DishTypeId = 1 });

                    List<Ingredient> recipeIngredients = new List<Ingredient>();
                    foreach(Ingredient ingredient in ingredients)
                    {
                        recipeIngredients.Add(ingredient);
                    }
                    string instructions = "Doe boter in de pan. Bak de hamburger. Snij sla, tomaten en een bolletje. Doe de hamburger in het bolletje met de sla en tomaten.";
                    string description = "Een heerlijke rijk gevulde hamburger uitermate geschikt voor de donderdagmiddag.";
                    byte[] DishPhoto = { 0 };

                    Recipe[] recipes =
                    {
                        new Recipe("Amerikaanse Hamburger", recipeDishTypes, kitchenTypes[1], description, instructions, 4, users[0], DishPhoto, "location unknown", recipeIngredients),
                        new Recipe("Duitse Hamburger", recipeDishTypes, kitchenTypes[1], description, instructions, 3, users[0], DishPhoto, "location unknown", recipeIngredients),
                        new Recipe("Engelse Hamburger", recipeDishTypes, kitchenTypes[1], description, instructions, 1, users[0], DishPhoto, "location unknown", recipeIngredients),
                        new Recipe("Nederlandse Hamburger", recipeDishTypes, kitchenTypes[1], description,instructions, 5, users[0], DishPhoto, "location unknown", recipeIngredients)
                    };

                    dbContext.Products.AddRange(products);
                    dbContext.KitchenTypes.AddRange(kitchenTypes);
                    dbContext.Recipes.AddRange(recipes);
                    dbContext.Ingredients.AddRange(ingredients);
                    await dbContext.SaveChangesAsync();
                }
                if (!dbContext.Events.Any())
                {
                    Event[] events =
                    {
                        new Event
                        {
                            Title = "Vegetarisch koken",
                            Description = "Een workshop vegetarisch koken, onder leiding van Trientje Hupsakee",
                            Date = new DateOnly(2024, 01, 30),
                            StartTime = new TimeOnly(14, 0),
                            EndTime = new TimeOnly(16, 0),
                            Place = "Jaarbeurs Utrecht",
                            Price = 12.99m
                        },
                        new Event
                        {
                            Title = "Tafeldekken",
                            Description = "Een workshop op een snelle en chique manier een dinertafel dekken",
                            Date = new DateOnly(2024, 02, 15),
                            StartTime = new TimeOnly(12, 0),
                            EndTime = new TimeOnly(17, 0),
                            Place = "De Kuip",
                            Price = 10.49m
                        },
                        new Event
                        {
                            Title = "Secuur afwassen",
                            Description = "Hier leert u hoe je kunt afwassen op een veilige en duurzame manier",
                            Date = new DateOnly(2024, 02, 25),
                            StartTime = new TimeOnly(09, 30),
                            EndTime = new TimeOnly(12, 30),
                            Place = "Johan Cruijff ArenA",
                            Price = 15.99m
                        },
                        new Event
                        {
                            Title = "Wokken",
                            Description = "Wat is wokken precies en wat maakt het zo lekker? Volg deze workshop en je zal alle vragen erover kunnen beantwoorden",
                            Date = new DateOnly(2024, 03, 04),
                            StartTime = new TimeOnly(10, 0),
                            EndTime = new TimeOnly(12, 30),
                            Place = "Philips Stadion",
                            Price = 18.99m
                        }
                    };

                    dbContext.Events.AddRange(events);
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
