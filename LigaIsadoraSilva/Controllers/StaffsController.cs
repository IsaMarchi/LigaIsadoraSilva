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
    public class StaffsController : Controller
    {
        private readonly DataContext _context;

        public StaffsController(DataContext context)
        {
            _context = context;
        }

        // GET: Staffs
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.Staffs.Include(s => s.Club).Include(s => s.User);
            return View(await dataContext.ToListAsync());
        }

        // GET: Staffs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new PageNotFoundViewResult("PageNotFound");
            }

            var staff = await _context.Staffs
                .Include(s => s.Club)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (staff == null)
            {
                return new PageNotFoundViewResult("PageNotFound");
            }

            return View(staff);
        }

        // GET: Staffs/Create
        [Authorize(Roles = "Team")]
        public IActionResult Create()
        {
            ViewData["ClubId"] = new SelectList(_context.Clubs, "Id", "Coach");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Staffs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ClubId,FullName,ContactNumber,Email,StaffDutyId,UserId")] Staff staff)
        {
            if (ModelState.IsValid)
            {
                _context.Add(staff);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClubId"] = new SelectList(_context.Clubs, "Id", "Coach", staff.ClubId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", staff.UserId);
            return View(staff);
        }

        // GET: Staffs/Edit/5
        [Authorize(Roles = "Team")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new PageNotFoundViewResult("PageNotFound");
            }

            var staff = await _context.Staffs.FindAsync(id);
            if (staff == null)
            {
                return new PageNotFoundViewResult("PageNotFound");
            }
            ViewData["ClubId"] = new SelectList(_context.Clubs, "Id", "Coach", staff.ClubId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", staff.UserId);
            return View(staff);
        }

        // POST: Staffs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClubId,FullName,ContactNumber,Email,StaffDutyId,UserId")] Staff staff)
        {
            if (id != staff.Id)
            {
                return new PageNotFoundViewResult("PageNotFound");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(staff);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StaffExists(staff.Id))
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
            ViewData["ClubId"] = new SelectList(_context.Clubs, "Id", "Coach", staff.ClubId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", staff.UserId);
            return View(staff);
        }

        // GET: Staffs/Delete/5
        [Authorize(Roles = "Team")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new PageNotFoundViewResult("PageNotFound");
            }

            var staff = await _context.Staffs
                .Include(s => s.Club)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (staff == null)
            {
                return new PageNotFoundViewResult("PageNotFound");
            }

            return View(staff);
        }

        // POST: Staffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var staff = await _context.Staffs.FindAsync(id);
            if (staff != null)
            {
                _context.Staffs.Remove(staff);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StaffExists(int id)
        {
            return _context.Staffs.Any(e => e.Id == id);
        }

        public IActionResult PageNotFound()
        {
            return View();
        }
    }
}
