using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProjectNet.Models;


namespace ProjectNet.Controllers
{
    public class SanPhamsController : BaseController
    {
        private readonly QLNoiThatDBContext _context;
        private readonly ILogger<SanPhamsController> _logger;
        private readonly ILogger<AccountController> _acc;


        public SanPhamsController(ILogger<SanPhamsController> logger, QLNoiThatDBContext context, ILogger<AccountController> acc)
        {
            _context = context;
            _logger = logger;
            _acc = acc;
        }

        //Key luu chuoi json cua cart
        public const string CartKey = "cart";
        //Lay Cart tu session
        List<GioHang> LayGioHang()
        {
            var session = HttpContext.Session;
            string jsoncart = session.GetString(CartKey);
            if (jsoncart != null)
            {
                return JsonConvert.DeserializeObject<List<GioHang>> (jsoncart);

            }
            return new List<GioHang>();
        }
        //Xoa khoi session
        void ClearCart()
        {
            var session = HttpContext.Session;
            session.Remove(CartKey);
        }
        //Luu cart vo danh sach giohang
        void SaveCartSession(List<GioHang> ls)
        {
            var session = HttpContext.Session;
            string jsoncart = JsonConvert.SerializeObject(ls);
            session.SetString(CartKey, jsoncart);
        }
        //Processcart
        [Route("pay/{id:int}", Name = "pay")]
        public IActionResult DatHang([FromRoute] int id)
        {
            var sanpham = _context.sanPhams.Where(p => p.ID == id).FirstOrDefault();
            if (sanpham == null)
            {
                return NotFound("Khong co san pham");
            }
            //Xu li dua vao cart
            var cart = LayGioHang();
            var cartItem = cart.Find(p => p.sanPham.ID == id);
            if (cartItem != null)
            {
                //Da ton tai tang them 1
                cartItem.SoLuong++;
            }
            else
            {
                //Them moi
                cart.Add(new GioHang() { Id = id, sanPham = sanpham, DonGia = sanpham.GIABAN, SoLuong = 1 });
            }
            SaveCartSession(cart);
            //Chuyen den trang hien thi cart
            return RedirectToAction("Create", "HoaDons");

        }

        //AddToCart
        [Route("addcart/{id:int}", Name = "addcart")]
        public IActionResult AddToCart([FromRoute] int id)
        {
            if (!IsLogin)
            {
                return RedirectToAction("Login", "NhanViens");
            }
            var sanpham = _context.sanPhams.Where(p => p.ID == id).FirstOrDefault();
            if (sanpham == null)
            {
                return NotFound("Khong co san pham");
            }
            //Xu li dua vao cart
            var cart = LayGioHang();
            var cartItem = cart.Find(p => p.sanPham.ID == id);
            if (cartItem != null)
            {
                //Da ton tai tang them 1
                cartItem.SoLuong++;
            }
            else
            {
                //Them moi
                cart.Add(new GioHang() { Id = id, sanPham = sanpham, DonGia = sanpham.GIABAN, SoLuong = 1 });
            }
            SaveCartSession(cart);
            //Chuyen den trang hien thi cart
            return RedirectToAction("GioHang", "SanPhams");
        }


        [Route("/checkout", Name = "checkout")]
        public async Task<IActionResult> CheckOut(int id)
        {
            //xu ly khi dat hang
            var cart = LayGioHang();
            Hoadon hoadon = new Hoadon();
            hoadon.IDKH = id;
            hoadon.NGAYDATHANG = DateTime.Now;
            hoadon.NGAYGIAOHANG = DateTime.UtcNow;
            var user = await _context.nhanViens.FindAsync(id);
            hoadon.DIACHIGIAOHANG = user.DIACHI;
            hoadon.IDNV = user.ID;
            hoadon.IDPT = 1;
            hoadon.IDTT = 1;

            _context.hoadons.Add(hoadon);
            _context.SaveChanges();
            foreach (var item in cart)
            {
                Chitiethoadon chitiethoadon = new Chitiethoadon();
                chitiethoadon.IDHD = hoadon.Id;
                chitiethoadon.IDSP = item.sanPham.ID;
                chitiethoadon.SOLUONG = item.SoLuong;
                chitiethoadon.DONGIA = item.DonGia;
                _context.chitiethoadons.Add(chitiethoadon);

            }
            _context.SaveChanges();
            cart.Clear();
            ClearCart();
            return RedirectToAction("Success", "SanPhams");

        }

