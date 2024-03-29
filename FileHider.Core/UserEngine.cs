using FileHider.Data;
using FileHider.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StegoSharp;
using System.Text;

namespace FileHider.Core
{
    public class UserEngine : IUserEngine
    {
        private IdentityUser _user;
        private UserDbContext _dbContext;
        public ImageFile[] UserImageFiles
        {
            get
            {
                return _dbContext.ImageFiles.Where(i => i.UserId == _user.Id).ToArray();
            }
        }

        private IStegoEngine _stegoEngine;
        public UserEngine(IdentityUser user, IStegoEngine stegoEngine, UserDbContext dbContext)
        {
            _user = user;
            _stegoEngine = stegoEngine;
            _dbContext = dbContext;
        }

        public void HideMessageInImage(string content, FileHider.Data.StegoOverwrite.StegoImage stegoImage, ImageStegoStrategy imageStegoStrategy, string imageNameWithExt)
        {
            _stegoEngine.HideMessageInImage(content, stegoImage, imageNameWithExt, imageStegoStrategy);

            _dbContext.ImageStegoStrategies.Add(imageStegoStrategy);

            var hiddenMessage = new HiddenMessage(content);
            _dbContext.HiddenInformations.Add(hiddenMessage);

            // We have to save the DB changes so hiddenMessage can get it's autoassigned ID
            _dbContext.SaveChanges();

            string downloadLink = _stegoEngine.GenerateDownloadLink(stegoImage, imageNameWithExt);

            var imageFile = new ImageFile(_user.Id, imageStegoStrategy.Id, downloadLink, hiddenMessage.Id, Convert.ToInt32(stegoImage.ByteCapacity));
            imageFile.LoadHiddenInformation(_dbContext);
            _dbContext.ImageFiles.Add(imageFile);

            _dbContext.SaveChanges();
        }
        public void HideFileInImage(byte[] fileBytes, string fileNameWithExt, FileHider.Data.StegoOverwrite.StegoImage stegoImage, ImageStegoStrategy imageStegoStrategy, string imageNameWithExt)
        {
            _stegoEngine.HideFileInImage(fileBytes, fileNameWithExt, stegoImage, imageNameWithExt, imageStegoStrategy);

            _dbContext.ImageStegoStrategies.Add(imageStegoStrategy);

            string downloadLinkHiddenFile = _stegoEngine.GenerateDownloadLink(fileBytes, imageNameWithExt);

            var hiddenFile = new HiddenFile(downloadLinkHiddenFile, fileBytes.Length);
            _dbContext.HiddenInformations.Add(hiddenFile);

            // We have to save the DB changes so hiddenMessage can get it's autoassigned ID
            _dbContext.SaveChanges();

            string downloadLinkImage = _stegoEngine.GenerateDownloadLink(fileBytes, imageNameWithExt);

            var imageFile = new ImageFile(_user.Id, imageStegoStrategy.Id, downloadLinkImage, hiddenFile.Id, Convert.ToInt32(stegoImage.ByteCapacity));
            imageFile.LoadHiddenInformation(_dbContext);
            _dbContext.ImageFiles.Add(imageFile);

            _dbContext.SaveChanges();
        }

        public string ExtractHiddenMessageFromImage(int hiddenMessageLength, FileHider.Data.StegoOverwrite.StegoImage stegoImage, ImageStegoStrategy imageStegoStrategy)
        {
            return Encoding.Default.GetString(_stegoEngine.ExtractBytesFromStegoImage(hiddenMessageLength, stegoImage, imageStegoStrategy));
        }

        public string ExtractHiddenFileFromImage(int fileByteSize, string fileNameWithExt, FileHider.Data.StegoOverwrite.StegoImage stegoImage, ImageStegoStrategy imageStegoStrategy)
        {
            var fileBytes = _stegoEngine.ExtractBytesFromStegoImage(fileByteSize, stegoImage, imageStegoStrategy);
            return _stegoEngine.GenerateDownloadLink(fileBytes, fileNameWithExt);
        }
    }
}
