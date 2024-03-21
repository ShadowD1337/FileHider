using FileHider.Data;
using FileHider.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StegoSharp;
using static Dropbox.Api.Files.SearchMatchType;

namespace FileHider.Core
{
    public class UserEngine
    {
        private string _userId;
        private string _dropBoxApiKey;
        private UserDbContext _dbContext {
            get
            {
                var optionsBuilder = new DbContextOptionsBuilder<UserDbContext>();
                //optionsBuilder.LogTo(Console.WriteLine, minimumLevel: LogLevel.Information);
                optionsBuilder.UseMySQL(_connectionString);

                return new UserDbContext(optionsBuilder.Options);
            }
        }
        private StegoEngine _stegoEngine;
        private string _connectionString;
        public UserEngine(string userId, string connectionString, string dropBoxApiKey)
        {
            _userId = userId;
            _connectionString = connectionString;
            _dropBoxApiKey = dropBoxApiKey;
            _stegoEngine = new StegoEngine(userId, _dropBoxApiKey);
        }

        public void HideMessageInImage(string content, StegoImage stegoImage, string imageNameWithExt, ImageStegoStrategy imageStegoStrategy)
        {
            _stegoEngine.HideMessageInImage(content, stegoImage, imageNameWithExt, imageStegoStrategy);

            using var dbContext = _dbContext;
            dbContext.ImageStegoStrategies.Add(imageStegoStrategy);

            var hiddenMessage = new HiddenMessage(content);
            dbContext.HiddenInformations.Add(hiddenMessage);

            string downloadLink = _stegoEngine.GenerateDownloadLink(stegoImage, imageNameWithExt);

            var imageFile = new ImageFile(_userId, imageStegoStrategy.Id, downloadLink, hiddenMessage.Id, Convert.ToInt32(stegoImage.ByteCapacity));
            dbContext.ImageFiles.Add(imageFile);

            dbContext.SaveChanges();
        }
        public void HideFileInImage(byte[] fileBytes, string fileNameWithExt, StegoImage stegoImage, string imageNameWithExt, ImageStegoStrategy imageStegoStrategy)
        {
            _stegoEngine.HideFileInImage(fileBytes, fileNameWithExt, stegoImage, imageNameWithExt, imageStegoStrategy);

            using var dbContext = _dbContext;
            dbContext.ImageStegoStrategies.Add(imageStegoStrategy);

            string downloadLink = _stegoEngine.GenerateDownloadLink(fileBytes, imageNameWithExt);

            var hiddenFile = new HiddenFile(downloadLink, fileBytes.Length);
            dbContext.HiddenInformations.Add(hiddenFile);

            var imageFile = new ImageFile(_userId, imageStegoStrategy.Id, downloadLink, hiddenFile.Id, Convert.ToInt32(fileBytes.Length));
            dbContext.ImageFiles.Add(imageFile);

            dbContext.SaveChanges();
        }
    }
}
