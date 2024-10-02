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
    public class FootballMatchesController : Controller
    {
        private readonly DataContext _context;

        public FootballMatchesController(DataContext context)
        {
            _context = context;
        }

        // GET: FootballMatches
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.Games.Include(f => f.HomeTeam).Include(f => f.VisitTeam);
            return View(await dataContext.ToListAsync());
        }

        // GET: FootballMatches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var footballMatch = await _context.Games
                .Include(f => f.HomeTeam)
                .Include(f => f.VisitTeam)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (footballMatch == null)
            {
                return NotFound();
            }

            return View(footballMatch);
        }

        // GET: FootballMatches/Create
        public IActionResult Create()
        {
            ViewData["HomeTeamId"] = new SelectList(_context.Clubs, "Id", "Coach");
            ViewData["VisitTeamId"] = new SelectList(_context.Clubs, "Id", "Coach");
            return View();
        }

        // POST: FootballMatches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FootballMatch footballMatch)
        {
            if (ModelState.IsValid)
            {
                _context.Add(footballMatch);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["HomeTeamId"] = new SelectList(_context.Clubs, "Id", "Coach", footballMatch.HomeTeamId);
            ViewData["VisitTeamId"] = new SelectList(_context.Clubs, "Id", "Coach", footballMatch.VisitTeamId);
            return View(footballMatch);
        }

        // GET: FootballMatches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var footballMatch = await _context.Games.FindAsync(id);
            if (footballMatch == null)
            {
                return NotFound();
            }
            ViewData["HomeTeamId"] = new SelectList(_context.Clubs, "Id", "Coach", footballMatch.HomeTeamId);
            ViewData["VisitTeamId"] = new SelectList(_context.Clubs, "Id", "Coach", footballMatch.VisitTeamId);
            return View(footballMatch);
        }

        // POST: FootballMatches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FootballMatch footballMatch)
        {
            if (id != footballMatch.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(footballMatch);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FootballMatchExists(footballMatch.Id))
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
            ViewData["HomeTeamId"] = new SelectList(_context.Clubs, "Id", "Coach", footballMatch.HomeTeamId);
            ViewData["VisitTeamId"] = new SelectList(_context.Clubs, "Id", "Coach", footballMatch.VisitTeamId);
            return View(footballMatch);
        }

        // GET: FootballMatches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var footballMatch = await _context.Games
                .Include(f => f.HomeTeam)
                .Include(f => f.VisitTeam)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (footballMatch == null)
            {
                return NotFound();
            }

            return View(footballMatch);
        }

        // POST: FootballMatches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var footballMatch = await _context.Games.FindAsync(id);
            if (footballMatch != null)
            {
                _context.Games.Remove(footballMatch);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FootballMatchExists(int id)
        {
            return _context.Games.Any(e => e.Id == id);
        }
    }
}
