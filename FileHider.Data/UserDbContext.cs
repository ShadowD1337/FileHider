using FileHider.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileHider.Data
{
    public class UserDbContext : IdentityDbContext
    {
        public DbSet<ImageFile> ImageFiles { get; set; }
        public UserDbContext()
        {
        }
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }
    }
}
