🛒 OnlineShop — учебный интернет-магазин на ASP.NET Core MVC
<div align="center"> <img src="https://github.com/user-attachments/assets/39738078-89b3-4b3e-b98e-55842b35b813" alt="ASP.NET Core разработка" width="80%"> <br> <sub>Полноценный веб-сайт интернет-магазина с корзиной, избранным, заказами и админ-панелью</sub> </div>
📂 Содержание
Архитектура репозитория

Ключевые возможности

Технологический стек

Архитектурные решения и примеры кода

Демонстрация

Запуск проекта

🏛️ Архитектура репозитория
text
📁 OnlineShop
├── 📁 OnlineShop.Core               # Модели, DTO, интерфейсы, команды/запросы CQRS
├── 📁 OnlineShop.Db                  # Контекст БД, миграции, репозитории, обработчики CQRS
├── 📁 OnlineShopWebApp                # MVC-приложение
│   ├── 📁 Areas                       # Admin и UserProfile области
│   ├── 📁 Controllers                  # Cart, Product, Order, Account и др.
│   ├── 📁 Helpers                      # Мапперы, UserIdHelper, EnumExtensions
│   ├── 📁 Models                       # ViewModel'и
│   ├── 📁 Services                      # ReviewsApiService, GuestDataMigrator, TokenService
│   └── 📁 Views                         # Razor-представления
└── 📄 OnlineShopWebApp.sln               # Файл решения
💡 Ключевые возможности
📦 Управление товарами и каталог
Просмотр товаров с пагинацией и поиском

Детальная карточка товара с фото и описанием

Полный CRUD для администратора (область Admin)

🛍️ Корзина, избранное, сравнение
Добавление/удаление товаров в корзину, изменение количества

Сохранение списков избранного и сравнения для гостей и авторизованных

Автоматическая миграция данных гостя при входе (GuestDataMigrator)

🔧 Пример кода (репозиторий корзины):

csharp
public void AddToCart(int productId, int quantity, string userId)
{
    var cart = GetCart(userId);
    var existingItem = cart.Items.FirstOrDefault(item => item.ProductId == productId);
    if (existingItem != null)
        existingItem.Quantity += quantity;
    else
        cart.Items.Add(new CartItem { ProductId = productId, Quantity = quantity });
    _context.SaveChanges();
}
📦 Оформление заказов
Создание заказа из корзины с валидацией данных

Просмотр истории заказов в личном кабинете

Управление статусами заказов для администратора

👤 Аутентификация и авторизация
Регистрация и вход с ASP.NET Core Identity

Разделение ролей: пользователь / администратор

Личный кабинет с редактированием профиля и сменой пароля

🔧 Администрирование
Отдельная область /Admin с управлением товарами, заказами, пользователями и ролями

💬 Интеграция с микросервисом отзывов
Асинхронное получение отзывов и рейтингов через ReviewsApiService

Микросервис Reviews запускается отдельно, общение по HTTP

🔧 Пример кода (сервис для отзывов):

csharp
public async Task<List<Review>> GetReviewsByProductIdAsync(int productId)
{
    await AddAuthorizationHeader();
    var response = await _httpClient.GetAsync($"/api/Reviews/filter?productId={productId}");
    if (response.IsSuccessStatusCode)
        return await response.Content.ReadFromJsonAsync<List<Review>>();
    return new List<Review>();
}
🔧 Технологический стек
ASP.NET Core 8 MVC (контроллеры, представления, middleware)

Entity Framework Core 8 (Code First, миграции)

SQL Server (LocalDB / Express)

ASP.NET Core Identity (аутентификация, роли)

CQRS + MediatR (разделение команд и запросов)

Кеширование (Memcached через ICacheService)

FluentValidation (валидация моделей)

Serilog (логирование в файл и консоль)

Bootstrap 5, jQuery (фронтенд)

Git / GitHub

🧠 Архитектурные решения и примеры кода
1. Чистая архитектура и CQRS с MediatR
Команды и запросы вынесены в Core, обработчики — в Db. Кеширование прозрачно добавляется в обработчики.

Запрос:

csharp
public record GetProductByIdQuery(int Id) : IRequest<ProductDto?>;
Обработчик с кешированием:

