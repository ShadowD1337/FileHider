using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using StegoSharp;
using System.Configuration;
using System.Net;

namespace FileHider.Data
{
    public class FileUploader
    {
        private readonly StorageClient _storageClient;
        private string _bucketName;
        public FileUploader((string filePath, string bucketName) options)
        {
            var credential = GoogleCredential.FromFile(options.filePath);
            FirebaseApp.Create(new AppOptions
            {
                Credential = credential
            });

            _storageClient = StorageClient.Create(credential);
            _bucketName = options.bucketName;
        }

        public async Task<string> UploadImageAsync(StegoImage stegoImage, string imageNameWithExt)
        {
            return await UploadFileAsync(stegoImage.ExtractBytes().ToArray(), imageNameWithExt);
        }

        public async Task<string> UploadFileAsync(byte[] fileBytes, string fileNameWithExt)
        {
            using (var mem = new MemoryStream(fileBytes))
            {
                var filePath = "Files/" + fileNameWithExt;

                await _storageClient.UploadObjectAsync(_bucketName, filePath, null, mem);

                var downloadUrl = $"https://firebasestorage.googleapis.com/v0/b/{_bucketName}/o/{Uri.EscapeDataString(filePath)}?alt=media";

                return downloadUrl;
            }
        }
    }
}
