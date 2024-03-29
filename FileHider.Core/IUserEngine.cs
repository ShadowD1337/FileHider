using FileHider.Data.Models;
using FileHider.Data.StegoOverwrite;

namespace FileHider.Core
{
    public interface IUserEngine
    {
        ImageFile[] UserImageFiles { get; }

        string ExtractHiddenFileFromImage(int fileByteSize, string fileNameWithExt, StegoImage stegoImage, ImageStegoStrategy imageStegoStrategy);
        string ExtractHiddenMessageFromImage(int hiddenMessageLength, StegoImage stegoImage, ImageStegoStrategy imageStegoStrategy);
        void HideFileInImage(byte[] fileBytes, string fileNameWithExt, StegoImage stegoImage, ImageStegoStrategy imageStegoStrategy, string imageNameWithExt);
        void HideMessageInImage(string content, StegoImage stegoImage, ImageStegoStrategy imageStegoStrategy, string imageNameWithExt);
    }
}