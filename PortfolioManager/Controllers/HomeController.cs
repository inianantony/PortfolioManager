using System.Web.Mvc;

namespace PortfolioManager.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Portfolio()
        {
            return View();
        }
    }
}
