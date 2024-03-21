using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileHider.Data.Models
{
    public class HiddenInformation
    {
        [Column("id")]
        public int Id { get; init; }
        [Column("size")]
        public int Size { get; init; }
        [Column("content")]
        public string Content { get; init; }
        public HiddenInformation(string content)
        {
            Content = content;
        }
    }
}
