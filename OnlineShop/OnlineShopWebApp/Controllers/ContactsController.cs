using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Interfaces;

namespace OnlineShopWebApp.Controllers
{
    public class ContactsController : Controller
    {
        
        public IActionResult Index()
        {
            
            return View();
        }
    }
}
