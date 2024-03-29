
namespace FileHider.Data
{
    public interface IFileUploader
    {
        Task<string> UploadFileAsync(byte[] fileBytes, string fileNameWithExt);
        Task<string> UploadImageAsync(byte[] image, string imageNameWithExt);
    }
}