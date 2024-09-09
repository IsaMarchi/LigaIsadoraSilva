using Microsoft.AspNetCore.Mvc;

namespace LigaIsadoraSilva.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
