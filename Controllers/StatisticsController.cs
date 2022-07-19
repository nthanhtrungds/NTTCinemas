using Microsoft.AspNetCore.Mvc;

namespace NTTCinemas.Controllers
{
    public class StatisticsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
