using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStore.Domain.DAL;
using BookStore.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using BookStore.Utility;
using Microsoft.AspNetCore.Http;
using BookStore.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace BookStore.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly BookStoreContext _context;
        private readonly AppSettings _appSettings;
        private readonly IHostingEnvironment _env;

        public AccountController(BookStoreContext context, IOptions<AppSettings> appSettings, IHostingEnvironment env)
        {
            _context = context;
            _appSettings = appSettings.Value;
            _env = env;
        }

        // GET: User
        [Route("[controller]/index")]
        [Route("[controller]")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }


        [Route("[controller]/profile/{username}/edition")]
        public IActionResult ProfileEdition(string username)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user != null)
            {
                var vm = new ProfileViewModel<BookEdition>
                {
                    User = user,
                    ActionLogList = _context.BookEditions
                        .Where(u => u.User == user).OrderByDescending(t => t.CreateTime)
                        .Include(be=>be.Book)
                        .ToList()
                };
                return View(vm);
            }
            TempData.Flash("danger", $"用户<span class='text-danger'><b> {username} </b></span>不存在！");
            return NotFound();
        }

        [Route("[controller]/profile/{username}/push")]
        public IActionResult ProfilePush(string username)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user != null)
            {
                var vm = new ProfileViewModel<ActionLog>
                {
                    User = user,
                    ActionLogList = _context.ActionLogs.Where(x => x.Taxonomy == TaxonomyEnum.Push)
                        .Where(x => x.User == user).OrderByDescending(x => x.CreateTime)
                        .Include(x=>x.BookEdition).Include(x=>x.BookEdition.Book)
                        .ToList()
                };
                return View(vm);
            }
            TempData.Flash("danger", $"用户<span class='text-danger'><b> {username} </b></span>不存在！");
            return NotFound();
        }

        [Route("[controller]/profile/{username}/download")]
        public IActionResult ProfileDownload(string username)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user != null)
            {
                var vm = new ProfileViewModel<ActionLog>
                {
                    User = user,
                    ActionLogList = _context.ActionLogs.Where(x => x.Taxonomy == TaxonomyEnum.Download)
                        .Where(x => x.User == user).OrderByDescending(x => x.CreateTime)
                        .Include(x=>x.BookEdition).Include(x=>x.BookEdition.Book)
                        .ToList()
                };
                return View(vm);
            }
            TempData.Flash("danger", $"用户<span class='text-danger'><b> {username} </b></span>不存在！");
            return NotFound();
        }

        [Route("[controller]/profile/{username}/favorite")]
        public IActionResult ProfileFavorite(string username)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user != null)
            {
                var vm = new ProfileViewModel<ActionLog>
                {
                    User = user,
                    ActionLogList = _context.ActionLogs.Where(x => x.Taxonomy == TaxonomyEnum.Favorite)
                        .Where(x => x.User == user).OrderByDescending(x => x.CreateTime)
                        .Include(x=>x.BookEdition).Include(x=>x.BookEdition.Book)
                        .ToList()
                };
                return View(vm);
            }
            TempData.Flash("danger", $"用户<span class='text-danger'><b> {username} </b></span>不存在！");
            return NotFound();
        }

        [Route("[controller]/profile/{username}/comment")]
        public IActionResult ProfileComment(string username)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user != null)
            {
                var vm = new ProfileViewModel<BookEditionComment>
                {
                    ActionLogList = _context.BookEditionComments.Where(x => x.User == user)
                        .OrderByDescending(x => x.CreateTime)
                        .Include(x=>x.BookEdition).Include(x=>x.BookEdition.Book)
                        .ToList(),
                    User = user
                };
                return View(vm);
            }
            TempData.Flash("danger", $"用户<span class='text-danger'><b> {username} </b></span>不存在！");
            return NotFound();
        }

        [Route("[controller]/profile/{username}/checkin")]
        public IActionResult ProfileCheckin(string username)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user != null)
            {
                var vm = new ProfileViewModel<ActionLog>
                {
                    User = user,
                    ActionLogList = _context.ActionLogs.Where(x => x.Taxonomy == TaxonomyEnum.Checkin)
                        .Where(x => x.User == user)
                        .OrderByDescending(x => x.CreateTime)
                        .ToList()
                };
                return View(vm);
            }
            TempData.Flash("danger", $"用户<span class='text-danger'><b> {username} </b></span>不存在！");
            return NotFound();
        }
        
        [Route("[controller]/settings/username")]
        public async Task<IActionResult> SettingsUsername()
        {
            var loginUser = await _context.Users.FirstOrDefaultAsync(m => m.Username == HttpContext.User.Identity.Name);
            var vm = new SettingsUsernameViewModel
            {
                User = loginUser,
                PushmainDomain = _appSettings.PushmailDomain
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[controller]/settings/username")]
        public async Task<IActionResult> SettingsUsername(SettingsUsernameViewModel vm)
        {
            var loginUser = _context.Users.FirstOrDefault(m => m.Username == HttpContext.User.Identity.Name);
            if (ModelState.IsValid)
            {
                loginUser.Username = vm.Username;
                await _context.SaveChangesAsync();
                TempData.Flash("success", $"用户名更改成功，新的用户名：{vm.Username}");
                return RedirectToAction(nameof(Logout));
            }
            vm.User = loginUser;
            vm.PushmainDomain = _appSettings.PushmailDomain;
            return View(vm);
        }

        [Route("[controller]/settings/push")]
        public async Task<IActionResult> SettingsPush()
        {
            var loginUser = _context.Users.FirstOrDefault(m => m.Username == HttpContext.User.Identity.Name);
            var vm = new SettingsPushViewModel
            {
                PushSettings = await _context.PushSettings.Where(u => u.User == loginUser)
                    .OrderByDescending(x => x.CreateTime).ToListAsync(),
                User = loginUser
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[controller]/settings/push")]
        public async Task<IActionResult> SettingsPush([Bind("PushEmail")] SettingsPushViewModel vm)
        {
            var loginUser = _context.Users.FirstOrDefault(m => m.Username == HttpContext.User.Identity.Name);
            if (ModelState.IsValid)
            {
                var pushSetting = new PushSetting
                {
                    Id = IdGen.NewId(AppkeyEnum.PushSettings.ToString()),
                    PushEmail = vm.PushEmail,
                    User = loginUser,
                    CreateTime = DateTime.Now
                };
                _context.PushSettings.Add(pushSetting);
                await _context.SaveChangesAsync();
                TempData.Flash("success", $"成功添加推送邮箱: <span class='text-danger'><b>{vm.PushEmail}</b></span>");
                return RedirectToAction(nameof(SettingsPush));
            }
            vm.PushSettings = await _context.PushSettings.Where(u => u.User == loginUser)
                .OrderByDescending(x => x.CreateTime).ToListAsync();
            vm.User = loginUser;
            return View(vm);
        }

        [Route("[controller]/settings/push/delete")]
        public JsonResult SettingsPushDelete(long id)
        {
            if (id > 0)
            {
                var pushSetting = _context.PushSettings.FirstOrDefault(x => x.Id == id);
                if (pushSetting != null)
                {
                    _context.PushSettings.Remove(pushSetting);
                    _context.SaveChanges();
                    return Json(true);
                }
            }
            return Json(false);
        }

        [Route("[controller]/settings/push/default")]
        public JsonResult SettingsPushDefault(long id)
        {
            if (id > 0)
            {
                var loginUser = _context.Users.FirstOrDefault(m => m.Username == HttpContext.User.Identity.Name);
                var pushSettings = _context.PushSettings.Where(x => x.User == loginUser).ToList(); //当前用户的所有设置
                var pushSettingDefault = _context.PushSettings.FirstOrDefault(x => x.Id == id);
                if (pushSettingDefault != null)
                {
                    pushSettings.ForEach(ps => { ps.IsDefault = 0; });
                    pushSettingDefault.IsDefault = 1;
                    _context.SaveChanges();
                    return Json(true);
                }
            }
            return Json(false);
        }

        [Route("[Controller]/settings/avatar")]
        public IActionResult SettingsAvatar()
        {
            var loginUser = _context.Users.FirstOrDefault(m => m.Username == HttpContext.User.Identity.Name);
            var vm = new SettingsAvatarViewModel
            {
                User = loginUser,
                CurrentAvatar = loginUser.Avatar
            };
            return View(vm);
        }

        [Route("[Controller]/settings/avatar")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SettingsAvatar(SettingsAvatarViewModel vm)
        {
            var loginUser = _context.Users.FirstOrDefault(m => m.Username == HttpContext.User.Identity.Name);
            if (ModelState.IsValid)
            {
                var avatarFileFolder = Path.Combine(_env.WebRootPath, _appSettings.UploadAvatarDir);
                if (!Directory.Exists(avatarFileFolder)) Directory.CreateDirectory(avatarFileFolder);
                var filename = Utils.GetId() + Path.GetExtension(vm.AvatarFile.FileName);
                var filepath = Path.Combine(avatarFileFolder, filename);
                using (var stream = new FileStream(filepath, FileMode.Create))
                {
                    vm.AvatarFile.CopyTo(stream);
                }
                loginUser.Avatar = filename;
                await _context.SaveChangesAsync();
                TempData.Flash("success", "头像上传成功");
                return RedirectToAction(nameof(SettingsAvatar));
            }

            vm.CurrentAvatar = loginUser.Avatar;
            vm.User = loginUser;
            return View(vm);
        }


        [Route("[controller]/change_password")]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[controller]/change_password")]
        public async Task<IActionResult> ChangePassword(
            [Bind("Password", "NewPassword", "ConfirmNewPassword")] ChangePasswordViewModel vm)
        {
            var loginUser = _context.Users.FirstOrDefault(m => m.Username == HttpContext.User.Identity.Name);
            if (ModelState.IsValid)
            {
                loginUser.Password = Utils.GeneratePassword(vm.NewPassword);
                await _context.SaveChangesAsync();
                TempData.Flash("success", "密码修改成功, 下次请使用新密码登陆");

                return RedirectToAction(nameof(ChangePassword));
            }
            vm.User = loginUser;
            return View(vm);
        }

        [Route("[controller]/change_email")]
        public IActionResult ChangeEmail()
        {
            var loginUser = _context.Users.FirstOrDefault(m => m.Username == HttpContext.User.Identity.Name);
            var vm = new ChangeEmailViewModel
            {
                User = loginUser,
                CurrentEmail = loginUser != null ? loginUser.Email : ""
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[controller]/change_email")]
        public async Task<IActionResult> ChangeEmail([Bind("Email")] ChangeEmailViewModel vm)
        {
            var loginUser = _context.Users.FirstOrDefault(m => m.Username == HttpContext.User.Identity.Name);
            if (ModelState.IsValid)
            {
                loginUser.Email = vm.Email;
                await _context.SaveChangesAsync();
                TempData.Flash("success", $"邮箱修改成功, 新邮箱为: {vm.Email}");
                return RedirectToAction(nameof(ChangeEmail));
            }
            vm.User = loginUser;
            return View(vm);
        }
        [Route("[controller]/settings/pro")]
        public IActionResult SettingsPro()
        {
            return View();
        }


        [AllowAnonymous]
        [Route("[controller]/register")]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[controller]/register")]
        public async Task<IActionResult> Register(
            [Bind("Username,Password,ConfirmPassword,Email")] RegisterViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);
            var user = new User
            {
                Id = IdGen.NewId(AppkeyEnum.Users.ToString()),
                Username = vm.Username,
                Password = Utils.GeneratePassword(vm.Password),
                Email = vm.Email,
                UserType = UserTypeEnum.Free,
                CreateTime = DateTime.Now
            };
            _context.Add(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Login));
        }


        [AllowAnonymous]
        [Route("[controller]/login")]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[controller]/login")]
        public async Task<IActionResult> Login([Bind("Username,Password,VerifyCode")] LoginViewModel vm,
            string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid) return View(vm);
            var user = _context.Users.FirstOrDefault(m =>
                string.Equals(m.Username, vm.Username, StringComparison.CurrentCultureIgnoreCase));
            if (user.Password == Utils.GeneratePassword(vm.Password))
            {
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, user.Username));
                identity.AddClaim(new Claim(ClaimTypes.Sid, user.Id.ToString()));
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity));
                HttpContext.Session.Remove("verify_code"); //清除验证码session
                user.LoginCount += 1;
                user.LoginTime = DateTime.Now;
                user.LoginIp = HttpContext.GetUserIp();
                await _context.SaveChangesAsync();
                TempData.Flash("success", string.Format("登录成功，欢迎你{0}", user.Username));
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
            }
            ModelState.AddModelError("Password", "密码不正确.");
            return View(vm);
        }

        [Route("[controller]/logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TempData.Flash("success", "成功登出系统");
            return RedirectToAction(nameof(Login));
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("[controller]/validate/username")]
        public JsonResult ValidateUsername(string username, string initialUsername)
        {
            if (!string.Equals(username, initialUsername, StringComparison.CurrentCultureIgnoreCase))
            {
                return Json(!_context.Users.Any(x =>
                    string.Equals(x.Username, username, StringComparison.CurrentCultureIgnoreCase)));
            }
            return Json(true);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("[controller]/validate/email")]
        public JsonResult ValidateEmail(string email, string initialEmail)
        {
            if (!string.Equals(email, initialEmail, StringComparison.CurrentCultureIgnoreCase))
            {
                return Json(!_context.Users.Any(x =>
                    string.Equals(x.Email, email, StringComparison.CurrentCultureIgnoreCase)));
            }
            return Json(true);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("[controller]/validate/pushemail")]
        public JsonResult ValidatePushEmail(string pushEmail)
        {
            return Json(!_context.PushSettings.Any(x =>
                string.Equals(x.PushEmail, pushEmail, StringComparison.CurrentCultureIgnoreCase)));
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("[controller]/check/username")]
        public JsonResult CheckUsername(string username)
        {
            return Json(_context.Users.Any(x =>
                x.IsDelete == 0 && string.Equals(x.Username, username, StringComparison.CurrentCultureIgnoreCase)));
        }

        [AllowAnonymous]
        [Route("[controller]/verifycode")]
        public IActionResult VerifyCode()
        {
            MemoryStream ms = VerifyCodeUtil.GenerateCode(out string code);
            HttpContext.Session.SetString("verify_code", code);
            Response.Body.Dispose();
            return File(ms.ToArray(), @"image/png");
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("[controller]/check/verifycode")]
        public JsonResult CheckVerifyCode(string verifyCode)
        {
            if (!string.IsNullOrEmpty(verifyCode))
            {
                string sessionCode = HttpContext.Session.GetString("verify_code");
                return Json(string.Equals(sessionCode.Trim(), verifyCode.Trim(),
                    StringComparison.CurrentCultureIgnoreCase));
            }
            return Json(false);
        }

        [Authorize]
        [HttpPost]
        [Route("[controller]/check/password")]
        public JsonResult CheckPassword(string password)
        {
            var loginUser = _context.Users.FirstOrDefault(m => m.Username == HttpContext.User.Identity.Name);
            return Json(Utils.GeneratePassword(password) == loginUser.Password);
        }

       
    }
}