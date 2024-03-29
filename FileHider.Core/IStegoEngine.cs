using FileHider.Data.Models;
using FileHider.Data.StegoOverwrite;

namespace FileHider.Core
{
    public interface IStegoEngine
    {
        byte[] ExtractBytesFromStegoImage(int byteCount, StegoImage stegoImage, ImageStegoStrategy imageStegoStrategy);
        string GenerateDownloadLink(byte[] fileBytes, string fileNameWithExt);
        string GenerateDownloadLink(StegoImage stegoImage, string imageNameWithExt);
        void HideFileInImage(byte[] fileBytes, string fileNameWithExt, StegoImage stegoImage, string imageNameWithExt, ImageStegoStrategy imageStegoStrategy);
        void HideMessageInImage(string content, StegoImage stegoImage, string imageNameWithExt, ImageStegoStrategy imageStegoStrategy);
    }
}