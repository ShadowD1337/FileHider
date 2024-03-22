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
        private UserDbContext _dbContext {
            get
            {
                var optionsBuilder = new DbContextOptionsBuilder<UserDbContext>();
                //optionsBuilder.LogTo(Console.WriteLine, minimumLevel: LogLevel.Information);
                optionsBuilder.UseMySQL(_connectionString);

                return new UserDbContext(optionsBuilder.Options);
            }
        }

        public List<ImageFile> UserImageFiles
        {
            get
            {
                var dbContext = _dbContext;
                return dbContext.ImageFiles.Where(i => i.UserId == _userId).ToList();
            }
        }

        private StegoEngine _stegoEngine;
        private string _connectionString;
        public UserEngine(string userId, string connectionString, (string filePath, string bucketName) options)
        {
            _userId = userId;
            _connectionString = connectionString;
            _stegoEngine = new StegoEngine(userId, options);
        }

        public void HideMessageInImage(string content, StegoImage stegoImage, string imageNameWithExt, int pixelSpacing)
        {
            var imageStegoStrategy = new ImageStegoStrategy(stegoImage.Strategy, pixelSpacing);
            _stegoEngine.HideMessageInImage(content, stegoImage, imageNameWithExt, imageStegoStrategy);

            using var dbContext = _dbContext;
            dbContext.ImageStegoStrategies.Add(imageStegoStrategy);

            var hiddenMessage = new HiddenMessage(content);
            dbContext.HiddenInformations.Add(hiddenMessage);

            // We have to save the DB changes so hiddenMessage can get it's autoassigned ID
            dbContext.SaveChanges();

            string downloadLink = _stegoEngine.GenerateDownloadLink(stegoImage, imageNameWithExt);

            var imageFile = new ImageFile(_userId, imageStegoStrategy.Id, downloadLink, hiddenMessage.Id, Convert.ToInt32(stegoImage.ByteCapacity));
            imageFile.LoadHiddenInformation(dbContext);
            dbContext.ImageFiles.Add(imageFile);

            dbContext.SaveChanges();
        }
        public void HideFileInImage(byte[] fileBytes, string fileNameWithExt, StegoImage stegoImage, string imageNameWithExt, int pixelSpacing)
        {
            var imageStegoStrategy = new ImageStegoStrategy(stegoImage.Strategy, pixelSpacing);
            _stegoEngine.HideFileInImage(fileBytes, fileNameWithExt, stegoImage, imageNameWithExt, imageStegoStrategy);

            using var dbContext = _dbContext;
            dbContext.ImageStegoStrategies.Add(imageStegoStrategy);

            string downloadLinkHiddenFile = _stegoEngine.GenerateDownloadLink(fileBytes, imageNameWithExt);

            var hiddenFile = new HiddenFile(downloadLinkHiddenFile, fileBytes.Length);
            dbContext.HiddenInformations.Add(hiddenFile);

            // We have to save the DB changes so hiddenMessage can get it's autoassigned ID
            dbContext.SaveChanges();

            string downloadLinkImage = _stegoEngine.GenerateDownloadLink(fileBytes, imageNameWithExt);

            var imageFile = new ImageFile(_userId, imageStegoStrategy.Id, downloadLinkImage, hiddenFile.Id, Convert.ToInt32(stegoImage.ByteCapacity));
            imageFile.LoadHiddenInformation(dbContext);
            dbContext.ImageFiles.Add(imageFile);

            dbContext.SaveChanges();
        }
    }
}
