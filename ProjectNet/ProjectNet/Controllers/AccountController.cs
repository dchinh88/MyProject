using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectNet.Models;
using System.Security.Cryptography;

namespace ProjectNet.Controllers
{
    public class AccountController : BaseController
    {
        private readonly QLNoiThatDBContext _context;

        public AccountController(QLNoiThatDBContext context)
        {
            _context = context;
        }
        //

        public IActionResult Login()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login([Bind("ID,TENDN,PASS")] Account model)
        {
            if (ModelState.IsValid)
            {
                //Kiểm tra user có tồn tại k?
                var loginUser = await _context.nhanViens.FirstOrDefaultAsync(m => m.TENDN == model.TENDN);
                if (loginUser == null)
                {
                    ModelState.AddModelError("", "Đăng nhập thất bại");
                    return View(model);
                }
                else
                {
                    //Kiểm tra ma MD5 của pass hiện tại có khớp vs MD5 của pass đã lưu k?
                    SHA256 hasMethod = SHA256.Create();
                    if (Utils.Cryptography.VerifyHash(hasMethod, model.PASS, loginUser.PASS))
                    {
                        //Lưu trạng thái user
                        CurrentUser = loginUser.TENDN;
                        return RedirectToAction("Index", "SanPhams");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Đăng nhập thất bại");
                        return View(model);
                    }
                }
            }
            return View(model);
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("TENDN, PASS")] Account model)
        {
            if (ModelState.IsValid)
            {
                //Mã hóa mật khẩu
                SHA256 hash = SHA256.Create();
                model.PASS = Utils.Cryptography.GetHash(hash, model.PASS);
                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login", "Account");
            }
            return View(model);
        }
        public IActionResult Logout()
        {
            CurrentUser = "";
            return RedirectToAction("Login");
        }
    }
}
