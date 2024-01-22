using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Verrukkulluk.Models;

namespace Verrukkulluk.Data
{
    public class VerrukkullukContext : IdentityDbContext<User, Role, int>
    {
        public DbSet<Recipe> Recipes { get; set; } 
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<User> Users {  get; set; }
        public VerrukkullukContext(DbContextOptions<VerrukkullukContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Ingredient>().Property(e => e.IngredientType).HasConversion<string>().HasMaxLength(20);
            builder.Entity<Product>().Property(e => e.IngredientType).HasConversion<string>().HasMaxLength(20);
            base.OnModelCreating(builder);
        }
    }
}
