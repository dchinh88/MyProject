using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using ProjectNet.Models;

namespace ProjectNet.Controllers
{
    public class NhanViensController : BaseController
    {
        private readonly QLNoiThatDBContext _context;

        public NhanViensController(QLNoiThatDBContext context)
        {
            _context = context;
        }
        //UploadIMG
        private string Upload(string mnv, IFormFile formFile)
        {
            string path = Path.GetFullPath("./wwwroot/AVTStaff");
            path = path + "/" + mnv;
            Directory.CreateDirectory(path); //Tao thu muc theo ma sinh vien
            string filePath = path + "/" + formFile.FileName;
            using var stream = new FileStream(filePath, FileMode.Create);
            formFile.CopyTo(stream); //copy file anh vao thu muc
            return mnv + "/" + formFile.FileName;
            /* return filePath;*/
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login([Bind("ID, TENDN, PASS")] NhanVien model)
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

        //Logout
        public IActionResult Logout()
        {
            CurrentUser = "";
            return RedirectToAction("Login");
        }
        // GET: NhanViens
        public async Task<IActionResult> Index()
        {
            //Action bắt buộc phải kiểm tra đăng nhập mới được thực hiện
            if (!IsLogin)
            {
                if (_context.nhanViens.All(m => m.ISADMIN == true))
                {
                    return RedirectToAction("Login", "NhanViens");
                }
                /*else
                {
                    return RedirectToAction("Index", "SanPhams");
                }*/
            }
            return _context.nhanViens != null ?
                          View(await _context.nhanViens.ToListAsync()) :
                          Problem("Entity set 'QLNoiThatDBContext.nhanViens'  is null.");
        }

        // GET: NhanViens/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.nhanViens == null)
            {
                return NotFound();
            }

            var nhanVien = await _context.nhanViens
                .FirstOrDefaultAsync(m => m.ID == id);
            if (nhanVien == null)
            {
                return NotFound();
            }

            return View(nhanVien);
        }

        // GET: NhanViens/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NhanViens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,MANV,HOTEN,NGAYSINH,DIACHI,SDT,AVARTAR,EMAIL,TENDN,PASS,ISADMIN")] NhanVien nhanVien, IFormFile formFile)
        {
            if (ModelState.IsValid)
            {
                string fileName = Upload(nhanVien.MANV, formFile);
                SHA256 hash = SHA256.Create();
                nhanVien.PASS = Utils.Cryptography.GetHash(hash, nhanVien.PASS);
                _context.Add(new NhanVien
                {
                    ID = nhanVien.ID,
                    MANV = nhanVien.MANV,
                    HOTEN = nhanVien.HOTEN,
                    NGAYSINH = nhanVien.NGAYSINH,
                    DIACHI = nhanVien.DIACHI,
                    SDT = nhanVien.SDT,
                    AVARTAR = fileName,
                    EMAIL = nhanVien.EMAIL,
                    TENDN = nhanVien.TENDN,
                    PASS = nhanVien.PASS,
                    ISADMIN = nhanVien.ISADMIN
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(nhanVien);
        }

        // GET: NhanViens/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.nhanViens == null)
            {
                return NotFound();
            }

            var nhanVien = await _context.nhanViens.FindAsync(id);
            if (nhanVien == null)
            {
                return NotFound();
            }
            return View(nhanVien);
        }

        // POST: NhanViens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,MANV,HOTEN,NGAYSINH,DIACHI,SDT,AVARTAR,EMAIL,TENDN,PASS,ISADMIN")] NhanVien nhanVien)
        {
            if (id != nhanVien.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nhanVien);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NhanVienExists(nhanVien.ID))
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
            return View(nhanVien);
        }

        // GET: NhanViens/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.nhanViens == null)
            {
                return NotFound();
            }

            var nhanVien = await _context.nhanViens
                .FirstOrDefaultAsync(m => m.ID == id);
            if (nhanVien == null)
            {
                return NotFound();
            }

            return View(nhanVien);
        }

        // POST: NhanViens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.nhanViens == null)
            {
                return Problem("Entity set 'QLNoiThatDBContext.nhanViens'  is null.");
            }
            var nhanVien = await _context.nhanViens.FindAsync(id);
            if (nhanVien != null)
            {
                _context.nhanViens.Remove(nhanVien);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NhanVienExists(int id)
        {
            return (_context.nhanViens?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
