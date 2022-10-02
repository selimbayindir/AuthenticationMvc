using AuthenticationMvc.Entities;
using AuthenticationMvc.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NETCore.Encrypt.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace AuthenticationMvc.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IConfiguration _configuration; //appsettings de ki veriyi okumak Configuration IC

        public AccountController(DatabaseContext databaseContext, IConfiguration configuration) //bağımlılık yaratır senbusun 
        {
            _databaseContext = databaseContext;
            _configuration = configuration;
        }
        
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {

            if (ModelState.IsValid)
            {
                String md5Salt = _configuration.GetValue<string>("AppSettings:MD5Salt"); //Okuyor
                String saltedPass = model.Password + md5Salt;//Birleştiriyor.
                String hashedPass = saltedPass.MD5();

                User user = _databaseContext.users.SingleOrDefault(p => p.UserName.ToLower() == model.UserName.ToLower()
                && p.Password.ToLower() == hashedPass.ToLower());
                if (user != null)
                {
                    if (user.Active)
                    {
                        ModelState.AddModelError(nameof(model.UserName), "User is locked..");
                    }
                    //Cookie Authentication başlar
                    List<Claim> claims = new List<Claim>();
                    claims.Add(new Claim("Id", user.Id.ToString()));
                    ///claims.Add(new Claim("FullName", user.FullName ?? String.Empty));
                    //claims.Add(new Claim("FullName", user.FullName ?? String.Empty));
                    //claims.Add(new Claim("UserName", user.UserName));
                    //2.Yol
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, user.FullName ?? String.Empty));
                    claims.Add(new Claim(ClaimTypes.Name, user.FullName ?? String.Empty));
                    claims.Add(new Claim(ClaimTypes.Role, user.Role));
                    claims.Add(new Claim("UserName", user.UserName));

                    ClaimsIdentity ıdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    //Hangi AUTH U KULLANCAĞIMIZI SÖYLERİZ
                    ClaimsPrincipal principal = new ClaimsPrincipal(ıdentity);

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "UserName or password is incorrect");
                    //Cookie Authentication başlar
                }

            }
            return View(model); //hata lar buradan dönecek
        }
        [AllowAnonymous]

        public IActionResult Register()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {

            if (ModelState.IsValid)
            {
                if (_databaseContext.users.Any(p => p.UserName.ToLower() == model.UserName.ToLower()))
                {
                    ModelState.AddModelError(nameof(model.UserName), "Kullanıcı Adı Kullanılmaktadır.");
                    View(model);
                }
                String md5Salt = _configuration.GetValue<string>("AppSettings:MD5Salt"); //Okuyor
                String saltedPass = model.Password + md5Salt;//Birleştiriyor.
                String hashedPass = saltedPass.MD5();
                User user = new()
                {
                    UserName = model.UserName,
                    Password = hashedPass
                };
                _databaseContext.users.Add(user);
                int affectedRowCount = _databaseContext.SaveChanges();
                if (affectedRowCount == 0)
                {
                    ModelState.AddModelError("", "User Can Not Be Added");
                }
                else
                {
                    return RedirectToAction(nameof(Login));
                }
            }
            return View(model);
        }

        public IActionResult Profile()
        {
            ProfileInfoLoader();
            return View();
        }
        private string DoMD5HashedString(string s)
        {
            string md5Salt = _configuration.GetValue<string>("AppSettings:MD5Salt");
            string salted = s + md5Salt;
            string hashed = salted.MD5();
            return hashed;
        }
        private void ProfileInfoLoader()
        {
            Guid userid = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
            User user = _databaseContext.users.SingleOrDefault(x => x.Id == userid);

            ViewData["FullName"] = user.FullName;
        }

        [HttpPost]
        public IActionResult ProfileChangeFullName([Required][StringLength(50)] string? fullname)
        {
            if (ModelState.IsValid)
            {
                Guid userid = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
                User user = _databaseContext.users.SingleOrDefault(x => x.Id == userid);

                user.FullName = fullname;
                _databaseContext.SaveChanges();

                return RedirectToAction(nameof(Profile));
            }

            ProfileInfoLoader();
            return View("Profile");
        }

        [HttpPost]
        public IActionResult ProfileChangePassword([Required][MinLength(6)][MaxLength(16)] string? password)
        {
            if (ModelState.IsValid)
            {
                Guid userid = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
                User user = _databaseContext.users.SingleOrDefault(x => x.Id == userid);

                string hashedPassword = DoMD5HashedString(password);

                user.Password = hashedPassword;
                _databaseContext.SaveChanges();

                ViewData["result"] = "PasswordChanged";
            }

            ProfileInfoLoader();
            return View("Profile");
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }
    }
}
