using FileHider.Data;
using FileHider.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StegoSharp;

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
        private StegoEngine _stegoEngine;
        private string _connectionString;
        public UserEngine(string userId, string connectionString)
        {
            _userId = userId;
            _connectionString = connectionString;
            _stegoEngine = new StegoEngine(userId);
        }

        public void HideMessageInImage(string content, StegoImage stegoImage, ImageStegoStrategy imageStegoStrategy)
        {
            _stegoEngine.HideMessageInImage(content, stegoImage, imageStegoStrategy);

            var dbContext = _dbContext;

            dbContext.ImageStegoStrategies.Add(imageStegoStrategy);

            var hiddenInformation = new HiddenInformation(content);
            dbContext.HiddenInformations.Add(hiddenInformation);
            string downloadLink = "";

            var imageFile = new ImageFile(_userId, imageStegoStrategy.Id, downloadLink, hiddenInformation.Id, Convert.ToInt32(stegoImage.ByteCapacity));

            dbContext.ImageFiles.Add(imageFile);
        }
        public void HideFileInImage(byte[] fileBytes, StegoImage stegoImage, ImageStegoStrategy imageStegoStrategy)
        {

        }
    }
}
