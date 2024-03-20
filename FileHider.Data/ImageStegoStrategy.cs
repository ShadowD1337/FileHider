using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StegoSharp;
using StegoSharp.Enums;

namespace FileHider.Data
{
    public class ImageStegoStrategy
    {
        [Column("id")]
        public int Id { get; init; }
        [Column("color_channels")]
        public string ColorChannelsString { get; private set; }

        [NotMapped]
        public StegoSharp.Enums.ColorChannel[] ColorChannels
        {
            get => StringToColorChannels(ColorChannelsString);
            init => ColorChannelsString = string.Join(",", value.Select(c => Enum.GetName(typeof(ColorChannel), c)));
        }

        private int _bitsPerChannel;
        [Column("bits_per_channel")]
        public int BitsPerChannel { get => _bitsPerChannel; init => _bitsPerChannel = Math.Clamp(value, 1, 8); }
        [NotMapped]
        public Func<StegoPixel, bool> PixelSelection { get; init; }
        [Column("pixel_spacing")]
        public int PixelSpacing { get; init; }

        public ImageStegoStrategy(string colorChannelsString, int bitsPerChannel, int pixelSpacing)
        {
            ColorChannels = StringToColorChannels(colorChannelsString);
            BitsPerChannel = bitsPerChannel;
            pixelSpacing = Math.Max(1, pixelSpacing);
            PixelSelection = p => p.Index % pixelSpacing == 0;
            PixelSpacing = pixelSpacing;
        }
        public ImageStegoStrategy(StegoSharp.Enums.ColorChannel[] colorChannels, int bitsPerChannel, int pixelSpacing)
        {
            ColorChannels = colorChannels;
            BitsPerChannel = bitsPerChannel;
            pixelSpacing = Math.Max(1, pixelSpacing);
            PixelSelection = p => p.Index % pixelSpacing == 0;
            PixelSpacing = pixelSpacing;
        }

        private StegoSharp.Enums.ColorChannel[] StringToColorChannels(string colorChannelsString)
        {
            string[] channelStrings = colorChannelsString.Split(',');
            var colorChannels = new StegoSharp.Enums.ColorChannel[channelStrings.Length];
            for (int i = 0; i < channelStrings.Length; i++)
            {
                if (Enum.TryParse(channelStrings[i], out ColorChannel channel))
                {
                    colorChannels[i] = (StegoSharp.Enums.ColorChannel)channel;
                }
            }
            return colorChannels;
        }
    }
    [Flags]
    public enum ColorChannel
    {
        Red = StegoSharp.Enums.ColorChannel.R,
        Green = StegoSharp.Enums.ColorChannel.G,
        Blue = StegoSharp.Enums.ColorChannel.B,
        Alpha = StegoSharp.Enums.ColorChannel.A
    }
}
