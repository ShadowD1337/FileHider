using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileHider.Data.Models
{
    public class HiddenMessage : HiddenInformation
    {
        public string Content { get; init; }

        public HiddenMessage(string content)
        {
            Content = content;
        }
    }
}
