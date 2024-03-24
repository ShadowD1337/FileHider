using FileHider.Core;
using FileHider.Data;
using FileHider.Data.Models;
using FileHider.Web.MVC.Models;
using FileHider.Web.MVC.Settings;
using FileHider.Data.StegoOverwrite;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StegoSharp;
using System.Diagnostics;
using System.Drawing;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System.Security.Principal;
using Microsoft.EntityFrameworkCore;


namespace FileHider.Web.MVC.Controllers
{
    public class HideInformationController : Controller
    {
        private readonly ILogger<HideInformationController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IOptions<GoogleFirebaseSettings> _firebaseOptions;
        private (string filePath, string bucketName) _options => (_firebaseOptions.Value.ServiceAccountFilePath, _firebaseOptions.Value.BucketName);
        private string _connectionString => _configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException("No given connection string.");
        private static UserEngine _userEngine;
        public List<ImageFile> UserImageFiles {
            get
            {
                return _userEngine.UserImageFiles;
            }
        }

        public HideInformationController(ILogger<HideInformationController> logger, IOptions<GoogleFirebaseSettings> options, IConfiguration config)
        {
            _logger = logger;
            _configuration = config;
            _firebaseOptions = options;
        }
        public IActionResult HideMessage()
        {
            if (_userEngine is null) InitializeUserEngine(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            return PartialView("_HideMessage");
        }

        public IActionResult HideFile()
        {
            if (_userEngine is null) InitializeUserEngine(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            return PartialView("_HideFile");
        }

        [HttpPost]
        public IActionResult EmbedMessageInImage(string colorChannels, int bitsPerChannel, int pixelSpacing, string message, IFormFile file)
        {
            Console.WriteLine(colorChannels);
            Bitmap bitmap;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                bitmap = new Bitmap(memoryStream);
            }

            var stegoImage = new FileHider.Data.StegoOverwrite.StegoImage(bitmap);
            var imageStegoStrategy = new ImageStegoStrategy(colorChannels, bitsPerChannel, pixelSpacing);
            stegoImage.Strategy = imageStegoStrategy.AsStegoStrategy;

            HideMessageInImage(message, stegoImage, file.FileName, pixelSpacing);

            return Ok();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public void InitializeUserEngine()
        {

            //this._userEngine = new UserEngine(userId
            //    ?? throw new ArgumentNullException("Not signed in an user profile.")
            //    , _connectionString, _options);
        }
        public void InitializeUserEngine(string userId)
        {
            _userEngine = new UserEngine(userId, _connectionString, _options);
        }

        public void HideMessageInImage(string content, FileHider.Data.StegoOverwrite.StegoImage stegoImage, string imageNameWithExt, int pixelSpacing)
        {
            if (_userEngine is null) throw new ArgumentNullException("Not signed in an user profile.");

            _userEngine.HideMessageInImage(content, stegoImage, imageNameWithExt, pixelSpacing);
        }
        public void HideFileInImage(byte[] fileBytes, string fileNameWithExt, FileHider.Data.StegoOverwrite.StegoImage stegoImage, string imageNameWithExt, int pixelSpacing)
        {
            if (_userEngine is null) throw new ArgumentNullException("Not signed in an user profile.");
            
            _userEngine.HideFileInImage(fileBytes, fileNameWithExt, stegoImage, imageNameWithExt, pixelSpacing);
        }

        public string ExtractHiddenMessageFromImage(int hiddenMessageLength, FileHider.Data.StegoOverwrite.StegoImage stegoImage)
        {
            if (_userEngine is null) throw new ArgumentNullException("Not signed in an user profile.");

            return _userEngine.ExtractHiddenMessageFromImage(hiddenMessageLength, stegoImage);
        }
        ///<summary>
        ///Returns a download link for the file.
        ///</summary>
        public string ExtractHiddenFileFromImage(int fileByteSize, string fileNameWithExt, FileHider.Data.StegoOverwrite.StegoImage stegoImage)
        {
            if (_userEngine is null) throw new ArgumentNullException("Not signed in an user profile.");

            return _userEngine.ExtractHiddenFileFromImage(fileByteSize, fileNameWithExt, stegoImage);
        }
    }
}
