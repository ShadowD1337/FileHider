using FileHider.Web.MVC.Data;
using FileHider.Web.MVC.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FileHider.Web.MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.   builder.Configuration.GetConnectionString("DefaultConnection")
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException("No DefaultConnection connection string found.")));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter(); 
            
            builder.Services.Configure<GoogleFirebaseSettings>(builder.Configuration.GetSection(GoogleFirebaseSettings.Section));

            builder.Services.AddDefaultIdentity<IdentityUser>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            //app.MapIdentityApi()

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}