csharp
public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
{
    private readonly IProductQueryRepository _productQueryRepository;
    private readonly ICacheService _cache;

    public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"product_{request.Id}";
        var cachedProduct = await _cache.GetAsync<ProductDto?>(cacheKey);
        if (cachedProduct != null) return cachedProduct;

        var product = await _productQueryRepository.GetByIdAsync(request.Id);
        if (product == null) return null;

        var productDto = product.ToDto();
        await _cache.SetAsync(cacheKey, productDto, TimeSpan.FromMinutes(10));
        return productDto;
    }
}
2. Репозитории и работа с корзиной
Используется паттерн Repository для инкапсуляции доступа к данным.

csharp
public class CartDbRepository : ICartRepository
{
    private readonly DatabaseContext _context;

    public Cart GetCart(string userId)
    {
        var cart = _context.Carts.Include(c => c.Items).ThenInclude(i => i.Product)
                   .FirstOrDefault(c => c.UserId == userId);
        if (cart == null)
        {
            cart = new Cart { UserId = userId };
            _context.Carts.Add(cart);
            _context.SaveChanges();
        }
        return cart;
    }
}
3. Миграция данных гостя при входе
Сервис GuestDataMigrator переносит корзину, избранное и сравнение из сессии гостя в постоянного пользователя.

csharp
public void MigrateGuestData(string userName)
{
    MigrateCart(guestId, userName);
    MigrateFavorite(guestId, userName);
    MigrateComparison(guestId, userName);
}
4. Интеграция с микросервисом отзывов
Клиент для HTTP-API микросервиса отзывов с автоматическим добавлением JWT-токена.

csharp
public async Task<ProductRatingDto> GetProductRatingAsync(int productId)
{
    await AddAuthorizationHeader();
    var response = await _httpClient.GetAsync($"/api/Product/{productId}/rating");
    if (response.IsSuccessStatusCode)
        return await response.Content.ReadFromJsonAsync<ProductRatingDto>();
    return new ProductRatingDto { Rating = 0, ReviewCount = 0 };
}
📸 Демонстрация
Главная страница каталога	Корзина покупателя	Админ-панель управления
https://github.com/user-attachments/assets/%D1%81%D1%81%D0%BB%D1%8B%D0%BA%D0%B0-%D0%BD%D0%B0-%D1%81%D0%BA%D1%80%D0%B8%D0%BD%D1%88%D0%BE%D1%82-1	https://github.com/user-attachments/assets/%D1%81%D1%81%D1%8B%D0%BB%D0%BA%D0%B0-%D0%BD%D0%B0-%D1%81%D0%BA%D1%80%D0%B8%D0%BD%D1%88%D0%BE%D1%82-2	https://github.com/user-attachments/assets/%D1%81%D1%81%D1%8B%D0%BB%D0%BA%D0%B0-%D0%BD%D0%B0-%D1%81%D0%BA%D1%80%D0%B8%D0%BD%D1%88%D0%BE%D1%82-3
(Замени ссылки на свои реальные скриншоты, загрузив их в Issues или создав папку /screenshots в репозитории.)

🚀 Запуск проекта
Клонировать репозиторий:

bash
git clone https://github.com/PaulPomogaev/OnlineShop.git
cd OnlineShop
Настроить строку подключения в OnlineShopWebApp/appsettings.json (по умолчанию — LocalDB).

Применить миграции для создания базы данных:

bash
cd OnlineShop.Db
dotnet ef database update --startup-project ../OnlineShopWebApp
(Если dotnet ef не установлен: dotnet tool install --global dotnet-ef)

Запустить микросервис отзывов (опционально, но рекомендуется для полной функциональности).

Запустить веб-приложение:

bash
cd ../OnlineShopWebApp
dotnet run
Открыть браузер по адресу https://localhost:5001.

🔑 Ключевые слова
ASP.NET Core | MVC | Entity Framework Core | CQRS | MediatR | Identity | SQL Server | Корзина | Избранное | Сравнение товаров | Админ-панель | Микросервисы | Clean Architecture | Репозиторий | Кеширование | Memcached | FluentValidation | Serilog | Bootstrap
