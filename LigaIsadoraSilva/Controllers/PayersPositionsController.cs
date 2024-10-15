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
    public class PayersPositionsController : Controller
    {
        private readonly DataContext _context;

        public PayersPositionsController(DataContext context)
        {
            _context = context;
        }

        // GET: PayersPositions
        public async Task<IActionResult> Index()
        {
            return View(await _context.PayersPositions.ToListAsync());
        }

        // GET: PayersPositions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new PageNotFoundViewResult("PageNotFound");
            }

            var payersPosition = await _context.PayersPositions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (payersPosition == null)
            {
                return new PageNotFoundViewResult("PageNotFound");
            }

            return View(payersPosition);
        }

        // GET: PayersPositions/Create
        [Authorize(Roles = "Club")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: PayersPositions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( PayersPosition payersPosition)
        {
            if (ModelState.IsValid)
            {
                _context.Add(payersPosition);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(payersPosition);
        }

        // GET: PayersPositions/Edit/5
        [Authorize(Roles = "Club")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new PageNotFoundViewResult("PageNotFound");
            }

            var payersPosition = await _context.PayersPositions.FindAsync(id);
            if (payersPosition == null)
            {
                return new PageNotFoundViewResult("PageNotFound");
            }
            return View(payersPosition);
        }

        // POST: PayersPositions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PayersPosition payersPosition)
        {
            if (id != payersPosition.Id)
            {
                return new PageNotFoundViewResult("PageNotFound");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(payersPosition);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PayersPositionExists(payersPosition.Id))
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
            return View(payersPosition);
        }

        // GET: PayersPositions/Delete/5
        [Authorize(Roles = "Club")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new PageNotFoundViewResult("PageNotFound");
            }

            var payersPosition = await _context.PayersPositions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (payersPosition == null)
            {
                return new PageNotFoundViewResult("PageNotFound");
            }

            return View(payersPosition);
        }

        // POST: PayersPositions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var payersPosition = await _context.PayersPositions.FindAsync(id);
            if (payersPosition != null)
            {
                _context.PayersPositions.Remove(payersPosition);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PayersPositionExists(int id)
        {
            return _context.PayersPositions.Any(e => e.Id == id);
        }

        public IActionResult PageNotFound()
        {
            return View();
        }
    }
}
