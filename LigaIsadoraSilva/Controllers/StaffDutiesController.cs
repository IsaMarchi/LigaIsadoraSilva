using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LigaIsadoraSilva.Data;
using LigaIsadoraSilva.Data.Entities;

namespace LigaIsadoraSilva.Controllers
{
    public class StaffDutiesController : Controller
    {
        private readonly DataContext _context;

        public StaffDutiesController(DataContext context)
        {
            _context = context;
        }

        // GET: StaffDuties
        public async Task<IActionResult> Index()
        {
            return View(await _context.StaffDuties.ToListAsync());
        }

        // GET: StaffDuties/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staffDuty = await _context.StaffDuties
                .FirstOrDefaultAsync(m => m.Id == id);
            if (staffDuty == null)
            {
                return NotFound();
            }

            return View(staffDuty);
        }

        // GET: StaffDuties/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StaffDuties/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] StaffDuty staffDuty)
        {
            if (ModelState.IsValid)
            {
                _context.Add(staffDuty);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(staffDuty);
        }

        // GET: StaffDuties/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staffDuty = await _context.StaffDuties.FindAsync(id);
            if (staffDuty == null)
            {
                return NotFound();
            }
            return View(staffDuty);
        }

        // POST: StaffDuties/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] StaffDuty staffDuty)
        {
            if (id != staffDuty.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(staffDuty);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StaffDutyExists(staffDuty.Id))
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
            return View(staffDuty);
        }

        // GET: StaffDuties/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staffDuty = await _context.StaffDuties
                .FirstOrDefaultAsync(m => m.Id == id);
            if (staffDuty == null)
            {
                return NotFound();
            }

            return View(staffDuty);
        }

        // POST: StaffDuties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var staffDuty = await _context.StaffDuties.FindAsync(id);
            if (staffDuty != null)
            {
                _context.StaffDuties.Remove(staffDuty);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StaffDutyExists(int id)
        {
            return _context.StaffDuties.Any(e => e.Id == id);
        }
    }
}
