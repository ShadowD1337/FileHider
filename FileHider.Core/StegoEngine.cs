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
        private FileUploader _fileUploader;
        public StegoEngine(string userId, (string filePath, string bucketName) options)
        {
            _userId = userId;
            _fileUploader = new FileUploader(options);
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
            return _fileUploader.UploadImageAsync(stegoImage, imageNameWithExt).Result;
        }
        public string GenerateDownloadLink(byte[] fileBytes, string fileNameWithExt)
        {
            return _fileUploader.UploadFileAsync(fileBytes, fileNameWithExt).Result;
        }
    }
}
