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
    public class DashBoardController : Controller
    {
        private readonly ILogger<DashBoardController> _logger;

        public DashBoardController(ILogger<DashBoardController> logger)
        {
            _logger = logger;
        }

        public IActionResult DashBoard()
        {
            return View();
        }
        public IActionResult btn_hideInformationInImage()
        {
            return PartialView("_HideInformationInImage");
        }
        public IActionResult Button2()
        {
            return PartialView("_SecondButtonPartialView");
        }
        public IActionResult Button3()
        {
            return PartialView("_ThirdButtonPartialView");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
