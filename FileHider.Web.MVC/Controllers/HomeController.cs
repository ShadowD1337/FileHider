using FileHider.Core;
using FileHider.Data;
using FileHider.Data.Models;
using FileHider.Web.MVC.Models;
using FileHider.Web.MVC.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StegoSharp;
using System.Diagnostics;
using System.Drawing;
using System.Security.Claims;


namespace FileHider.Web.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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
