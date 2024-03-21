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
        public HiddenInformation? HiddenInformation { get; private set; }
        [Column("total_byte_capacity")]
        public int TotalByteCapacity { get; init; }
        [Column("byte_capacity_left")]
        public int ByteCapacityLeft {
            get
            {
                int byteSize = HiddenInformation is not null ? HiddenInformation.Size : 0;
                return TotalByteCapacity - byteSize;
            }
        }
        [Column("user_id")]
        public string UserId { get; init; }
        public ImageFile(string userId, int stegoStrategyId, string downloadLink, int hiddenInformationId, int totalByteCapacity)
        {
            UserId = userId;
            StegoStrategyId = stegoStrategyId;
            DownloadLink = downloadLink;
            HiddenInformationId = hiddenInformationId;
            TotalByteCapacity = totalByteCapacity;
        }

        public void LoadHiddenInformation(UserDbContext dbContext)
        {
            this.HiddenInformation = dbContext.HiddenInformations.First(h => h.Id == this.Id);
        }
    }
}
