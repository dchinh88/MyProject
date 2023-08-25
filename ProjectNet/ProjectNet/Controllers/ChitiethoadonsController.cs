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
    public class ChitiethoadonsController : Controller
    {
        private readonly QLNoiThatDBContext _context;

        public ChitiethoadonsController(QLNoiThatDBContext context)
        {
            _context = context;
        }

        // GET: Chitiethoadons
        public async Task<IActionResult> Index()
        {
              return _context.chitiethoadons != null ? 
                          View(await _context.chitiethoadons.ToListAsync()) :
                          Problem("Entity set 'QLNoiThatDBContext.chitiethoadons'  is null.");
        }

        // GET: Chitiethoadons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.chitiethoadons == null)
            {
                return NotFound();
            }

            var chitiethoadon = await _context.chitiethoadons
                .FirstOrDefaultAsync(m => m.ID == id);
            if (chitiethoadon == null)
            {
                return NotFound();
            }

            return View(chitiethoadon);
        }

        // GET: Chitiethoadons/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Chitiethoadons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,IDHD,IDSP,SOLUONG,DONGIA")] Chitiethoadon chitiethoadon)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chitiethoadon);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(chitiethoadon);
        }

        // GET: Chitiethoadons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.chitiethoadons == null)
            {
                return NotFound();
            }

            var chitiethoadon = await _context.chitiethoadons.FindAsync(id);
            if (chitiethoadon == null)
            {
                return NotFound();
            }
            return View(chitiethoadon);
        }

        // POST: Chitiethoadons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,IDHD,IDSP,SOLUONG,DONGIA")] Chitiethoadon chitiethoadon)
        {
            if (id != chitiethoadon.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chitiethoadon);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChitiethoadonExists(chitiethoadon.ID))
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
            return View(chitiethoadon);
        }

        // GET: Chitiethoadons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.chitiethoadons == null)
            {
                return NotFound();
            }

            var chitiethoadon = await _context.chitiethoadons
                .FirstOrDefaultAsync(m => m.ID == id);
            if (chitiethoadon == null)
            {
                return NotFound();
            }

            return View(chitiethoadon);
        }

        // POST: Chitiethoadons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.chitiethoadons == null)
            {
                return Problem("Entity set 'QLNoiThatDBContext.chitiethoadons'  is null.");
            }
            var chitiethoadon = await _context.chitiethoadons.FindAsync(id);
            if (chitiethoadon != null)
            {
                _context.chitiethoadons.Remove(chitiethoadon);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChitiethoadonExists(int id)
        {
          return (_context.chitiethoadons?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
