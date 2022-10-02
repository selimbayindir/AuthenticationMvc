using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationMvc.Controllers
{
    [Authorize(Roles ="admin,manager")] //Authorize  olanlar girebilir.. //Rolü bunlardan  olanlar girebilir..
    public class AdminController : Controller
    {
      /*  [Authorize]*/ //Buraya özel Sorgular
        //[AllowAnonymous] //Bu Methoda giris serbest demektir
        public IActionResult Index()
        {
            return View();
        }
    }
}
