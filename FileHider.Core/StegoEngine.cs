using FileHider.Data;
using FileHider.Data.Models;
using StegoSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileHider.Core
{
    public class StegoEngine
    {
        private string _userId;
        private string _dropBoxApiKey;
        private FileUploader _fileUploader;
        public StegoEngine(string userId, string dropBoxApiKey)
        {
            _userId = userId;
            _dropBoxApiKey = dropBoxApiKey;
            _fileUploader = new FileUploader(dropBoxApiKey);
        }

        public void HideMessageInImage(string content, StegoImage stegoImage, string imageNameWithExt, ImageStegoStrategy imageStegoStrategy)
        {
            stegoImage.Strategy = imageStegoStrategy.AsStegoStrategy;

            stegoImage.EmbedPayload(content);
        }
        public void HideFileInImage(byte[] fileBytes, string fileNameWithExt, StegoImage stegoImage, string imageNameWithExt, ImageStegoStrategy imageStegoStrategy)
        {
            stegoImage.Strategy = imageStegoStrategy.AsStegoStrategy;

            stegoImage.EmbedPayload(fileBytes);
        }

        public string GenerateDownloadLink(StegoImage stegoImage, string imageNameWithExt)
        {
            return _fileUploader.UploadImage(stegoImage, imageNameWithExt).Result;
        }
        public string GenerateDownloadLink(byte[] fileBytes, string fileNameWithExt)
        {
            return _fileUploader.UploadFile(fileBytes, fileNameWithExt).Result;
        }
    }
}
