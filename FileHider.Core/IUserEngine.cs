using FileHider.Data.Models;
using Microsoft.AspNetCore.Http;

namespace FileHider.Core
{
    public interface IUserEngine
    {
        string ExtractHiddenMessageFromImage(IFormFile image, string password);
        void HideMessageInImage(IFormFile image, string encryptionKey, string message);
        ImageFile[] UserImageFiles();
    }
}