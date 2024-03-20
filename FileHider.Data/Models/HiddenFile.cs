using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileHider.Data.Models
{
    public class HiddenFile : HiddenInformation
    {
        public string DownloadLink { get => base.Content; init { base.Content = value; } }

        public HiddenFile(string downloadLink, int fileSize) : base(downloadLink)
        {
            Size = fileSize;
        }
    }
}