        public ActionResult Success()
        {
            List<Hoadon> hoadons = _context.hoadons.ToList();
            return View(hoadons);

        }

        //Xoa item trong cart
        [Route("/removecart/{id:int}", Name = "removecart")]
        public IActionResult RemoveCart([FromRoute] int id)
        {
            var cart = LayGioHang();
            var cartItem = cart.Find(p => p.sanPham.ID == id);
            if(cartItem != null)
            {
                cart.Remove(cartItem);
            }
            //Luu cart vao session
            SaveCartSession(cart);
            return RedirectToAction("GioHang", "SanPhams");
        }

        //Search
        public async Task<IActionResult> Search(SearchModel model)
        {
            var listSearch = from sp in _context.sanPhams
                             where sp.MASP == model.keyword ||
                             sp.TENSP.Contains(model.keyword)
                             select new SearchResult()
                             {
                                 TENSP = sp.TENSP,
                                 SOLUONGTON = sp.SOLUONGTON,
                                 MOTA = sp.MOTA,
                                 GIABAN = sp.GIABAN,
                                 CHATLIEU = sp.CHATLIEU,
                                 MAUSAC = sp.MAUSAC,
                                 BAOHANH = sp.BAOHANH,
                                 IMG = sp.IMG,
                             };
            ViewData["TuKhoa"] = model.keyword;
            return View("Search", listSearch);
        }
        //Them san pham vao gio hang
        public async Task<IActionResult> LocSanPham(int id)
        {
            var DsSP = _context.sanPhams;
            
            var listSP = from sp in DsSP.Include(s => s.LoaiSanPham)
                           where sp.ID == id
                           select sp;
            return View("Category", listSP);
        }
        public string Dem(int id)
        {
            var totalSp = (from sp in _context.sanPhams where sp.ID == id select sp.SOLUONGTON).Count();
            return totalSp.ToString();
        }
        public IActionResult GioHang()
        {
            return View(LayGioHang());
        }
        //Thanh tien
        public JsonResult Thanhtien(int soX, int soY)
        {
            int Thanhtien = soX * soY;
            return Json(Thanhtien.ToString("C", new CultureInfo("vi-VN")));
        }
        //dinh dang tien
        public JsonResult Dinhdang(int dongia, int y)
        {
            int Dinhdang = dongia + y;
            return Json(Dinhdang.ToString("C", new CultureInfo("vi-VN")));
        }


        public async Task<IActionResult> TrangChu()
        {
            var qLNoiThatDBContext = _context.sanPhams.Include(s => s.LoaiSanPham);
            return View(await qLNoiThatDBContext.ToListAsync());
        }
        public async Task<IActionResult> Category()
        {
            var qLNoiThatDBContext = _context.sanPhams.Include(s => s.LoaiSanPham);
            return View(await qLNoiThatDBContext.ToListAsync());
        }
        //ep kieu
        public int gia(int x)
        {
            int giaban = x;
            giaban.ToString("C", new CultureInfo("vi-VN"));
            return giaban;

        }
        

        //UploadIMG
        private string Upload(string msp, IFormFile formFile)
        {
            string path = Path.GetFullPath("./wwwroot/Avatar");
            path = path + "/" + msp;
            Directory.CreateDirectory(path); //Tao thu muc theo ma sinh vien
            string filePath = path + "/" + formFile.FileName;
            using var stream = new FileStream(filePath, FileMode.Create);
            formFile.CopyTo(stream); //copy file anh vao thu muc
            return msp + "/" + formFile.FileName;
            /* return filePath;*/
        }

