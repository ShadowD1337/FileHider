using StegoSharp;
using StegoSharp.Enums;
using System.Drawing;
using System.Text;
using FileHider.Data.Models;

namespace Stego
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var image = new FileHider.Data.StegoOverwrite.StegoImage(new Bitmap("C:\\Users\\Shadow Dragon\\Desktop\\rly34.png"));
            var imageStegoStrategy = new ImageStegoStrategy("Red,Green,Blue", 2, 2);
            image.Strategy = imageStegoStrategy.AsStegoStrategy;

            var msg = "test857";
            //image.EmbedPayload(msg);
            //image.Save("C:\\Users\\Shadow Dragon\\Desktop\\rly2.png");

            Console.WriteLine(Encoding.Default.GetString(image.ExtractBytes().ToArray()).Substring(0, msg.Length));
            //Console.WriteLine(Encoding.Default.GetString(image.AsByteArray()).Substring(0, msg.Length));
            // Console.WriteLine(image.ByteCapacity);

            //image._image = new Bitmap("C:\\Users\\Shadow Dragon\\Desktop\\Files_rly.png");
            //image.Strategy = imageStegoStrategy.AsStegoStrategy;
            //Console.WriteLine(Encoding.ASCII.GetString(image.ExtractBytes().ToArray()).Substring(0, msg.Length));
        }
    }
}