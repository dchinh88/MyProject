using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectNet.Models;

namespace ProjectNet.Controllers
{
    public class BinhluansController : Controller
    {
        private readonly QLNoiThatDBContext _context;

        public BinhluansController(QLNoiThatDBContext context)
        {
            _context = context;
        }

        // GET: Binhluans
        public async Task<IActionResult> Index()
        {
              return _context.binhluans != null ? 
                          View(await _context.binhluans.ToListAsync()) :
                          Problem("Entity set 'QLNoiThatDBContext.binhluans'  is null.");
        }

        // GET: Binhluans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.binhluans == null)
            {
                return NotFound();
            }

            var binhluan = await _context.binhluans
                .FirstOrDefaultAsync(m => m.ID == id);
            if (binhluan == null)
            {
                return NotFound();
            }

            return View(binhluan);
        }

        // GET: Binhluans/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Binhluans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,IDKH,IDSP,NOIDUNGBL,THOIGIANBL")] Binhluan binhluan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(binhluan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(binhluan);
        }

        // GET: Binhluans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.binhluans == null)
            {
                return NotFound();
            }

            var binhluan = await _context.binhluans.FindAsync(id);
            if (binhluan == null)
            {
                return NotFound();
            }
            return View(binhluan);
        }

        // POST: Binhluans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,IDKH,IDSP,NOIDUNGBL,THOIGIANBL")] Binhluan binhluan)
        {
            if (id != binhluan.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(binhluan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BinhluanExists(binhluan.ID))
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
            return View(binhluan);
        }

        // GET: Binhluans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.binhluans == null)
            {
                return NotFound();
            }

            var binhluan = await _context.binhluans
                .FirstOrDefaultAsync(m => m.ID == id);
            if (binhluan == null)
            {
                return NotFound();
            }

            return View(binhluan);
        }

        // POST: Binhluans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.binhluans == null)
            {
                return Problem("Entity set 'QLNoiThatDBContext.binhluans'  is null.");
            }
            var binhluan = await _context.binhluans.FindAsync(id);
            if (binhluan != null)
            {
                _context.binhluans.Remove(binhluan);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BinhluanExists(int id)
        {
          return (_context.binhluans?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
