using FileHider.Data;
using FileHider.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StegoSharp;

namespace FileHider.Experiments
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string connectionString = "Server=localhost;Database=filehider;Uid=root;Pwd=root;";

            var optionsBuilder = new DbContextOptionsBuilder<UserDbContext>();
            optionsBuilder.LogTo(Console.WriteLine, minimumLevel: LogLevel.Information);
            optionsBuilder.UseMySQL(connectionString);

            using var dbContext = new UserDbContext(optionsBuilder.Options);
            dbContext.Database.EnsureCreated();

            var hiddenMessage = new HiddenMessage("test");
            dbContext.HiddenInformations.Add(hiddenMessage);
            dbContext.ImageFiles.Add(new ImageFile(1, "test", 1, 999));

            /*foreach (var imageFile in dbContext.ImageFiles)
            {
                imageFile.HiddenInformation = dbContext.HiddenInformations.Where(h => h.Id == imageFile.HiddenInformationId).First();
            }*/

            Console.WriteLine(dbContext.Users.Where(u => u.FirstName == "gosho").First().Id);

            dbContext.SaveChanges();
        }
    }
}
