using System.Web.Mvc;

namespace CloudX.DietPoints.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}