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
        [Column("download_link")]
        public string DownloadLink { get; init; }
        [Column("user_id")]
        public string UserId { get; init; }
        public ImageFile(string userId, string downloadLink)
        {
            UserId = userId;
            DownloadLink = downloadLink;
        }
    }
}
