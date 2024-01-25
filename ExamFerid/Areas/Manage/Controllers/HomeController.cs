using Microsoft.AspNetCore.Mvc;

namespace ExamFerid.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
