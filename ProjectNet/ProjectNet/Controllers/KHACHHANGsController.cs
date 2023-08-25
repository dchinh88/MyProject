using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectNet.Models;

namespace ProjectNet.Controllers
{
    public class KHACHHANGsController : BaseController
    {
        private readonly QLNoiThatDBContext _context;

        public KHACHHANGsController(QLNoiThatDBContext context)
        {
            _context = context;
        }
        //UploadIMG
        private string Upload(string mkh, IFormFile formFile)
        {
            string path = Path.GetFullPath("./wwwroot/Customer");
            path = path + "/" + mkh;
            Directory.CreateDirectory(path); //Tao thu muc theo ma sinh vien
            string filePath = path + "/" + formFile.FileName;
            using var stream = new FileStream(filePath, FileMode.Create);
            formFile.CopyTo(stream); //copy file anh vao thu muc
            return mkh + "/" + formFile.FileName;
            /* return filePath;*/
        }
        // GET: KHACHHANGs
        public async Task<IActionResult> Index()
        {
            //Action bắt buộc phải kiểm tra đăng nhập mới được thực hiện
            if (!IsLogin)
            {
                return RedirectToAction("Login", "NhanViens");
            }
            /*if (!IsLogin)
            {
                return RedirectToAction("Login", "KHACHHANGs");
            }*/
            return _context.kHACHHANGs != null ? 
                          View(await _context.kHACHHANGs.ToListAsync()) :
                          Problem("Entity set 'QLNoiThatDBContext.kHACHHANGs'  is null.");
        }

        // GET: KHACHHANGs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.kHACHHANGs == null)
            {
                return NotFound();
            }

            var kHACHHANG = await _context.kHACHHANGs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kHACHHANG == null)
            {
                return NotFound();
            }

            return View(kHACHHANG);
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login([Bind("Id,EMAIL,PASS")] KHACHHANG kHACHHANG)
        {
            if (ModelState.IsValid)
            {
                //Kiểm tra user có tồn tại k?
                var loginUser = await _context.kHACHHANGs.FirstOrDefaultAsync(m => m.EMAIL == kHACHHANG.EMAIL);

                if (loginUser == null)
                {
                    ModelState.AddModelError("", "Đăng nhập thất bại");
                    return View(kHACHHANG);
                }
                else
                {
                    //Kiểm tra ma MD5 của pass hiện tại có khớp vs MD5 của pass đã lưu k?
                    SHA256 hasMethod = SHA256.Create();
                    if (Utils.Cryptography.VerifyHash(hasMethod, kHACHHANG.PASS, loginUser.PASS))
                    {
                        //Lưu trạng thái user
                        CurrentUser = loginUser.EMAIL;
                        return RedirectToAction("Index", "KHACHHANGs");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Đăng nhập thất bại");
                        return View(kHACHHANG);
                    }
                }
            }
            return View(kHACHHANG);
        }
        public IActionResult Register()
        {
            return View();
        }
        // GET: KHACHHANGs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: KHACHHANGs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MAKH,HOTEN,DIACHI,SDT,AVARTAR,EMAIL,PASS")] KHACHHANG kHACHHANG, IFormFile formFile)
        {
            if (ModelState.IsValid)
            {
                string fileName = Upload(kHACHHANG.MAKH, formFile);
                SHA256 hash = SHA256.Create();
                kHACHHANG.PASS = Utils.Cryptography.GetHash(hash, kHACHHANG.PASS);
                _context.Add(new KHACHHANG
                {
                    Id = kHACHHANG.Id,
                    MAKH = kHACHHANG.MAKH,
                    HOTEN = kHACHHANG.HOTEN,
                    DIACHI = kHACHHANG.DIACHI,
                    SDT = kHACHHANG.SDT,
                    AVARTAR = fileName,
                    EMAIL = kHACHHANG.EMAIL,
                    PASS = kHACHHANG.PASS
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(kHACHHANG);
        }

        // GET: KHACHHANGs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.kHACHHANGs == null)
            {
                return NotFound();
            }

            var kHACHHANG = await _context.kHACHHANGs.FindAsync(id);
            if (kHACHHANG == null)
            {
                return NotFound();
            }
            return View(kHACHHANG);
        }

        // POST: KHACHHANGs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MAKH,HOTEN,DIACHI,SDT,AVARTAR,EMAIL,PASS")] KHACHHANG kHACHHANG)
        {
            if (id != kHACHHANG.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kHACHHANG);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KHACHHANGExists(kHACHHANG.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(kHACHHANG);
        }

        // GET: KHACHHANGs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.kHACHHANGs == null)
            {
                return NotFound();
            }

            var kHACHHANG = await _context.kHACHHANGs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kHACHHANG == null)
            {
                return NotFound();
            }

            return View(kHACHHANG);
        }

        // POST: KHACHHANGs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.kHACHHANGs == null)
            {
                return Problem("Entity set 'QLNoiThatDBContext.kHACHHANGs'  is null.");
            }
            var kHACHHANG = await _context.kHACHHANGs.FindAsync(id);
            if (kHACHHANG != null)
            {
                _context.kHACHHANGs.Remove(kHACHHANG);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KHACHHANGExists(int id)
        {
          return (_context.kHACHHANGs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
