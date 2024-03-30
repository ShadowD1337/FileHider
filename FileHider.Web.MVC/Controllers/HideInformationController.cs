using FileHider.Core;
using FileHider.Data.Models;
using FileHider.Web.MVC.Models;
using FirebaseAdmin.Messaging;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using StegSharp.Application.Common.Interfaces;
using System.Diagnostics;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;


namespace FileHider.Web.MVC.Controllers
{
    public class HideInformationController : Controller
    {
        private readonly ILogger<HideInformationController> _logger;
        private static IUserEngine _userEngine;

        public HideInformationController(ILogger<HideInformationController> logger, IUserEngine userEngine)
        {
            _logger = logger;
            _userEngine = userEngine;

        }
        public IActionResult HideMessage()
        {
            return PartialView("_HideMessage");
        }

        public IActionResult ExtractMessage()
        {
            return PartialView("_ExtractMessage");
        }

        public IActionResult UserImages()
        {
            return PartialView("_UserImages");
        }

        [HttpPost]
        public IActionResult EmbedMessageInImage(IFormFile image, string encryptionKey, string message)
        {
            HideMessageInImage(image, encryptionKey, message);

            return Ok();
        }

        [HttpPost]
        public IActionResult ExtractMessageFromImage(IFormFile image, string encryptionKey)
        {
            string result = ExtractHiddenMessageFromImage(image, encryptionKey);

            return Ok(result);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult ImageFiles()
        {
            ImageFile[] result = _userEngine.UserImageFiles();
            return Ok(result);
        }

        public void HideMessageInImage(IFormFile image, string encryptionKey, string message)
        {
            _userEngine.HideMessageInImage(image, encryptionKey, message);
        }

        public string ExtractHiddenMessageFromImage(IFormFile image, string password)
        {
            string result = _userEngine.ExtractHiddenMessageFromImage(image, password);

            return result;
        }
    }
}
