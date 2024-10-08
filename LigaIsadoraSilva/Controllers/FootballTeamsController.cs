﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LigaIsadoraSilva.Data;
using LigaIsadoraSilva.Data.Entities;

namespace LigaIsadoraSilva.Controllers
{
    public class FootballTeamsController : Controller
    {
        private readonly DataContext _context;

        public FootballTeamsController(DataContext context)
        {
            _context = context;
        }

        // GET: FootballTeams
        public async Task<IActionResult> Index()
        {
            return View(await _context.Clubs.ToListAsync());
        }

        // GET: FootballTeams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var footballTeam = await _context.Clubs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (footballTeam == null)
            {
                return NotFound();
            }

            return View(footballTeam);
        }

        // GET: FootballTeams/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FootballTeams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FootballTeam footballTeam)
        {
            if (ModelState.IsValid)
            {
                _context.Add(footballTeam);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(footballTeam);
        }

        // GET: FootballTeams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var footballTeam = await _context.Clubs.FindAsync(id);
            if (footballTeam == null)
            {
                return NotFound();
            }
            return View(footballTeam);
        }

        // POST: FootballTeams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FootballTeam footballTeam)
        {
            if (id != footballTeam.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(footballTeam);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FootballTeamExists(footballTeam.Id))
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
            return View(footballTeam);
        }

        // GET: FootballTeams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var footballTeam = await _context.Clubs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (footballTeam == null)
            {
                return NotFound();
            }

            return View(footballTeam);
        }

        // POST: FootballTeams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var footballTeam = await _context.Clubs.FindAsync(id);
            if (footballTeam != null)
            {
                _context.Clubs.Remove(footballTeam);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FootballTeamExists(int id)
        {
            return _context.Clubs.Any(e => e.Id == id);
        }
    }
}
