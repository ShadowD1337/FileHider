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
        private readonly IOptions<GoogleFirebaseSettings> _firebaseOptions;
        private (string filePath, string bucketName) _options => (_firebaseOptions.Value.ServiceAccountFilePath, _firebaseOptions.Value.BucketName);
        private string _connectionString => _configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException("No given connection string.");
        private UserEngine _userEngine;
        public List<ImageFile> UserImageFiles {
            get
            {
                return _userEngine.UserImageFiles;
            }
        }

        public HomeController(ILogger<HomeController> logger, IOptions<GoogleFirebaseSettings> options, IConfiguration config)
        {
            _logger = logger;
            _configuration = config;
            _firebaseOptions = options;
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
            this._userEngine = new UserEngine(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) 
                ?? throw new ArgumentNullException("Not signed in an user profile.")
                , _connectionString, _options);
        }
        public void InitializeUserEngine(string userId)
        {
            this._userEngine = new UserEngine(userId, _connectionString, _options);
        }

        public void HideMessageInImage(string content, StegoImage stegoImage, string imageNameWithExt, int pixelSpacing)
        {
            if (_userEngine is null) throw new ArgumentNullException("Not signed in an user profile.");

            _userEngine.HideMessageInImage(content, stegoImage, imageNameWithExt, pixelSpacing);
        }
        public void HideFileInImage(byte[] fileBytes, string fileNameWithExt, StegoImage stegoImage, string imageNameWithExt, int pixelSpacing)
        {
            if (_userEngine is null) throw new ArgumentNullException("Not signed in an user profile.");
            
            _userEngine.HideFileInImage(fileBytes, fileNameWithExt, stegoImage, imageNameWithExt, pixelSpacing);
        }
    }
}
