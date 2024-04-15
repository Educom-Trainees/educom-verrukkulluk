using System.Reflection.Emit;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Verrukkulluk.Models;
using Verrukkulluk.Models.DbModels;
using Verrukkulluk;

namespace Verrukkulluk.Data
{
    public class VerrukkullukContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public DbSet<Recipe> Recipes { get; set; } 
        public DbSet<Product> Products { get; set; }
        public DbSet<PackagingType> PackagingTypes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<KitchenType> KitchenTypes { get; set; }
        public DbSet<ImageObj> ImageObjs { get; set; }
        public DbSet<Allergy> Allergies { get; set; }
        public DbSet<ProductAllergy> ProductAllergies { get; set; }
        public DbSet<RecipeRating> RecipeRatings { get; set; }
        public DbSet<EventParticipant> EventParticipants { get; set; }
        public VerrukkullukContext(DbContextOptions<VerrukkullukContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Product>().Property(e => e.IngredientType).HasConversion<string>().HasMaxLength(20);
            builder.Entity<ProductAllergy>().HasKey(e => new { e.ProductId, e.AllergyId });
            builder.Entity<Event>().Property(e => e.Price).HasColumnType("decimal(18,2)");
            builder.Entity<Event>().Property(e => e.StartTime).HasColumnType("time");
            builder.Entity<Event>().Property(e => e.EndTime).HasColumnType("time");
            builder.Entity<Recipe>().HasOne<ImageObj>().WithOne();
            builder.Entity<Recipe>().HasOne(r => r.Creator).WithMany();
            builder.Entity<Product>().HasOne<ImageObj>().WithOne();
            builder.Entity<User>().HasMany(u => u.FavouritesList).WithMany().UsingEntity("FavoriteRecipes");
            builder.Entity<KitchenType>().HasIndex(kt => kt.Name).IsUnique();
            builder.Entity<Allergy>().HasIndex(a => a.Name).IsUnique();
            builder.Entity<PackagingType>().HasIndex(pt => pt.Name).IsUnique();
            builder.Entity<Product>().HasIndex(p => p.Name).IsUnique();
            builder.Entity<Recipe>().HasIndex(r => r.Title).IsUnique();
            base.OnModelCreating(builder);
        }
    }
}
