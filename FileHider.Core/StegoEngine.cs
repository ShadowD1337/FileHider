using FileHider.Data;
using FileHider.Data.Models;
using Microsoft.Extensions.DependencyInjection;
using StegoSharp;
using StegSharp.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;
using System.Drawing.Imaging;
using System.IO;
using FirebaseAdmin.Messaging;
using StegSharp.Infrastructure.Services;
using Org.BouncyCastle.Utilities.IO;

namespace FileHider.Core
{
    public class StegoEngine : IStegoEngine
    {
        private IF5Service _stegoService;
        public StegoEngine(IServiceProvider serviceProvider)
        {
            _stegoService = serviceProvider.GetService<IF5Service>();
        }

        public void HideMessageInImage(ref Bitmap image, string password, string message)
        {
            var skBitmap = new SKBitmap();

            using (var stream = new MemoryStream())
            {
                image.Save(stream, image.RawFormat);

                stream.Seek(0, SeekOrigin.Begin);

                skBitmap = SKBitmap.Decode(stream);
            }

            MemoryStream memoryStream = new MemoryStream();
            image.Save(memoryStream, image.RawFormat);
            byte[] bitmapBytes = memoryStream.ToArray();

            BinaryWriter binaryWriter = new BinaryWriter(memoryStream, Encoding.ASCII, true);

            _stegoService.Embed(skBitmap, password, message, binaryWriter);

            binaryWriter.Flush();

            memoryStream.Position = 0;

            Bitmap modifiedBitmap = new Bitmap(memoryStream);

            image.Dispose();
            image = modifiedBitmap;

            //memoryStream.Dispose();
        }

        public string ExtractHiddenMessageFromImage(Bitmap image, string password)
        {
            MemoryStream memoryStream = new MemoryStream();
            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Compression, 0);
            ImageCodecInfo codecInfo = ImageCodecInfo.GetImageEncoders().FirstOrDefault(codec => codec.FormatID == image.RawFormat.Guid);
            image.Save(memoryStream, codecInfo, encoderParams);
            memoryStream.Position = 0;
            BinaryReader binaryReader = new BinaryReader(memoryStream);
            byte[] data = binaryReader.ReadBytes((int)memoryStream.Length);
            memoryStream.Position = 0;
            
            string result = _stegoService.Extract(password, binaryReader);

            memoryStream.Dispose();
            binaryReader.Dispose();

            return result;
        }
    }
}

