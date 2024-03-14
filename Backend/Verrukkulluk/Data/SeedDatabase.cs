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

                VerrukkullukContext dbContext = scope.ServiceProvider.GetRequiredService<VerrukkullukContext>();

                if (!dbContext.Products.Any() && !dbContext.Ingredients.Any() && !dbContext.KitchenTypes.Any() && !dbContext.Recipes.Any())
                {
                    ImageObj Jan = new ImageObj(ReadImageFile("jan.jpg"), "jpg");
                    dbContext.ImageObjs.Add(Jan);
                    ImageObj Bert = new ImageObj(ReadImageFile("bert.png"), "png");
                    dbContext.ImageObjs.Add(Bert);
                    ImageObj Els = new ImageObj(ReadImageFile("els.jpg"), "jpg");
                    dbContext.ImageObjs.Add(Els);
                    ImageObj Anoniem = new ImageObj(ReadImageFile("anoniem.png"), "png");
                    dbContext.ImageObjs.Add(Anoniem);
                    ImageObj Admin = new ImageObj(ReadImageFile("admin.png"), "png");
                    dbContext.ImageObjs.Add(Admin);
                    await dbContext.SaveChangesAsync();
                    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

                    User[] users =
                    {
                        new User("jan@jan.jan", "Jan", "Utrecht", Jan.Id, "0612345678"),
                        new User("bert@bert.bert", "Bert", "Sesamstraat", Bert.Id, "0612345678"),
                        new User("els@els.els", "Els", "Sittard", Els.Id, "0612345678"),
                        new User("a@a.a", "Albert", "Soesterberg", Anoniem.Id, "0612345678")
                    };
                    User adminUser = new User("admin@admin.admin", "Admin", "Admindam", Admin.Id, "0612345678");

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
                    ImageObj EiIcoon = new ImageObj(ReadImageFile("allergenen/ei.png"), "png");
                    dbContext.ImageObjs.Add(EiIcoon);
                    ImageObj GlutenIcoon = new ImageObj(ReadImageFile("allergenen/gluten.png"), "png");
                    dbContext.ImageObjs.Add(GlutenIcoon);
                    ImageObj LupineIcoon = new ImageObj(ReadImageFile("allergenen/lupine.png"), "png");
                    dbContext.ImageObjs.Add(LupineIcoon);
                    ImageObj MelkIcoon = new ImageObj(ReadImageFile("allergenen/melk.png"), "png");
                    dbContext.ImageObjs.Add(MelkIcoon);
                    ImageObj MosterdIcoon = new ImageObj(ReadImageFile("allergenen/mosterd.png"), "png");
                    dbContext.ImageObjs.Add(MosterdIcoon);
                    ImageObj NotenIcoon = new ImageObj(ReadImageFile("allergenen/noten.png"), "png");
                    dbContext.ImageObjs.Add(NotenIcoon);
                    ImageObj PindasIcoon = new ImageObj(ReadImageFile("allergenen/pindas.png"), "png");
                    dbContext.ImageObjs.Add(PindasIcoon);
                    ImageObj SchaaldIcoon = new ImageObj(ReadImageFile("allergenen/schaald.png"), "png");
                    dbContext.ImageObjs.Add(SchaaldIcoon);
                    ImageObj SelderijIcoon = new ImageObj(ReadImageFile("allergenen/selderij.png"), "png");
                    dbContext.ImageObjs.Add(SelderijIcoon);
                    ImageObj SesamzaadIcoon = new ImageObj(ReadImageFile("allergenen/sesamzaad.png"), "png");
                    dbContext.ImageObjs.Add(SesamzaadIcoon);
                    ImageObj SojaIcoon = new ImageObj(ReadImageFile("allergenen/soja.png"), "png");
                    dbContext.ImageObjs.Add(SojaIcoon);
                    ImageObj VisIcoon = new ImageObj(ReadImageFile("allergenen/vis.png"), "png");
                    dbContext.ImageObjs.Add(VisIcoon);
                    ImageObj WeekdierenIcoon = new ImageObj(ReadImageFile("allergenen/weekdieren.png"), "png");
                    dbContext.ImageObjs.Add(WeekdierenIcoon);
                    ImageObj ZwavelIcoon = new ImageObj(ReadImageFile("allergenen/zwavel.png"), "png");
                    dbContext.ImageObjs.Add(ZwavelIcoon);
                    ImageObj VleesIcoon = new ImageObj(ReadImageFile("allergenen/vlees.png"), "png");
                    dbContext.ImageObjs.Add(VleesIcoon);
                    await dbContext.SaveChangesAsync();

                    PackagingType[] packagingTypes =
                    {
                        new PackagingType(0, "Los"),
                        new PackagingType(1, "Pak"),
                        new PackagingType(2, "Duo-pack"),
                        new PackagingType(3, "Doos"),
                        new PackagingType(4, "Four-pack"),
                        new PackagingType(5, "Fles"),
                        new PackagingType(6, "Six-pack"),
                        new PackagingType(7, "Blik"),
                        new PackagingType(8, "Kuipje"),
                        new PackagingType(9, "Zak"),
                        new PackagingType(10, "Net"),
                        new PackagingType(11, "Pot"),
                        new PackagingType(12, "Plastic zak"),
                        new PackagingType(13, "Plastic verpakking")
                    };
                    dbContext.PackagingTypes.AddRange(packagingTypes);
                    await dbContext.SaveChangesAsync();

                    Product[] products =
                    {
                        new Product("Witte Bol", 1.59m, 759, 6, packagingTypes[12],IngredientType.stuks, WitteBolImage.Id, "Verpakking van zes witte bollen"),
                        new Product("Avocado", 1.39m, 335, 1, packagingTypes[0],IngredientType.stuks, AvocadoImage.Id, "Verse losse avocado's"),
                        new Product("Vegan Burgersaus", 7.29m, 906, 300, packagingTypes[5],IngredientType.gram, VeganburgersausImage.Id, "Fles met vegan burgersaus (300 g)"),
                        new Product("Hamburger", 3.39m, 655, 2, packagingTypes[2], IngredientType.stuks, HamburgersImage.Id, "Verpakking van twee hamburgers"),
                        new Product("Tomaten", 1.39m, 105, 6, packagingTypes[0], IngredientType.stuks, TomatenImage.Id, "Plastic verpakking van zes verse tomaten"),
                        new Product("Ijsbergsla", 1.09m, 25, 200, packagingTypes[0], IngredientType.gram, IjsbergslaImage.Id, "Zakje ijsbergsla (200 g)"),
                        new Product("Boter", 3.79m, 1674, 225, packagingTypes[8], IngredientType.gram, BoterImage.Id, "Pakje boter (225 g)"),
                        new Product("Spaghetti", 2.55m, 1835, 500, packagingTypes[13], IngredientType.gram, SpaghettiVImage.Id, "Verpakking (500 g)", 0.5),
                        new Product("Couscous", 1.79m, 724, 275, packagingTypes[0], IngredientType.gram, CouscousVImage.Id, "Pakje boter (225 g)"),
                        new Product("Komkommer", 1.05m, 16, 1, packagingTypes[0], IngredientType.stuks, KomkommerImage.Id, "Verpakking van 1 komkommer"),
                        new Product("Broccoli", 1.85m, 24, 500, packagingTypes[0], IngredientType.gram, BroccoliImage.Id, "Stronk van 500 g"),
                        new Product("Aardbeien", 3.99m, 100, 400, packagingTypes[10], IngredientType.gram, AardbeienImage.Id, "Doosje aardbeien (400 g)")
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
                        new Ingredient("Wit Bolletje", 4, products[0]),
                        new Ingredient("Avocado", 2, products[1]),
                        new Ingredient("Vegan Burgersaus", 30, products[2]),
                        new Ingredient("Hamburger", 4, products[3]),
                        new Ingredient("Tomaten", 1, products[4]),
                        new Ingredient("Ijsbergsla", 100, products[5]),
                        new Ingredient("Boter", 25, products[6]),
                        new Ingredient("Spaghetti", 200, products[7]),
                        new Ingredient("Couscous", 75, products[8]),
                        new Ingredient("Komkommer", 0.5, products[9]),
                        new Ingredient("Broccoli", 250, products[10]),
                        new Ingredient("Aardbeien", 400, products[11]),
                        new Ingredient("Hamburger", 4, products[3])
                    };
                    await dbContext.SaveChangesAsync();

                    List<Ingredient> recipeIngredients = new List<Ingredient>();
                    foreach(Ingredient ingredient in ingredients)
                    {
                        recipeIngredients.Add(ingredient);
                    }
                    List<Ingredient> recipeIngredientsTest = new List<Ingredient>();
                    recipeIngredientsTest.Add(new Ingredient("Tomaten", 0.5, products[4]));
                    List<Ingredient> burgerIngredients = new List<Ingredient>();
                    burgerIngredients.Add(ingredients[12]);
                    
                    string[] instructions = 
                    {
                        "Doe boter in de pan.",
                        "Bak de hamburger.",
                        "Snij sla, tomaten en een bolletje.",
                        "Doe de hamburger in het bolletje met de sla en tomaten."
                    };
                    
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
                        new Recipe("Couscous", kitchenTypes[10], description, instructions, 0, users[0], CouscousImage.Id, recipeIngredients, 3),
                        new Recipe("Duitse Hamburger", kitchenTypes[11], description, instructions, 0, users[0], HamburgerImage.Id , burgerIngredients, 4),
                        new Recipe("Fruit Pokébowl", kitchenTypes[0], description, instructions, 0, users[0], PokeBowlImage.Id , recipeIngredients, 2),
                        new Recipe("Spaghetti", kitchenTypes[6], description,instructions, 0, users[0], SpaghettiImage.Id, recipeIngredientsTest,3 ),
                        new Recipe("Pizza", kitchenTypes[6], description,instructions, 0, users[0], PizzaImage.Id, recipeIngredientsTest, 2),
                        new Recipe("Salade", kitchenTypes[6], description,instructions, 0, users[0], SaladeImage.Id, recipeIngredientsTest, 4)
                    };

                    dbContext.Products.AddRange(products);
                    dbContext.KitchenTypes.AddRange(kitchenTypes);
                    dbContext.Recipes.AddRange(recipes);
                    dbContext.Ingredients.AddRange(ingredients);
                    await dbContext.SaveChangesAsync();

                    Allergy[] allergies =
                    {
                        new Allergy("Ei", EiIcoon.Id),
                        new Allergy("Gluten", GlutenIcoon.Id),
                        new Allergy("Lupine", LupineIcoon.Id),
                        new Allergy("Melk", MelkIcoon.Id),
                        new Allergy("Mosterd", MosterdIcoon.Id),
                        new Allergy("Noten", NotenIcoon.Id),
                        new Allergy("Pindas", PindasIcoon.Id),
                        new Allergy("Schaaldieren", SchaaldIcoon.Id),
                        new Allergy("Selderij", SelderijIcoon.Id),
                        new Allergy("Sesamzaad", SesamzaadIcoon.Id),
                        new Allergy("Soja", SojaIcoon.Id),
                        new Allergy("Vis", VisIcoon.Id),
                        new Allergy("Weekdieren", WeekdierenIcoon.Id),
                        new Allergy("Zwavel", ZwavelIcoon.Id),
                        new Allergy("Vlees", VleesIcoon.Id)
                    };
                    dbContext.Allergies.AddRange(allergies);
                    await dbContext.SaveChangesAsync();

                    ProductAllergy[] productAllergies =
                    {
                        new ProductAllergy(allergies[1], products[0]),
                        new ProductAllergy(allergies[10], products[0]),
                        new ProductAllergy(allergies[4], products[2]),
                        new ProductAllergy(allergies[14], products[3]),
                        new ProductAllergy(allergies[3], products[6]),
                        new ProductAllergy(allergies[1], products[7])
                    };
                    dbContext.ProductAllergies.AddRange(productAllergies);
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
                            Price = 12.99m,
                            MaxParticipants = 10
                        },
                        new Event
                        {
                            Title = "Tafeldekken",
                            Description = "Een workshop om op een snelle en chique manier een dinertafel te dekken",
                            Date = new DateOnly(2024, 02, 15),
                            StartTime = new TimeOnly(12, 0),
                            EndTime = new TimeOnly(17, 0),
                            Place = "De Kuip",
                            Price = 10.49m,
                            MaxParticipants = 500
                        },
                        new Event
                        {
                            Title = "Secuur afwassen",
                            Description = "Hier leert u hoe u kunt afwassen op een veilige en duurzame manier",
                            Date = new DateOnly(2024, 02, 25),
                            StartTime = new TimeOnly(09, 30),
                            EndTime = new TimeOnly(12, 30),
                            Place = "Johan Cruijff ArenA",
                            Price = 15.99m,
                            MaxParticipants = 20
                        },
                        new Event
                        {
                            Title = "Wokken",
                            Description = "Wat is wokken precies en wat maakt het nou zo lekker?",
                            Date = new DateOnly(2024, 03, 04),
                            StartTime = new TimeOnly(10, 0),
                            EndTime = new TimeOnly(12, 30),
                            Place = "Philips Stadion",
                            Price = 18.99m,
                            MaxParticipants = 10
                        }
                    };

                    dbContext.Events.AddRange(events);
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}