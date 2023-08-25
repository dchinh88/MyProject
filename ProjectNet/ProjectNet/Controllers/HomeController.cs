using Microsoft.AspNetCore.Mvc;
using ProjectNet.Models;
using System.Diagnostics;

namespace ProjectNet.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly QLNoiThatDBContext _context;

        public HomeController(QLNoiThatDBContext context, ILogger<HomeController> logger)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}