using FileHider.Core;
using FileHider.Data.Models;
using FileHider.Web.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Drawing;


namespace FileHider.Web.MVC.Controllers
{
    public class HideInformationController : Controller
    {
        private readonly ILogger<HideInformationController> _logger;
        private static IUserEngine _userEngine;
        public ImageFile[] UserImageFiles {
            get
            {
                return _userEngine.UserImageFiles;
            }
        }

        public HideInformationController(ILogger<HideInformationController> logger, IUserEngine userEngine)
        {
            _logger = logger;
            _userEngine = userEngine;
        }
        public IActionResult HideMessage()
        {
            return PartialView("_HideMessage");
        }

        public IActionResult HideFile()
        {
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

            HideMessageInImage(message, stegoImage, imageStegoStrategy, file.FileName);

            return Ok();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public void HideMessageInImage(string content, FileHider.Data.StegoOverwrite.StegoImage stegoImage, ImageStegoStrategy imageStegoStrategy, string imageNameWithExt)
        {
            if (_userEngine is null) throw new ArgumentNullException("Not signed in an user profile.");

            _userEngine.HideMessageInImage(content, stegoImage, imageStegoStrategy, imageNameWithExt);
        }
        public void HideFileInImage(byte[] fileBytes, string fileNameWithExt, FileHider.Data.StegoOverwrite.StegoImage stegoImage, ImageStegoStrategy imageStegoStrategy, string imageNameWithExt)
        {
            if (_userEngine is null) throw new ArgumentNullException("Not signed in an user profile.");
            
            _userEngine.HideFileInImage(fileBytes, fileNameWithExt, stegoImage, imageStegoStrategy, imageNameWithExt);
        }

        public string ExtractHiddenMessageFromImage(int hiddenMessageLength, FileHider.Data.StegoOverwrite.StegoImage stegoImage, ImageStegoStrategy imageStegoStrategy)
        {
            if (_userEngine is null) throw new ArgumentNullException("Not signed in an user profile.");

            return _userEngine.ExtractHiddenMessageFromImage(hiddenMessageLength, stegoImage, imageStegoStrategy);
        }
        ///<summary>
        ///Returns a download link for the file.
        ///</summary>
        public string ExtractHiddenFileFromImage(int fileByteSize, string fileNameWithExt, FileHider.Data.StegoOverwrite.StegoImage stegoImage, ImageStegoStrategy imageStegoStrategy)
        {
            if (_userEngine is null) throw new ArgumentNullException("Not signed in an user profile.");

            return _userEngine.ExtractHiddenFileFromImage(fileByteSize, fileNameWithExt, stegoImage, imageStegoStrategy);
        }
    }
}
