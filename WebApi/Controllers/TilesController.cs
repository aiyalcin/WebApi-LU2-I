using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class TilesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
