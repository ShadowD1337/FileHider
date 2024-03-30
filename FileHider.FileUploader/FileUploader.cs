using FileHider.Web.MVC.Settings;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using static Google.Cloud.Storage.V1.UrlSigner;

namespace FileHider.Data
{
    public class FileUploader : IFileUploader
    {
        private readonly StorageClient _storageClient;
        private readonly GoogleFirebaseSettings _firebaseSettings;
        public FileUploader()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();

            _firebaseSettings = configuration.GetRequiredSection(GoogleFirebaseSettings.Section).Get<GoogleFirebaseSettings>();

            var credential = GoogleCredential.FromFile(_firebaseSettings.ServiceAccountFilePath);
            if (FirebaseApp.DefaultInstance is null)
            {
                FirebaseApp.Create(new AppOptions
                {
                    Credential = credential
                });
            }

            _storageClient = StorageClient.Create(credential);
        }

        public async Task<string> UploadImageAsync(byte[] image, string imageNameWithExt)
        {
            return await UploadFileAsync(image, imageNameWithExt);
        }

        public async Task<string> UploadFileAsync(byte[] fileBytes, string fileNameWithExt)
        {
            using (var mem = new MemoryStream(fileBytes))
            {
                var filePath = "Files/" + fileNameWithExt;

                await _storageClient.UploadObjectAsync(_firebaseSettings.BucketName, filePath, null, mem);

                var downloadUrl = $"https://firebasestorage.googleapis.com/v0/b/{_firebaseSettings.BucketName}/o/{Uri.EscapeDataString(filePath)}?alt=media";

                return downloadUrl;
            }
        }
    }
}
