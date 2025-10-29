using OnlineShopWebApp.Data;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Db;
using OnlineShop.Db.Repostories;
using OnlineShop.Db.Interfaces;
using OnlineShopWebApp.Interfaces;
using OnlineShopWebApp.Services;
using Serilog;

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

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            string connection = builder.Configuration.GetConnectionString("online_shop");
            builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlServer(connection));

            builder.Services.AddScoped<IProductRepository, ProductDbRepository>();

            builder.Services.AddScoped<ICartRepository, CartDbRepository>();

            builder.Services.AddScoped<IOrderRepository, OrderDbRepository>();

            builder.Services.AddSingleton<IFavoriteRepository, FavoriteJsonRepository>();

            builder.Services.AddSingleton<IComparisonRepository, ComparisonJsonRepository>();

            builder.Services.AddScoped<IRolesRepository, RolesDbRepository>();

            builder.Services.AddScoped<IUserRepository, UserDbRepository>();

            builder.Services.AddScoped<IUserService, UserService>();

            var app = builder.Build();
            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();
            app.UseRouting();

            app.MapStaticAssets();

            app.MapControllerRoute(
                name: "MyArea",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
