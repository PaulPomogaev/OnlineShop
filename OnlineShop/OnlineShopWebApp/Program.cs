using FluentValidation;
using OnlineShop.Core.Validators;
using OnlineShop.Db.Behaviors;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Db;
using OnlineShop.Db.Repostories;
using OnlineShop.Db.Interfaces;
using Serilog;
using Microsoft.AspNetCore.Identity;
using OnlineShop.Db.Models;
using OnlineShop.Core.Interfaces;
using OnlineShopWebApp.Services;
using OnlineShop.Core.Models.Products.Commands;
using OnlineShop.Db.Handlers.Products.Commands;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;
using OnlineShop.Db.Services;
using MediatR;

namespace OnlineShopWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog((context, services, configuration) =>
            {
                configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .Enrich.WithProperty("ApplicationName", "OnlineShopWebApp");
            });

            builder.Logging.AddFilter("LuckyPennySoftware.MediatR.License", LogLevel.None); // Фильтрация предупреждений о лицензии MediatR

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.Name = "OnlineShop.Session";
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; 
                options.Cookie.SameSite = SameSiteMode.Lax;
            });

            builder.Services.AddHttpClient<TokenService>(client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["ReviewsApi:BaseUrl"] ?? "https://localhost:7274");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.Timeout = TimeSpan.FromSeconds(30);
            });

            builder.Services.AddHttpClient<IReviewsApiService, ReviewsApiService>(client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["ReviewsApi:BaseUrl"] ?? "https://localhost:7274");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.Timeout = TimeSpan.FromSeconds(30);
            });

           
            string connection = builder.Configuration.GetConnectionString("online_shop");

            builder.Services.AddDbContext<DatabaseContext>(options =>
               options.UseSqlServer(connection));

            builder.Services.AddIdentity<User, Role>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;

                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
            })
             .AddEntityFrameworkStores<DatabaseContext>()
             .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.ExpireTimeSpan = TimeSpan.FromHours(720);
                options.SlidingExpiration = true;

                options.Cookie = new CookieBuilder
                {
                    IsEssential = true,
                    SameSite = SameSiteMode.Lax, 
                    HttpOnly = true,
                    SecurePolicy = CookieSecurePolicy.SameAsRequest 
                };
            });

            string memcachedAddress = builder.Configuration["Memcached:Address"] ?? "localhost";
            int memcachedPort = builder.Configuration.GetValue<int>("Memcached:Port", 11211);

            builder.Services.AddEnyimMemcached(options =>
            {
                options.Servers = new List<Server>
                {
                   new Server
                   {
                     Address = memcachedAddress,
                     Port = memcachedPort
                   }
                };
                options.Protocol = MemcachedProtocol.Binary;
                options.SocketPool = new SocketPoolOptions
                {
                    MinPoolSize = 5,
                    MaxPoolSize = 100,
                    ConnectionTimeout = TimeSpan.FromSeconds(10),
                    ReceiveTimeout = TimeSpan.FromSeconds(10),
                    DeadTimeout = TimeSpan.FromSeconds(10)
                };
            });
                        
            builder.Services.AddScoped<ICacheService, MemcachedCacheService>();

            builder.Services.AddValidatorsFromAssemblyContaining<CreateProductCommandValidator>(); // Регистрируем валидатор из OnlineShop.Core

            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies( // регистрируем MediatR
               typeof(Program).Assembly,                    
               typeof(CreateProductCommand).Assembly,       
               typeof(CreateProductCommandHandler).Assembly 
            ));

            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>)); // Регистрируем PipelineBehavior

            builder.Services.AddScoped<IProductQueryRepository, ProductQueryRepository>();
            builder.Services.AddScoped<IProductCommandRepository, ProductCommandRepository>();
            builder.Services.AddScoped<ICartRepository, CartDbRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderDbRepository>();
            builder.Services.AddScoped<IFavoriteRepository, FavoriteDbRepository>();
            builder.Services.AddScoped<IComparisonRepository, ComparisonDbRepository>();

            
            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                db.Database.Migrate();
            }

            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
                IdentityInitializer.Initialize(userManager, roleManager);
            }

            app.UseSerilogRequestLogging();
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseSession();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();

            app.MapAreaControllerRoute(
                name: "admin",
                areaName: "Admin",
                pattern: "Admin/{controller=Products}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "MyArea",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
