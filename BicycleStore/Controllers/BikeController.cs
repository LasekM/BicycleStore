using Microsoft.AspNetCore.Mvc;

namespace RowerStore.Controllers
{
    public class BikeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RedirectToIndex()
        {
            return RedirectToAction("Index");
        }
    }
}
