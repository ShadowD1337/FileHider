using FileHider.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileHider.Data
{
    public class UserDbContext : DbContext
    {
        public DbSet<HiddenInformation> HiddenInformations { get; set; }
        public DbSet<ImageFile> ImageFiles { get; set; }
        public DbSet<ImageStegoStrategy> ImageStegoStrategies { get; set; }
        public UserDbContext()
        {
        }
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }
    }
}
