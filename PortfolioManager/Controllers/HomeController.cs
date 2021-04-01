using System.Text;
using System.Web.Mvc;
using PortfolioManager.Repository;
using PortfolioManager.Services;

namespace PortfolioManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ClientDashboardService _clientDashboardService;
        private readonly IPortfolioRepository _portfolioRepository;

        public HomeController(IPortfolioRepository portfolioRepository, ClientDashboardService clientDashboardService)
        {
            _portfolioRepository = portfolioRepository;
            _clientDashboardService = clientDashboardService;
        }

        public ActionResult Index()
        {
            var clients = _portfolioRepository.GetAllClients();
            return View(clients);
        }

        public ActionResult Portfolio(int id)
        {
            var viewModel = _clientDashboardService.GetPortfolioPageViewModel(id);
            return View(viewModel);
        }

        public FileResult Export(int id)
        {
            var plReport = _clientDashboardService.GetProfitLossReport(id);

            var sb = new StringBuilder();
            sb.Append("Ticker,AsOfDate,Cost,Quantity,Price,MarketValue,Prev. Close,Daily P&L,Inception P&L\r\n");
            plReport.ForEach(report =>
            {
                sb.Append($"{report.Ticker},{report.AsOfDate},{report.Cost},{report.Quantity},{report.Price},{report.MarketValue},{report.PrevPrice},{report.DailyPandL},{report.InceptionPandL}\r\n");
            });

            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "ProftLossReport.csv");
        }
    }
}
