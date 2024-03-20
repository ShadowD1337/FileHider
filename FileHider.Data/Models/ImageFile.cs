using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace FileHider.Data.Models
{
    public class ImageFile
    {
        [Column("id")]
        public int Id { get; init; }
        [Column("stego_strat_id")]
        public int StegoStrategyId { get; init; }
        [Column("download_link")]
        public string DownloadLink { get; init; }
        [Column("hidden_info_id")]
        public int HiddenInformationId { get; set; }
        [NotMapped]
        public HiddenInformation HiddenInformation { get; set; }
        [Column("total_byte_capacity")]
        public int TotalByteCapacity { get; init; }
        [Column("byte_capacity_left")]
        public int ByteCapacityLeft {
            get
            {
                return TotalByteCapacity - HiddenInformation.Size;
            }
        }
        public ImageFile(int stegoStrategyId, string downloadLink, int hiddenInformationId, int totalByteCapacity)
        {
            StegoStrategyId = stegoStrategyId;
            DownloadLink = downloadLink;
            HiddenInformationId = hiddenInformationId;
            TotalByteCapacity = totalByteCapacity;
        }
    }
}
