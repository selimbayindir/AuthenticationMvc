using AuthenticationMvc.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
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
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSqlConnection"));
                options.UseLazyLoadingProxies();
            }
            );
            /// Cookie
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Name = ".AuthenticationMvc.auth";//bu isimle sakla kullanýcý tarayýcýsýnda 
                    options.ExpireTimeSpan = TimeSpan.FromDays(7); //7 gün geçerli olacak
                    options.SlidingExpiration = false; //Sistem Kullanýldýkça 7 günden daha fazla ilerlesin demek ama kapatýyoruz
                    options.LoginPath = "/Account/Login"; //Geçersiz Olursa buraya 
                    options.LogoutPath = "/Account/Logout";//Çýkýþ yapýldýðýnda buraya
                    options.AccessDeniedPath = "/Home/MyAccesDenied";//Eriþim reddedildiðinde buraya geleceksin

                });
            #endregion


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();
            //Bunu biz ekliyoruz
            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.Run();       
        }
    }
}