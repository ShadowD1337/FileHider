using StegoSharp;
using StegoSharp.Enums;
using System.Drawing;
using System.Text;

namespace Stego
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var image = new StegoImage("C:\\Users\\user\\Desktop\\old.jpg");
            image.Strategy.ColorChannels = new[] { ColorChannel.G, ColorChannel.B, ColorChannel.R };
            image.Strategy.BitsPerChannel = 2;

            var msg = "test123";
            image.EmbedPayload(msg);
            image.Save("C:\\Users\\user\\Desktop\\new.jpg");

            Console.WriteLine(Encoding.Default.GetString(image.ExtractBytes().ToArray()).Substring(0, msg.Length));
            Console.WriteLine(image.ByteCapacity);
        }
    }
}