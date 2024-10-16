using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LigaIsadoraSilva.Data;
using LigaIsadoraSilva.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using LigaIsadoraSilva.Helpers;
using System.Numerics;

namespace LigaIsadoraSilva.Controllers
{
    public class StaffsController : Controller
    {
        private readonly DataContext _context;
        private readonly IImageHelper _imageHelper;

        public StaffsController(DataContext context, IImageHelper imageHelper)
        {
            _context = context;
            _imageHelper = imageHelper;
        }

        // GET: Staffs
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.Staffs
           .Include(s => s.Club)
           //.Include(s => s.User)
           .Include(s => s.StaffDuty);  
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
                //.Include(s => s.User)
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
            ViewData["ClubId"] = new SelectList(_context.Clubs, "Id", "Name");
            ViewData["StaffDutyId"] = new SelectList(_context.StaffDuties, "Id", "Name"); // Carregar Staff Duties
            return View();
        }

        // POST: Staffs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Staff staff)
        {
            if (ModelState.IsValid)
            {
                if (staff.ImageFile != null)
                {
                    string imageUrl = await _imageHelper.UploadImageAsync(staff.ImageFile, "Staff");
                    staff.Photo = imageUrl;
                }
                _context.Add(staff);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["ClubId"] = new SelectList(_context.Clubs, "Id", "Name", staff.ClubId);
            ViewData["StaffDutyId"] = new SelectList(_context.StaffDuties, "Id", "Name", staff.StaffDutyId); // Manter seleção ao retornar erro
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

            ViewData["ClubId"] = new SelectList(_context.Clubs, "Id", "Name", staff.ClubId);
            ViewData["StaffDutyId"] = new SelectList(_context.StaffDuties, "Id", "Name", staff.StaffDutyId); // Adiciona os StaffDuties
            return View(staff);
        }


        // POST: Staffs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Staff staff)
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

            ViewData["ClubId"] = new SelectList(_context.Clubs, "Id", "Name", staff.ClubId);
            ViewData["StaffDutyId"] = new SelectList(_context.StaffDuties, "Id", "Name", staff.StaffDutyId); // Manter seleção ao retornar erro
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
                //.Include(s => s.User)
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
