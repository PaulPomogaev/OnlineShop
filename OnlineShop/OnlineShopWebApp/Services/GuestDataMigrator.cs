using OnlineShop.Db.Interfaces;

namespace OnlineShopWebApp.Services
{
    public class GuestMigrator
    {
        private readonly ICartRepository _cartRepository;
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IComparisonRepository _comparisonRepository;
        private readonly HttpContext _httpContext;

        public GuestMigrator(ICartRepository cartRepository, IFavoriteRepository favoriteRepository, IComparisonRepository comparisonRepository, HttpContext httpContext)
        {
            _cartRepository = cartRepository;
            _favoriteRepository = favoriteRepository;
            _comparisonRepository = comparisonRepository;
            _httpContext = httpContext;
        }

        public void MigrateGuestData(string userName)
        {
            
            TryInitializeSession();

            var sessionId = _httpContext.Session.Id;

            if (string.IsNullOrEmpty(sessionId))
            {
               sessionId = Guid.NewGuid().ToString("N");
            }

            var guestId = $"guest_{sessionId}";

            MigrateCart(guestId, userName);
            MigrateFavorite(guestId, userName);
            MigrateComparison(guestId, userName);
        }

        private void TryInitializeSession()
        {
            try
            {
                
                if (string.IsNullOrEmpty(_httpContext.Session.GetString("__migrator_init")))
                {
                    _httpContext.Session.SetString("__migrator_init", "1");
                    var sessionId = _httpContext.Session.Id;
                }
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Session initialization error in GuestMigrator: {ex.Message}");
            }
        }

        private void MigrateCart(string guestId, string userName)
        {
            var guestCart = _cartRepository.GetCart(guestId);

            if (guestCart.Items.Any())
            {
                foreach (var item in guestCart.Items.ToList())
                {
                    _cartRepository.AddToCart(item.ProductId, item.Quantity, userName);
                }
                _cartRepository.ClearCart(guestId);
            }
        }

        private void MigrateFavorite(string guestId, string userName)
        {
            var guestFav = _favoriteRepository.Get(guestId);

            if (guestFav?.Products.Any() == true)
            {
                foreach (var product in guestFav.Products.ToList())
                {
                    _favoriteRepository.Add(product.Id, userName);
                }
                _favoriteRepository.Clear(guestId);
            }
        }

        private void MigrateComparison(string guestId, string userName)
        {
            var guestComp = _comparisonRepository.Get(guestId);

            if (guestComp?.Products.Any() == true)
            {
                foreach (var product in guestComp.Products.ToList())
                {
                    _comparisonRepository.Add(product.Id, userName);
                }
                _comparisonRepository.Clear(guestId);
            }
        }
    }

}
