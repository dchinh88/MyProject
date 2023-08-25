using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectNet.Models;

namespace ProjectNet.Controllers
{
    public class HoadonsController : Controller
    {
        private readonly QLNoiThatDBContext _context;

        public HoadonsController(QLNoiThatDBContext context)
        {
            _context = context;
        }
        
        // GET: Hoadons
        public async Task<IActionResult> Index()
        {
              return _context.hoadons != null ? 
                          View(await _context.hoadons.ToListAsync()) :
                          Problem("Entity set 'QLNoiThatDBContext.hoadons'  is null.");
        }

        // GET: Hoadons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.hoadons == null)
            {
                return NotFound();
            }

            var hoadon = await _context.hoadons
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hoadon == null)
            {
                return NotFound();
            }

            return View(hoadon);
        }

        // GET: Hoadons/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Hoadons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IDKH,NGAYDATHANG,NGAYGIAOHANG,DIACHIGIAOHANG,IDNV,IDPT,IDTT")] Hoadon hoadon)
        {
            if (ModelState.IsValid)
            {

                _context.Add(hoadon);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hoadon);
        }

        // GET: Hoadons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.hoadons == null)
            {
                return NotFound();
            }

            var hoadon = await _context.hoadons.FindAsync(id);
            if (hoadon == null)
            {
                return NotFound();
            }
            return View(hoadon);
        }

        // POST: Hoadons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IDKH,NGAYDATHANG,NGAYGIAOHANG,DIACHIGIAOHANG,IDNV,IDPT,IDTT")] Hoadon hoadon)
        {
            if (id != hoadon.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hoadon);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HoadonExists(hoadon.Id))
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
            return View(hoadon);
        }

        // GET: Hoadons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.hoadons == null)
            {
                return NotFound();
            }

            var hoadon = await _context.hoadons
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hoadon == null)
            {
                return NotFound();
            }

            return View(hoadon);
        }

        // POST: Hoadons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.hoadons == null)
            {
                return Problem("Entity set 'QLNoiThatDBContext.hoadons'  is null.");
            }
            var hoadon = await _context.hoadons.FindAsync(id);
            if (hoadon != null)
            {
                _context.hoadons.Remove(hoadon);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HoadonExists(int id)
        {
          return (_context.hoadons?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
