using Microsoft.AspNetCore.Mvc;

namespace LigaIsadoraSilva.Controllers
{
    public class FootballTeamsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
