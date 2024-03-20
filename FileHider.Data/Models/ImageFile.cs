using StegoSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace FileHider.Data.Models
{
    public class ImageFile
    {
        public StegoStrategy StegoStrategy { get; init; }
        public string DownloadLink { get; init; }
        public HiddenInformation HiddenInformation { get; init; }
        public int TotalByteCapacity { get; init; }
        public int ByteCapacityLeft {
            get
            {
                return TotalByteCapacity - HiddenInformation.Size;
            }
        }
        public ImageFile(StegoStrategy stegoStrategy, string downloadLink, HiddenInformation hiddenInformation, int totalCapacity)
        {
            StegoStrategy = stegoStrategy;
            DownloadLink = downloadLink;
            HiddenInformation = hiddenInformation;
            TotalByteCapacity = totalCapacity;
        }
    }
}
