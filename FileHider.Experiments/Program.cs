using FileHider.Data;
using FileHider.Data.Models;
using FileHider.Web.MVC.Controllers;
using FileHider.Web.MVC.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StegoSharp;
using StegoSharp.Enums;

namespace FileHider.Experiments
{
    internal class Program
    {
        private static ILogger<HomeController> logger;
        static void Main(string[] args)
        {
            const string connectionString = "Server=localhost;Database=filehider;Uid=root;Pwd=root;";

            /*var imageStegoStrategy = new ImageStegoStrategy(new[] { StegoSharp.Enums.ColorChannel.G, StegoSharp.Enums.ColorChannel.R }, 2, 1);

            Console.WriteLine(imageStegoStrategy.ColorChannelsString);*/

            /*
            var optionsBuilder = new DbContextOptionsBuilder<UserDbContext>();
            optionsBuilder.LogTo(Console.WriteLine, minimumLevel: LogLevel.Information);
            optionsBuilder.UseMySQL(connectionString);

            using var dbContext = new UserDbContext(optionsBuilder.Options);
            dbContext.Database.EnsureCreated();

            var hiddenMessage = new HiddenMessage("test");
            dbContext.HiddenInformations.Add(hiddenMessage);
            dbContext.ImageStegoStrategies.Add(new ImageStegoStrategy("Red,Green", 2, 1));
            dbContext.ImageFiles.Add(new ImageFile(1, "test", 1, 999));

            foreach (var imageFile in dbContext.ImageFiles)
            {
                imageFile.HiddenInformation = dbContext.HiddenInformations.Where(h => h.Id == imageFile.HiddenInformationId).First();
            }

            //Console.WriteLine(dbContext.Users.Where(u => u.FirstName == "gosho").First().Id);

            dbContext.SaveChanges();*/

            /*ILoggerFactory loggerFactory = new LoggerFactory();
            logger = new Logger<HomeController>(loggerFactory);

            var config = new ConfigurationBuilder()
            .AddInMemoryCollection()
            .Build();

            config["ConnectionStrings:DefaultConnection"] = connectionString;

            var firebaseSettings = new GoogleFirebaseSettings { ServiceAccountFilePath = Console.ReadLine(), BucketName = Console.ReadLine() };
            var options = Options.Create(firebaseSettings);

            var controller = new HomeController(logger, options, config);
            controller.InitializeUserEngine("1c2ee617-7134-44cd-920a-ee15408cff9a");

            var imagePath = "C:\\Users\\Shadow Dragon\\Desktop\\test.jpg";
            if (!File.Exists(imagePath)) throw new ArgumentException("No such image file.");

            var image = new StegoImage(imagePath);
            var imageStegoStrategy = new ImageStegoStrategy("Red,Green,Blue", 2, 1);
            image.Strategy = imageStegoStrategy.AsStegoStrategy;

            controller.HideMessageInImage("test123", image, "test.jpg", 1);

            var imagePath2 = "C:\\Users\\Shadow Dragon\\Desktop\\test2.jpg";
            if (!File.Exists(imagePath)) throw new ArgumentException("No such image file.");

            var image2 = new StegoImage(imagePath2);
            var imageStegoStrategy2 = new ImageStegoStrategy("Red,Green,Blue", 2, 1);
            image.Strategy = imageStegoStrategy2.AsStegoStrategy;

            var hiddenMessageLength = 7;

            var hiddenMessage = controller.ExtractHiddenMessageFromImage(hiddenMessageLength, image2);
            Console.WriteLine(hiddenMessage);*/
        }
    }
}
