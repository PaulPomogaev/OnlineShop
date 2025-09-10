namespace OnlineShopWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            app.UseHttpsRedirection();
            app.UseRouting();

            app.MapStaticAssets();
                        
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{num1?}/{num2?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
