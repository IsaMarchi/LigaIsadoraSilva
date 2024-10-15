using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LigaIsadoraSilva.Data;
using LigaIsadoraSilva.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using LigaIsadoraSilva.Helpers;

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
                return new PageNotFoundViewResult("PageNotFound");
            }

            var staffDuty = await _context.StaffDuties
                .FirstOrDefaultAsync(m => m.Id == id);
            if (staffDuty == null)
            {
                return new PageNotFoundViewResult("PageNotFound");
            }

            return View(staffDuty);
        }

        // GET: StaffDuties/Create
        [Authorize(Roles = "Team")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: StaffDuties/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StaffDuty staffDuty)
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
        [Authorize(Roles = "Team")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new PageNotFoundViewResult("PageNotFound");
            }

            var staffDuty = await _context.StaffDuties.FindAsync(id);
            if (staffDuty == null)
            {
                return new PageNotFoundViewResult("PageNotFound");
            }
            return View(staffDuty);
        }

        // POST: StaffDuties/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StaffDuty staffDuty)
        {
            if (id != staffDuty.Id)
            {
                return new PageNotFoundViewResult("PageNotFound");
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
                        return new PageNotFoundViewResult("PageNotFound");
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
        [Authorize(Roles = "Team")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new PageNotFoundViewResult("PageNotFound");
            }

            var staffDuty = await _context.StaffDuties
                .FirstOrDefaultAsync(m => m.Id == id);
            if (staffDuty == null)
            {
                return new PageNotFoundViewResult("PageNotFound");
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

        public IActionResult PageNotFound()
        {
            return View();
        }
    }
}

