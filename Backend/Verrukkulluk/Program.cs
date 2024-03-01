using Microsoft.EntityFrameworkCore;
using Verrukkulluk.Data;
using Verrukkulluk.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Verrukkulluk
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = Environment.GetEnvironmentVariable("VERRUKKULLUK_CONNECTION_STRING")
                                     ?? builder.Configuration.GetConnectionString("verrukkulluk") 
                                     ?? throw new InvalidOperationException("Environment variable for the connection string 'VERRUKKULLUK_CONNECTION_STRING' not found.");
            builder.Services.AddDbContext<VerrukkullukContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.TryAddScoped<ICrud, Crud>();
            builder.Services.TryAddScoped<IVerModel, VerModel>();
            builder.Services.TryAddScoped<IHomeModel, HomeModel>();
            builder.Services.TryAddScoped<IUserRecipesModel, UserRecipesModel>();
            builder.Services.TryAddScoped<IFavoritesModel, FavoritesModel>();
            builder.Services.TryAddScoped<IDetailsModel, DetailsModel>();
            builder.Services.TryAddScoped<IEventModel, EventModel>();
            builder.Services.TryAddScoped<IShopListModel, ShopListModel>();
            builder.Services.TryAddScoped<IServicer, Servicer>();
            builder.Services.TryAddScoped<ISessionManager, SessionManager>();

            builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole<int>>()
                .AddEntityFrameworkStores<VerrukkullukContext>();

            builder.Services.AddControllersWithViews()
            .AddNewtonsoftJson(opts => 
                {opts.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                    opts.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;});

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: "MyAllowSpecificOrigins", policy => { policy.WithOrigins("exp://192.168.80.1:45458", "192.168.80.1:45458").AllowAnyHeader().AllowAnyMethod(); });
            });

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(360);
            });

            builder.Services.AddRazorPages().AddMvcOptions(options =>
            {
                options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(
                    _ => "Vul het veld in");
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors("MyAllowSpecificOrigins");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Verrukkulluk}/{action=Index}/{id?}");
            app.MapRazorPages();

            await SeedDatabase.InitializeDatabase(app);

            app.Run();
        }
    }
}
