using Microsoft.EntityFrameworkCore;
using Verrukkulluk.Data;
using Verrukkulluk.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Microsoft.DotNet.Scaffolding.Shared;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Verrukkulluk
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddCors(options => {
                options.AddDefaultPolicy(policy => {
                    policy.WithOrigins("http://localhost:3000")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
                });
            });
            // Add services to the container.
            var connectionString = Environment.GetEnvironmentVariable("VERRUKKULLUK_CONNECTION_STRING")
                                     ?? builder.Configuration.GetConnectionString("verrukkulluk") 
                                     ?? throw new InvalidOperationException("Environment variable for the connection string 'VERRUKKULLUK_CONNECTION_STRING' not found.");
            builder.Services.AddDbContext<VerrukkullukContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            builder.Services.AddLogging(Console.WriteLine);
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Verrukkulluk API",
                    Description = "Managing the Admin API"                    
                });

                // using System.Reflection;
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
                options.EnableAnnotations();
                options.SchemaFilter<EnumSchemaFilter>();
            });

            builder.Services.TryAddScoped<ICrud, Crud>();
            builder.Services.TryAddScoped<IVerModel, VerModel>();
            builder.Services.TryAddScoped<IHomeModel, HomeModel>();
            builder.Services.TryAddScoped<IUserRecipesModel, UserRecipesModel>();
            builder.Services.TryAddScoped<IFavoritesModel, FavoritesModel>();
            builder.Services.TryAddScoped<IDetailsModel, DetailsModel>();
            builder.Services.TryAddScoped<IEventModel, EventModel>();
            builder.Services.TryAddScoped<IUserEventsModel, UserEventsModel>();
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

            builder.Services.AddAutoMapper(typeof(Program));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    options.RoutePrefix = "api";
                });
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
            app.UseCors();
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
    public class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema model, SchemaFilterContext context)
        {
            if (context.Type.IsEnum)
            {
                model.Enum.Clear();
                Enum.GetNames(context.Type)
                    .ToList()
                    .ForEach(name => model.Enum.Add(new OpenApiString(name)));
            }
        }
    }
}
