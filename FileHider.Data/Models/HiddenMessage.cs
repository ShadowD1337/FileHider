using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileHider.Data.Models
{
    public class HiddenMessage : HiddenInformation
    {

        public HiddenMessage(string content) : base(content)
        {
            base.Size = Encoding.ASCII.GetByteCount(content);
        }
    }
}
