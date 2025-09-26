using OnlineShopWebApp.Data;
using OnlineShopWebApp.Interfaces;

namespace OnlineShopWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddSingleton<IProductRepository, ProductJsonRepository>();

            builder.Services.AddSingleton<ICartRepository, CartJsonRepository>();

            builder.Services.AddSingleton<IOrderRepository, OrderJsonRepository>();

            builder.Services.AddSingleton<IFavoriteRepository, FavoriteJsonRepository>();

            var app = builder.Build();

            app.UseHttpsRedirection();
            app.UseRouting();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
