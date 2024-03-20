using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileHider.Data.Models
{
    public class HiddenFile : HiddenInformation
    {
        public string DownloadLink { get; init; }

        public HiddenFile(string downloadLink)
        {
            DownloadLink = downloadLink;
        }
    }
}
