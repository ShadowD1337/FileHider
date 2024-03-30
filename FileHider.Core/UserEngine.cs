using FileHider.Data;
using FileHider.Data.Models;
using FileHider.Data.StegoOverwrite;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SkiaSharp;
using StegoSharp;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace FileHider.Core
{
    public class UserEngine : IUserEngine
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private UserDbContext _dbContext;
        public ImageFile[] UserImageFiles()
        {
            return _dbContext.ImageFiles.Where(i => i.UserId == _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)).ToArray();
        }

        private IStegoEngine _stegoEngine;
        private IFileUploader _fileUploader;
        public UserEngine(IHttpContextAccessor httpContextAccessor, IStegoEngine stegoEngine, UserDbContext dbContext, IFileUploader fileUploader)
        {
            _httpContextAccessor = httpContextAccessor;
            _stegoEngine = stegoEngine;
            _dbContext = dbContext;
            _fileUploader = fileUploader;
        }

        public void HideMessageInImage(IFormFile image, string encryptionKey, string message)
        {
            Bitmap imageBitmap;
            using MemoryStream memoryStream = new MemoryStream();
            image.CopyTo(memoryStream);
            memoryStream.Position = 0;
            imageBitmap = new Bitmap(memoryStream);

            /*EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Compression, 0);
            ImageCodecInfo codecInfo = ImageCodecInfo.GetImageEncoders().FirstOrDefault(codec => codec.FormatID == imageBitmap.RawFormat.Guid);
            imageBitmap.Save("C:\\Users\\Shadow Dragon\\Desktop\\test69.jpg", codecInfo, encoderParams);*/

            _stegoEngine.HideMessageInImage(ref imageBitmap, encryptionKey, message);

            byte[] fileBytes;

            using MemoryStream memoryStream2 = new MemoryStream();
            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Compression, 0);
            ImageCodecInfo codecInfo = ImageCodecInfo.GetImageEncoders().FirstOrDefault(codec => codec.FormatID == imageBitmap.RawFormat.Guid);
            imageBitmap.Save(memoryStream2, codecInfo, encoderParams);

            fileBytes = memoryStream2.ToArray();

            string downloadLink = _fileUploader.UploadFileAsync(fileBytes, image.FileName).Result;

            var imageFile = new ImageFile(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier), downloadLink);
            _dbContext.ImageFiles.Add(imageFile);

            imageBitmap.Dispose();

            _dbContext.SaveChanges();
        }

        public string ExtractHiddenMessageFromImage(IFormFile image, string password)
        {
            Bitmap imageBitmap;
            MemoryStream memoryStream = new MemoryStream();
            image.CopyTo(memoryStream);
            imageBitmap = new Bitmap(memoryStream);

            string result = _stegoEngine.ExtractHiddenMessageFromImage(imageBitmap, password);

            memoryStream.Dispose();

            return result;
        }
    }
}