        // GET: SanPhams
        public async Task<IActionResult> Index()
        {
            //Action bắt buộc phải kiểm tra đăng nhập mới được thực hiện
            if (!IsLogin)
            {
                return RedirectToAction("Login", "NhanViens");
            }
            var qLNoiThatDBContext = _context.sanPhams.Include(s => s.LoaiSanPham);
            /*ViewBag.Chao = "Xin chao " + _context.accounts.*/;
            return View(await qLNoiThatDBContext.ToListAsync());
        }

        // GET: SanPhams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.sanPhams == null)
            {
                return NotFound();
            }

            var sanPham = await _context.sanPhams
                .Include(s => s.LoaiSanPham)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (sanPham == null)
            {
                return NotFound();
            }

            return View(sanPham);
        }

        // GET: SanPhams/Create
        public IActionResult Create()
        {
            ViewData["LOAISANPHAMID"] = new SelectList(_context.loaiSanPhams, "ID", "TENLSP");
            return View();
        }

        // POST: SanPhams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,LOAISANPHAMID,MASP,TENSP,SOLUONGTON,IMG,MOTA,GIABAN,CHATLIEU,MAUSAC,BAOHANH")] SanPham sanPham, IFormFile formFile)
        {
            if (ModelState.IsValid)
            {
                string fileName = Upload(sanPham.MASP, formFile);
                

                _context.Add(new SanPham
                {
                    ID = sanPham.ID,
                    LOAISANPHAMID = sanPham.LOAISANPHAMID,
                    MASP = sanPham.MASP,
                    TENSP = sanPham.TENSP,
                    SOLUONGTON = sanPham.SOLUONGTON,
                    MOTA = sanPham.MOTA,
                    GIABAN = sanPham.GIABAN,
                    CHATLIEU = sanPham.CHATLIEU,
                    MAUSAC = sanPham.MAUSAC,
                    BAOHANH = sanPham.BAOHANH,
                    IMG = fileName
                });
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "SanPhams");
            }
            ViewData["LOAISANPHAMID"] = new SelectList(_context.loaiSanPhams, "ID", "TENLSP", sanPham.LOAISANPHAMID);
            return View(sanPham);
        }

        // GET: SanPhams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.sanPhams == null)
            {
                return NotFound();
            }

            var sanPham = await _context.sanPhams.FindAsync(id);
            if (sanPham == null)
            {
                return NotFound();
            }
            ViewData["LOAISANPHAMID"] = new SelectList(_context.loaiSanPhams, "ID", "TENLSP", sanPham.LOAISANPHAMID);
            return View(sanPham);
        }

        // POST: SanPhams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,LOAISANPHAMID,MASP,TENSP,SOLUONGTON,IMG,MOTA,GIABAN,CHATLIEU,MAUSAC,BAOHANH")] SanPham sanPham)
        {
            if (id != sanPham.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sanPham);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SanPhamExists(sanPham.ID))
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
            ViewData["LOAISANPHAMID"] = new SelectList(_context.loaiSanPhams, "ID", "TENLSP", sanPham.LOAISANPHAMID);
            return View(sanPham);
        }

        // GET: SanPhams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.sanPhams == null)
            {
                return NotFound();
            }

            var sanPham = await _context.sanPhams
                .Include(s => s.LoaiSanPham)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (sanPham == null)
            {
                return NotFound();
            }

            return View(sanPham);
        }

        // POST: SanPhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.sanPhams == null)
            {
                return Problem("Entity set 'QLNoiThatDBContext.sanPhams'  is null.");
            }
            var sanPham = await _context.sanPhams.FindAsync(id);
            if (sanPham != null)
            {
                _context.sanPhams.Remove(sanPham);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SanPhamExists(int id)
        {
          return (_context.sanPhams?.Any(e => e.ID == id)).GetValueOrDefault();
        }




        
    }
}
