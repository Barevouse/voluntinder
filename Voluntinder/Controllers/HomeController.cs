using System.Web.Mvc;

namespace Voluntinder.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Match");
            }
            return View();
        }
    }
}