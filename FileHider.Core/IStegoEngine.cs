using System.Drawing;

namespace FileHider.Core
{
    public interface IStegoEngine
    {
        string ExtractHiddenMessageFromImage(Bitmap bitmap, string password);
        void HideMessageInImage(ref Bitmap image, string password, string message);
    }
}