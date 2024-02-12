using Microsoft.AspNetCore.Identity;
using Verrukkulluk.Models;

namespace Verrukkulluk.Data
{
    public static class SeedDatabase
    {
        public static byte[] ReadImageFile(string fileName)
        {
            string folderPath = "wwwroot/Images/";
            string filePath = Path.Combine(folderPath, fileName);

            try
            {
                if (File.Exists(filePath))
                {
                    return File.ReadAllBytes(filePath);
                }
                else
                {
                    Console.WriteLine($"File not found: {filePath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
            }

            return null;
        }
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
                    new User("jan@jan.jan", "Jan", "Utrecht", ReadImageFile("jan.jpg")),
                    new User("bert@bert.bert", "Bert", "Sesamstraat", ReadImageFile("bert.png")),
                    new User("els@els.els", "Els", "Sittard", ReadImageFile("els.jpg")),
                    new User("a@a.a", "Albert", "Soesterberg", ReadImageFile("bert.jpg"))
                };
                User adminUser = new User("admin@admin.admin", "Admin", "Admindam", ReadImageFile("admin.png"));

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

                    ImageObj WitteBolImage = new ImageObj(ReadImageFile("witte_bol.jpg"), "jpg");
                    dbContext.ImageObjs.Add(WitteBolImage);
                    await dbContext.SaveChangesAsync();
                    ImageObj AvocadoImage = new ImageObj(ReadImageFile("avocado.jpg"), "jpg");
                    dbContext.ImageObjs.Add(AvocadoImage);
                    await dbContext.SaveChangesAsync();
                    ImageObj VeganburgersausImage = new ImageObj(ReadImageFile("veganburgersaus.png"), "png");
                    dbContext.ImageObjs.Add(VeganburgersausImage);
                    await dbContext.SaveChangesAsync();
                    ImageObj HamburgersImage = new ImageObj(ReadImageFile("hamburgers.png"), "png");
                    dbContext.ImageObjs.Add(HamburgersImage);
                    await dbContext.SaveChangesAsync();
                    ImageObj TomatenImage = new ImageObj(ReadImageFile("tomaten.png"), "png");
                    dbContext.ImageObjs.Add(TomatenImage);
                    await dbContext.SaveChangesAsync();
                    ImageObj IjsbergslaImage = new ImageObj(ReadImageFile("ijsbergsla.png"), "png");
                    dbContext.ImageObjs.Add(IjsbergslaImage);
                    await dbContext.SaveChangesAsync();
                    ImageObj BoterImage = new ImageObj(ReadImageFile("boter.png"), "png");
                    dbContext.ImageObjs.Add(BoterImage);
                    await dbContext.SaveChangesAsync();
                    ImageObj SpaghettiVImage = new ImageObj(ReadImageFile("spaghettiv.jpeg"), "jpeg");
                    dbContext.ImageObjs.Add(SpaghettiVImage);
                    await dbContext.SaveChangesAsync();
                    ImageObj CouscousVImage = new ImageObj(ReadImageFile("couscousv.jpeg"), "jpeg");
                    dbContext.ImageObjs.Add(CouscousVImage);
                    await dbContext.SaveChangesAsync();
                    ImageObj KomkommerImage = new ImageObj(ReadImageFile("komkommer.jpeg"), "jpeg");
                    dbContext.ImageObjs.Add(KomkommerImage);
                    await dbContext.SaveChangesAsync();
                    ImageObj BroccoliImage = new ImageObj(ReadImageFile("broccoli.jpeg"), "jpeg");
                    dbContext.ImageObjs.Add(BroccoliImage);
                    await dbContext.SaveChangesAsync();
                    ImageObj AardbeienImage = new ImageObj(ReadImageFile("aardbeien.jpeg"), "jpeg");
                    dbContext.ImageObjs.Add(AardbeienImage);
                    await dbContext.SaveChangesAsync();

                    Product[] products =
                    {
                        new Product("Witte Bol", 1.59m, 759, 6, IngredientType.stuks, WitteBolImage.Id, "Verpakking van zes witte bollen"),
                        new Product("Avocado", 1.39m, 335, 1, IngredientType.stuks, AvocadoImage.Id, "Verse losse avocado's"),
                        new Product("Vegan Burgersaus", 7.29m, 906, 300, IngredientType.gram, VeganburgersausImage.Id, "Fles met vegan burgersaus (300 g)"),
                        new Product("Hamburger", 3.39m, 655, 2, IngredientType.stuks, HamburgersImage.Id, "Verpakking van twee hamburgers"),
                        new Product("Tomaten", 1.39m, 105, 6, IngredientType.stuks, TomatenImage.Id, "Plastic verpakking van zes verse tomaten"),
                        new Product("Ijsbergsla", 1.09m, 25, 200, IngredientType.gram, IjsbergslaImage.Id, "Zakje ijsbergsla (200 g)"),
                        new Product("Boter", 3.79m, 1674, 225, IngredientType.gram, BoterImage.Id, "Pakje boter (225 g)"),
                        new Product("Spaghetti", 2.55m, 1835, 500, IngredientType.gram, SpaghettiVImage.Id, "Verpakking (500 g)"),
                        new Product("Couscous", 1.79m, 724, 275, IngredientType.gram, CouscousVImage.Id, "Pakje boter (225 g)"),
                        new Product("Komkommer", 1.05m, 16, 1, IngredientType.stuks, KomkommerImage.Id, "Verpakking van 1 komkommer"),
                        new Product("Broccoli", 1.85m, 24, 500, IngredientType.gram, BroccoliImage.Id, "Stronk van 500 g"),
                        new Product("Aardbeien", 3.99m, 100, 400, IngredientType.gram, AardbeienImage.Id, "Doosje aardbeien (400 g)")
                    };
                    await dbContext.SaveChangesAsync();


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
                    await dbContext.SaveChangesAsync();

                    Ingredient[] ingredients =
                    {
                        new Ingredient("Wit Bolletje", 0.66, products[0]),
                        new Ingredient("Avocado", 2, products[1]),
                        new Ingredient("Vegan Burgersaus", 0.1, products[2]),
                        new Ingredient("Hamburger", 2, products[3]),
                        new Ingredient("Tomaten", 1, products[4]),
                        new Ingredient("Ijsbergsla", 0.5, products[5]),
                        new Ingredient("Boter", 0.1, products[6]),
                        new Ingredient("Spaghetti", 0.4, products[7]),
                        new Ingredient("Couscous", 0.3, products[8]),
                        new Ingredient("Komkommer", 0.5, products[9]),
                        new Ingredient("Broccoli", 1, products[10]),
                        new Ingredient("Aardbeien", 1, products[11])
                    };
                    await dbContext.SaveChangesAsync();


                    List<RecipeDishType> vlees = new List<RecipeDishType>();
                    vlees.Add(new RecipeDishType { DishTypeId = 1 });
                    List<RecipeDishType> veganistisch = new List<RecipeDishType>();
                    veganistisch.Add(new RecipeDishType { DishTypeId = 3 });
                    veganistisch.Add(new RecipeDishType { DishTypeId = 4 });
                    List<RecipeDishType> surfturf = new List<RecipeDishType>();
                    surfturf.Add(new RecipeDishType { DishTypeId = 1 });
                    surfturf.Add(new RecipeDishType { DishTypeId = 2 });
                    List<RecipeDishType> vis = new List<RecipeDishType>();
                    vis.Add(new RecipeDishType { DishTypeId = 2 });
                    List<RecipeDishType> vegetarisch1 = new List<RecipeDishType>();
                    vegetarisch1.Add(new RecipeDishType { DishTypeId = 3 });
                    List<RecipeDishType> vegetarisch2 = new List<RecipeDishType>();
                    vegetarisch2.Add(new RecipeDishType { DishTypeId = 3 });
                    List<RecipeDishType> vegetarisch3 = new List<RecipeDishType>();
                    vegetarisch3.Add(new RecipeDishType { DishTypeId = 3 });
                    List<RecipeDishType> veganistisch1 = new List<RecipeDishType>();
                    veganistisch1.Add(new RecipeDishType { DishTypeId = 3 });
                    veganistisch1.Add(new RecipeDishType { DishTypeId = 4 });

                    await dbContext.SaveChangesAsync();

                    List<Ingredient> recipeIngredients = new List<Ingredient>();
                    foreach(Ingredient ingredient in ingredients)
                    {
                        recipeIngredients.Add(ingredient);
                    }
                    List<Ingredient> recipeIngredientsTest = new List<Ingredient>();
                    recipeIngredientsTest.Add(new Ingredient("Tomaten", 0.5, products[4]));
                    
                    
                    string instructions = "Doe boter in de pan. Bak de hamburger. Snij sla, tomaten en een bolletje. Doe de hamburger in het bolletje met de sla en tomaten.";
                    string description = "Een lekkere gerecht, snel klaar te maken en een favoriet van het hele gezin.";
                    byte[] DishPhoto = { 0 };

                    ImageObj CouscousImage = new ImageObj(ReadImageFile("pexels-ella-olsson.jpg"), "jpg");
                    dbContext.ImageObjs.Add(CouscousImage);
                    await dbContext.SaveChangesAsync();
                    ImageObj HamburgerImage = new ImageObj(ReadImageFile("pexels-robin-stickel.jpg"), "jpg");
                    dbContext.ImageObjs.Add(HamburgerImage);
                    await dbContext.SaveChangesAsync();
                    ImageObj PokeBowlImage = new ImageObj(ReadImageFile("pexels-jane-doan.jpg"), "jpg");
                    dbContext.ImageObjs.Add(PokeBowlImage);
                    await dbContext.SaveChangesAsync();
                    ImageObj SpaghettiImage = new ImageObj(ReadImageFile("pexels-lisa-fotios.jpg"), "jpg");
                    dbContext.ImageObjs.Add(SpaghettiImage);
                    await dbContext.SaveChangesAsync();
                    ImageObj PizzaImage = new ImageObj(ReadImageFile("PizzaGreen.jpg"), "jpg");
                    dbContext.ImageObjs.Add(PizzaImage);
                    await dbContext.SaveChangesAsync();
                    ImageObj SaladeImage = new ImageObj(ReadImageFile("Salade.png"), "png");
                    dbContext.ImageObjs.Add(SaladeImage);
                    await dbContext.SaveChangesAsync();

                    Recipe[] recipes =
                    {
                        new Recipe("Couscous", vegetarisch1, kitchenTypes[10], description, instructions, 4, users[0], CouscousImage.Id, recipeIngredients, 3),
                        new Recipe("Duitse Hamburger", vlees, kitchenTypes[11], description, instructions, 3, users[0], HamburgerImage.Id , recipeIngredients, 4),
                        new Recipe("Fruit Pokébowl", veganistisch, kitchenTypes[0], description, instructions, 1, users[0], PokeBowlImage.Id , recipeIngredients, 2),
                        new Recipe("Spaghetti", vegetarisch2, kitchenTypes[6], description,instructions, 5, users[0], SpaghettiImage.Id, recipeIngredientsTest,3 ),
                        new Recipe("Pizza", vegetarisch3, kitchenTypes[6], description,instructions, 5, users[0], PizzaImage.Id, recipeIngredientsTest, 2),
                        new Recipe("Salade", veganistisch1, kitchenTypes[6], description,instructions, 5, users[0], SaladeImage.Id, recipeIngredientsTest, 4)
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
                            Description = "Een workshop om op een snelle en chique manier een dinertafel te dekken",
                            Date = new DateOnly(2024, 02, 15),
                            StartTime = new TimeOnly(12, 0),
                            EndTime = new TimeOnly(17, 0),
                            Place = "De Kuip",
                            Price = 10.49m
                        },
                        new Event
                        {
                            Title = "Secuur afwassen",
                            Description = "Hier leert u hoe u kunt afwassen op een veilige en duurzame manier",
                            Date = new DateOnly(2024, 02, 25),
                            StartTime = new TimeOnly(09, 30),
                            EndTime = new TimeOnly(12, 30),
                            Place = "Johan Cruijff ArenA",
                            Price = 15.99m
                        },
                        new Event
                        {
                            Title = "Wokken",
                            Description = "Wat is wokken precies en wat maakt het nou zo lekker?",
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
                if (!dbContext.RecipeRatings.Any())
                {
                    var recipes = dbContext.Recipes.ToList();

                    foreach (var recipe in recipes)
                    {
                        //Test rating for each recipe
                        dbContext.RecipeRatings.Add(new RecipeRating
                        {
                            RecipeId = recipe.Id,
                            UserId = 1,
                            RatingValue = 3 
                        });
                    }

                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}