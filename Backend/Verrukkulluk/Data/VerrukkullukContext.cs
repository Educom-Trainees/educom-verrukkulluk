﻿using Microsoft.AspNetCore.Identity;
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
        public DbSet<User> Users {  get; set; }
        public DbSet<DishType> DishTypes { get; set; }
        public DbSet<KitchenType> KitchenTypes { get; set; }
        public VerrukkullukContext(DbContextOptions<VerrukkullukContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Product>().Property(e => e.IngredientType).HasConversion<string>().HasMaxLength(20);
            base.OnModelCreating(builder);
        }
    }
}
