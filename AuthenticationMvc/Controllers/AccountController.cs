using AuthenticationMvc.Entities;
using AuthenticationMvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationMvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IConfiguration _configuration; //appsettings de ki veriyi okumak Configuration IC

        public AccountController(DatabaseContext databaseContext,IConfiguration configuration) //bağımlılık yaratır senbusun 
        {
            _databaseContext = databaseContext;
            _configuration = configuration;
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new()
                {
                    UserName = model.UserName,
                    Password = model.Password,

                };
                _databaseContext.Add(user);
             int affectedRowCount=_databaseContext.SaveChanges();
                if (affectedRowCount==0)
                {
                    ModelState.AddModelError("", "KULLANICI Eklenemedi"); //UserName sadece bu boş bırakırsan genel hata sayfa başında gösterir
                }
                else
                {
                    return RedirectToAction(nameof(Login));
                }
                return View(user);

            }
            return View(model); //hata lar buradan dönecek
        }
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult Profile()
        {
            return View();
        }
    }
}
