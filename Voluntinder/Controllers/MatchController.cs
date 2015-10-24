using System.Web.Mvc;

namespace Voluntinder.Controllers
{
    [Authorize]
    public class MatchController : Controller
    {
        // GET: Match
        public ActionResult Index()
        {
            return View();
        }
    }
}