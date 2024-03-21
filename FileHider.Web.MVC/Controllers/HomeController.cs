using FileHider.Core;
using FileHider.Data;
using FileHider.Data.Models;
using FileHider.Web.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using StegoSharp;
using System.Diagnostics;
using System.Drawing;
using System.Security.Claims;


namespace FileHider.Web.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private UserEngine userEngine;

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

        public void InitializeUserEngine(string connectionString)
        {
            this.userEngine = new UserEngine(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new ArgumentNullException("Not signed in an user profile."), connectionString);
        }
        public void InitializeUserEngine(string connectionString, string userId)
        {
            this.userEngine = new UserEngine(userId, connectionString);
        }

        public void HideInImage(string content, StegoImage stegoImage, ImageStegoStrategy imageStegoStrategy)
        {
            if (userEngine is null) throw new ArgumentNullException("Not signed in an user profile.");

            userEngine.HideMessageInImage(content, stegoImage, imageStegoStrategy);
        }
        public void HideInImage(byte[] fileBytes, StegoImage stegoImage, ImageStegoStrategy imageStegoStrategy)
        {
            if (userEngine is null) throw new ArgumentNullException("Not signed in an user profile.");
            
            userEngine.HideFileInImage(fileBytes, stegoImage, imageStegoStrategy);
        }
    }
}
