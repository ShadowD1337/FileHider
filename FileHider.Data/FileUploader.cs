using Dropbox.Api;
using Dropbox.Api.Files;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using StegoSharp;
using System.Configuration;

namespace FileHider.Data
{
    public class FileUploader
    {
        private DropboxClient _dropBoxClient;

        private string _dropBoxApiKey;
        public FileUploader(string dropBoxApiKey)
        {
            _dropBoxApiKey = dropBoxApiKey;
            _dropBoxClient = new DropboxClient(_dropBoxApiKey);
        }

        public Task<string> UploadImage(StegoImage stegoImage, string imageNameWithExt)
        {
            return UploadFile(stegoImage.ExtractBytes().ToArray(), imageNameWithExt);
        }
        public async Task<string> UploadFile(byte[] fileBytes, string fileNameWithExt)
        {
            using (var mem = new MemoryStream(fileBytes))
            {
                var filePath = "Files" + "/" + fileNameWithExt;
                var updated = await _dropBoxClient.Files.UploadAsync(
                    filePath,
                    WriteMode.Overwrite.Instance,
                    body: mem);
                var result = _dropBoxClient.Sharing.CreateSharedLinkWithSettingsAsync(filePath).Result;
                return result.Url;
            }
        }
    }
}
