using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LigaIsadoraSilva.Data;
using LigaIsadoraSilva.Data.Entities;
using LigaIsadoraSilva.Data.Interface;
using LigaIsadoraSilva.Models;
using System.Numerics;
using Microsoft.AspNetCore.Authorization;

namespace LigaIsadoraSilva.Controllers
{
    public class FootballMatchesController : Controller
    {
        private readonly DataContext _context;
        private readonly IMatchService _matchService;

        public FootballMatchesController(DataContext context, IMatchService matchService)
        {
            _context = context;
            _matchService = matchService;
        }

        //Converte FootballMatch to ViewModel
        private EditFootballMatchViewModel ConvertFootballMatchToMatchViewModel (FootballMatch match)
        {
            return new EditFootballMatchViewModel
            {
                Id = match.Id,
                StartDate = match.StartDate,
                IsFinalized = match.IsFinalized,
                HomeTeam = match.HomeTeam,
                HomeTeamId = match.HomeTeamId,
                VisitTeam = match.VisitTeam,
                VisitTeamId = match.VisitTeamId,
                HomeGoals = match.HomeGoals,
                VisitGoals = match.VisitGoals,
            };
        }

        //Converte MatchesViewModel to FootballMatch
        private FootballMatch ConvertViewModelToFootballMatch (EditFootballMatchViewModel viewModel)
        {
            return new FootballMatch
            {
                Id = viewModel.Id,
                StartDate = viewModel.StartDate,
                IsFinalized = viewModel.IsFinalized,
                HomeTeam = viewModel.HomeTeam,
                HomeTeamId = viewModel.HomeTeamId,
                VisitTeam = viewModel.VisitTeam,
                VisitTeamId = viewModel.VisitTeamId,
                HomeGoals = viewModel.HomeGoals,
                VisitGoals = viewModel.VisitGoals,
            };
        }

        private async Task<FootballMatch> ConvertViewModelToMatchEdit(EditFootballMatchViewModel viewModel, FootballMatch footballMatch)
        {
            footballMatch.Id = viewModel.Id;
            footballMatch.StartDate = viewModel.StartDate;
            footballMatch.IsFinalized = viewModel.IsFinalized;
            footballMatch.HomeTeam = viewModel.HomeTeam;
            footballMatch.HomeTeamId = viewModel.HomeTeamId;
            footballMatch.VisitTeam = viewModel.VisitTeam;
            footballMatch.VisitTeamId = viewModel.VisitTeamId;
            footballMatch.HomeGoals = viewModel.HomeGoals;
            footballMatch.VisitGoals = viewModel.VisitGoals;

            return footballMatch;

        }

        // GET: FootballMatches
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.Games.Include(f => f.HomeTeam).Include(f => f.VisitTeam);
            return View(await dataContext.ToListAsync());
        }

        // Método para gerar e salvar partidas
        public async Task<IActionResult> GenerateMatches()
        {
            var teams = await _context.Clubs.ToListAsync(); // Obtenha os times do banco de dados
            var matches = await _matchService.GenerateMatchesAsync(teams); // Gere as partidas

            _context.Games.AddRange(matches); // Adicione as partidas à base de dados
            await _context.SaveChangesAsync(); // Salve as mudanças

            return RedirectToAction(nameof(Index)); // Redirecione para a lista de partidas
        }

        // GET: FootballMatches/Resultado/5
        public async Task<IActionResult> Result(int? id)
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

        // POST: FootballMatches/Resultado/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Result(int id, FootballMatch footballMatch)
        {
            if (id != footballMatch.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Atualizar os gols
                    var matchToUpdate = await _context.Games.FindAsync(id);
                    if (matchToUpdate != null)
                    {
                        matchToUpdate.HomeGoals = footballMatch.HomeGoals;
                        matchToUpdate.VisitGoals = footballMatch.VisitGoals;

                        _context.Update(matchToUpdate);
                        await _context.SaveChangesAsync();
                    }
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

            return View(footballMatch);
        }

        // GET: FootballMatches/Finalizar/5
        public async Task<IActionResult> Finalise(int? id)
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

            if (footballMatch.HomeGoals == null || footballMatch.VisitGoals == null)
            {
                return BadRequest("Os resultados devem ser inseridos antes de finalizar o jogo.");
            }

            footballMatch.IsFinalized = true;
            _context.Update(footballMatch);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
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

            var viewModel = ConvertFootballMatchToMatchViewModel(footballMatch);
            return View(viewModel);
        }

        // GET: FootballMatches/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["HomeTeamId"] = new SelectList(_context.Clubs, "Id", "Name");
            ViewData["VisitTeamId"] = new SelectList(_context.Clubs, "Id", "Name");
            return View();
        }

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
            ViewData["HomeTeamId"] = new SelectList(_context.Clubs, "Id", "Name", footballMatch.HomeTeamId);
            ViewData["VisitTeamId"] = new SelectList(_context.Clubs, "Id", "Name", footballMatch.VisitTeamId);
            return View(footballMatch);
        }

        // GET: FootballMatches/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var footballMatch = await _context.Games.FindAsync(id);
            if (footballMatch == null || footballMatch.IsFinalized)
            {
                return NotFound();
            }

            var viewModel = ConvertFootballMatchToMatchViewModel(footballMatch);

            ViewData["HomeTeamId"] = new SelectList(_context.Clubs, "Id", "Name", footballMatch?.HomeTeamId);
            ViewData["VisitTeamId"] = new SelectList(_context.Clubs, "Id", "Name", footballMatch?.VisitTeamId);
            return View(viewModel);
        }

        // POST: FootballMatches/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditFootballMatchViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                try
                {
                    var footballMatch = await _context.Games.FirstOrDefaultAsync(g => g.Id == id);
                    if (footballMatch == null || footballMatch.IsFinalized)
                    {
                        return NotFound();
                    }

                    // Atualiza o footballMatch com os valores do ViewModel
                    if (await TryUpdateModelAsync(footballMatch, "",
                        m => m.StartDate,
                        m => m.IsFinalized,
                        m => m.HomeTeamId,
                        m => m.VisitTeamId,
                        m => m.HomeGoals,
                        m => m.VisitGoals))
                    {
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FootballMatchExists(viewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            ViewData["HomeTeamId"] = new SelectList(_context.Clubs, "Id", "Name", viewModel.HomeTeamId);
            ViewData["VisitTeamId"] = new SelectList(_context.Clubs, "Id", "Name", viewModel.VisitTeamId);
            return View(viewModel);
        }


        //// POST: FootballMatches/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, FootballMatch footballMatch)
        //{
        //    if (id != footballMatch.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        var oldFootballMatch = await _context.Games.FindAsync(footballMatch.Id);

        //        if (oldFootballMatch == null)
        //        {
        //            return NotFound();
        //        }

        //        try
        //        {
        //            oldFootballMatch.StartDate = footballMatch.StartDate;
        //            oldFootballMatch.HomeTeamId = footballMatch.HomeTeamId;
        //            oldFootballMatch.VisitTeamId = footballMatch.VisitTeamId;

        //            // Salvando as alterações no banco de dados
        //            _context.Update(oldFootballMatch);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!FootballMatchExists(footballMatch.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }

        //        return RedirectToAction(nameof(Index));

        //    }
        //    var clubs = _context.Clubs;
        //    ViewData["HomeTeamId"] = new SelectList(clubs, "Id", "Name", footballMatch.HomeTeamId);
        //    ViewData["VisitTeamId"] = new SelectList(clubs, "Id", "Name", footballMatch.VisitTeamId);
        //    return View(footballMatch);
        //}

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

