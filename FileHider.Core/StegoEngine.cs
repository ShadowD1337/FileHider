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
    public class StegoEngine : IStegoEngine
    {
        private IFileUploader _fileUploader;
        public StegoEngine(IFileUploader fileUploader)
        {
            _fileUploader = fileUploader;
        }

        public void HideMessageInImage(string content, Data.StegoOverwrite.StegoImage stegoImage, string imageNameWithExt, ImageStegoStrategy imageStegoStrategy)
        {
            stegoImage.Strategy = imageStegoStrategy.AsStegoStrategy;

            stegoImage.EmbedPayload(content);
        }
        public void HideFileInImage(byte[] fileBytes, string fileNameWithExt, Data.StegoOverwrite.StegoImage stegoImage, string imageNameWithExt, ImageStegoStrategy imageStegoStrategy)
        {
            stegoImage.Strategy = imageStegoStrategy.AsStegoStrategy;

            stegoImage.EmbedPayload(fileBytes);
        }

        public byte[] ExtractBytesFromStegoImage(int byteCount, Data.StegoOverwrite.StegoImage stegoImage, ImageStegoStrategy imageStegoStrategy)
        {
            return stegoImage.ExtractBytes().ToArray().Take(byteCount).ToArray();
        }

        public string GenerateDownloadLink(Data.StegoOverwrite.StegoImage stegoImage, string imageNameWithExt)
        {
            return _fileUploader.UploadImageAsync(stegoImage.AsByteArray(), imageNameWithExt).Result;
        }
        public string GenerateDownloadLink(byte[] fileBytes, string fileNameWithExt)
        {
            return _fileUploader.UploadFileAsync(fileBytes, fileNameWithExt).Result;
        }
    }
}
