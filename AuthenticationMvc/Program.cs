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
                    options.Cookie.Name = ".AuthenticationMvc.auth";//bu isimle sakla kullan�c� taray�c�s�nda 
                    options.ExpireTimeSpan = TimeSpan.FromDays(7); //7 g�n ge�erli olacak
                    options.SlidingExpiration = false; //Sistem Kullan�ld�k�a 7 g�nden daha fazla ilerlesin demek ama kapat�yoruz
                    options.LoginPath = "/Account/Login"; //Ge�ersiz Olursa buraya 
                    options.LogoutPath = "/Account/Logout";//��k�� yap�ld���nda buraya
                    options.AccessDeniedPath = "/Home/MyAccesDenied";//Eri�im reddedildi�inde buraya geleceksin

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