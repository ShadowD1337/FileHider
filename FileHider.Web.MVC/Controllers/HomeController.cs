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
        private readonly IConfiguration _configuration;
        private readonly IOptions<DropBoxSettings> _dropBoxSettings;
        private string _connectionString => _configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException("No given connection string.");
        private string _dropBoxApiKey => _dropBoxSettings.Value.ApiKey;
        private UserEngine userEngine;

        public HomeController(ILogger<HomeController> logger, IOptions<DropBoxSettings> options, IConfiguration config)
        {
            _logger = logger;
            _configuration = config;
            _dropBoxSettings = options;
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

        public void InitializeUserEngine()
        {
            this.userEngine = new UserEngine(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new ArgumentNullException("Not signed in an user profile."), _connectionString, _dropBoxApiKey);
        }
        public void InitializeUserEngine(string userId)
        {
            this.userEngine = new UserEngine(userId, _connectionString, _dropBoxApiKey);
        }

        public void HideInImage(string content, StegoImage stegoImage, string imageNameWithExt, ImageStegoStrategy imageStegoStrategy)
        {
            if (userEngine is null) throw new ArgumentNullException("Not signed in an user profile.");

            userEngine.HideMessageInImage(content, stegoImage, imageNameWithExt, imageStegoStrategy);
        }
        public void HideInImage(byte[] fileBytes, string fileNameWithExt, StegoImage stegoImage, string imageNameWithExt, ImageStegoStrategy imageStegoStrategy)
        {
            if (userEngine is null) throw new ArgumentNullException("Not signed in an user profile.");
            
            userEngine.HideFileInImage(fileBytes, fileNameWithExt, stegoImage, imageNameWithExt, imageStegoStrategy);
        }
    }
}
