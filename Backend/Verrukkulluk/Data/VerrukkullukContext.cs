using System.Reflection.Emit;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Verrukkulluk.Models;

namespace Verrukkulluk.Data
{
    public class VerrukkullukContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public DbSet<Recipe> Recipes { get; set; } 
        public DbSet<Product> Products { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<DishType> DishTypes { get; set; }
        public DbSet<KitchenType> KitchenTypes { get; set; }
        public DbSet<RecipeDishType> RecipeDishTypes { get; set; }
        public DbSet<ImageObj> ImageObjs { get; set; }
        public DbSet<Allergy> Allergies { get; set; }
        public DbSet<ProductAllergy> ProductAllergies { get; set; }
        public VerrukkullukContext(DbContextOptions<VerrukkullukContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Product>().Property(e => e.IngredientType).HasConversion<string>().HasMaxLength(20);
            builder.Entity<Event>().Property(e => e.Price).HasColumnType("decimal(18,2)");
            builder.Entity<Event>().Property(e => e.StartTime).HasColumnType("time");
            builder.Entity<Event>().Property(e => e.EndTime).HasColumnType("time");
            builder.Entity<Recipe>().HasOne<ImageObj>().WithOne();
            builder.Entity<Product>().HasOne<ImageObj>().WithOne();
            base.OnModelCreating(builder);
        }
    }
}
