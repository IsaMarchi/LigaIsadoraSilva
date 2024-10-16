﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LigaIsadoraSilva.Data;
using LigaIsadoraSilva.Data.Entities;
using LigaIsadoraSilva.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace LigaIsadoraSilva.Controllers
{
    public class FootballTeamsController : Controller
    {
        private readonly DataContext _context;
        private readonly IImageHelper _imageHelper;

        public FootballTeamsController(DataContext context, IImageHelper imageHelper)
        {
            _context = context;
            _imageHelper = imageHelper;
        }

        // GET: FootballTeams
        public async Task<IActionResult> Index()
        {
            var clubs = await _context.Clubs
                .OrderBy(p => p.Name)
                .ToListAsync();
            if (!User.Identity.IsAuthenticated)
            {
                // Retornar apenas os dados simplificados para o usuário anônimo
                var anonymousPlayers = clubs.Select(p => new
                {
                    p.Name,
                    p.Coach,
                    p.Points
                });

                // Passar dados simplificados para a view (utilize ViewBag ou crie um view model)
                ViewBag.AnonymousPlayers = anonymousPlayers;
                return View("AnonymousIndex");
            }

            return View(await _context.Clubs.ToListAsync());
        }

        // GET: FootballTeams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new PageNotFoundViewResult("PageNotFound");
            }

            var footballTeam = await _context.Clubs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (footballTeam == null)
            {
                return new PageNotFoundViewResult("PageNotFound");
            }

            return View(footballTeam);
        }

        // GET: FootballTeams/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: FootballTeams/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FootballTeam footballTeam)
        {
            if (ModelState.IsValid)
            {
                if (footballTeam.ImageFile != null)
                {
                    string imageUrl = await _imageHelper.UploadImageAsync(footballTeam.ImageFile, "Club");
                    footballTeam.Photo = imageUrl;
                }
                _context.Add(footballTeam);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(footballTeam);
        }

        // GET: FootballTeams/Edit/5
        [Authorize(Roles = "Team")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new PageNotFoundViewResult("PageNotFound");
            }

            var footballTeam = await _context.Clubs.FindAsync(id);
            if (footballTeam == null)
            {
                return new PageNotFoundViewResult("PageNotFound");
            }
            return View(footballTeam);
        }

        // POST: FootballTeams/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FootballTeam footballTeam)
        {
            if (id != footballTeam.Id)
            {
                return new PageNotFoundViewResult("PageNotFound");
            }

            if (ModelState.IsValid)
            {
                var oldTeam = await _context.Clubs.FindAsync(footballTeam.Id);

                if (oldTeam == null)
                {
                    return new PageNotFoundViewResult("PageNotFound");
                }

                try
                {
                    if (footballTeam.ImageFile != null)
                    {
                        // Upload da nova imagem
                        string newImageUrl = await _imageHelper.UploadImageAsync(footballTeam.ImageFile, "Club");
                        oldTeam.Photo = newImageUrl;  // Atualizando a imagem no objeto rastreado
                    }

                    // Atualizando os outros campos
                    oldTeam.Name = footballTeam.Name;
                    oldTeam.Coach = footballTeam.Coach;
                    oldTeam.Stadium = footballTeam.Stadium;
                    oldTeam.Foundation = footballTeam.Foundation;
                    oldTeam.Points = footballTeam.Points;
                    oldTeam.MatchesPlayed = footballTeam.MatchesPlayed;

                    // Salvando as alterações no banco de dados
                    _context.Update(oldTeam);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FootballTeamExists(footballTeam.Id))
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
            return View(footballTeam);
        }

        // GET: FootballTeams/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new PageNotFoundViewResult("PageNotFound");
            }

            var footballTeam = await _context.Clubs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (footballTeam == null)
            {
                return new PageNotFoundViewResult("PageNotFound");
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

        public IActionResult PageNotFound()
        {
            return View();
        }
    }
}
