using Microsoft.AspNetCore.Mvc;

namespace Videoteca.Controllers
{
    public class SuperAdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
