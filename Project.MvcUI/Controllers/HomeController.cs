using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Bll.Managers.Abstracts;
using Project.Bll.Managers.Concretes;
using Project.Common.Tools;
using Project.Entities.Models;
using Project.MvcUI.Models;
using Project.MvcUI.Models.PureVms.AppUsers;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Project.MvcUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        readonly UserManager<AppUser> _userManager; //Identity'nin UserManager sistemi kullanıcı ekleme silme güncelleme ve raporlama ile (Crud) ile ilgilenir...
        readonly SignInManager<AppUser> _signInManager; //Identity'nin SignInManager sistemi kullanıcı login durumuyla ilgilenir...
        readonly RoleManager<AppRole> _roleManager; //Identity'nin RoleManager sistemi role yapısı icin Crud ile ilgilenir
        readonly IAppUserRoleManager _appUserRoleManager;







        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager, IAppUserRoleManager appUserRoleManager)
        {

            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _appUserRoleManager = appUserRoleManager;
        }

        public IActionResult Index(int id)
        {
           
          
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterRequestModel item)
        {
         
            Guid specId = Guid.NewGuid();


            //Map'lemek : Bir class'in alınan instance'inin icerisindeki property bilgilerini baska bir class'a aktarmak...
            AppUser appUser = new()
            {
                UserName = item.UserName,
                Email = item.Email,
                ActivationCode = specId
            };

            IdentityResult result = await _userManager.CreateAsync(appUser, item.Password); //IDentity otomatik olarak şifrelemeyi yapacaktır...Dikkat ettiyseniz item'dan gelen password'u vererek Identity'nin sireleme tarafını otomatik calıstırdık

            if (result.Succeeded)
            {
                #region RolKontrolIslemleri

                AppRole appRole = await _roleManager.FindByNameAsync("Member");
                if (appRole == null) await _roleManager.CreateAsync(new() { Name = "Member" });

                AppUserRole appUserRole = new AppUserRole()
                {
                    RoleId = appRole.Id,
                    UserId = appUser.Id
                };
                await _appUserRoleManager.CreateAsync(appUserRole);

                #endregion

                string message = $"Hesabınız olusturulmustur...Üyeliginizi onaylamak icin lütfen http://localhost:5112/Home/ConfirmEmail?specId={specId}&id={appUser.Id} linkine tıklayınız ";

                MailSender.Send(item.Email, body: message);
                TempData["Message"] = "Lütfen hesabınızı onaylamak icin emailinizi kontrol ediniz";

                return RedirectToAction("RedirectPanel");
            }

            return View();
        }

        public IActionResult RedirectPanel()
        {
            return View();
        }

        public async Task<IActionResult> ConfirmEmail(Guid specId,int id)
        {
            AppUser appUser = await _userManager.FindByIdAsync(id.ToString());
            if(appUser == null)
            {
                TempData["Message"] = "Kullanıcı  bulunamadı";
                return RedirectToAction("RedirectPanel");
            }
            else if(appUser.ActivationCode == specId)
            {
                appUser.EmailConfirmed = true;
                await _userManager.UpdateAsync(appUser);
                TempData["Message"] = "Hesabınız basarıyla onaylandı";
                return RedirectToAction("SignIn");
            }

            return RedirectToAction("Register");
        }

        public IActionResult SignIn()
        {
            return View();
        }
       
    }
}
