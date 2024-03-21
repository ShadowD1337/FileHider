using Dropbox.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileHider.Data
{
    public class FileUploader
    {
        private DropboxClient _dropBoxClient;

        public FileUploader()
        {
            _dropBoxClient = new DropboxClient("eyk9ols0ji46qw0");
        }
    }
}
