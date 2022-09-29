using AuthenticationMvc.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationMvc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation(); //AddRazorRuntimeCompilation
            #region MyRegion
            builder.Services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("sqlServer"));
                options.UseLazyLoadingProxies();
            }
            );
            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}