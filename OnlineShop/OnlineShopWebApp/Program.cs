using OnlineShopWebApp.Data;
using OnlineShopWebApp.Interfaces;
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

            builder.Services.AddSingleton<IProductRepository, ProductJsonRepository>();

            builder.Services.AddSingleton<ICartRepository, CartJsonRepository>();

            builder.Services.AddSingleton<IOrderRepository, OrderJsonRepository>();

            builder.Services.AddSingleton<IFavoriteRepository, FavoriteJsonRepository>();

            builder.Services.AddSingleton<IComparisonRepository, ComparisonJsonRepository>();

            builder.Services.AddSingleton<IRolesRepository, RolesJsonRepository>();

            builder.Services.AddSingleton<IUserRepository, UserJsonRepository>();

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
