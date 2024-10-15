using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LigaIsadoraSilva.Data;
using LigaIsadoraSilva.Data.Entities;
using LigaIsadoraSilva.Data.Interface;
using LigaIsadoraSilva.Models;
using System.Numerics;
using Microsoft.AspNetCore.Authorization;
using LigaIsadoraSilva.Helpers;
using static System.Reflection.Metadata.BlobBuilder;

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
        private EditFootballMatchViewModel ConvertFootballMatchToMatchViewModel(FootballMatch match)
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
                YellowCards = match.YellowCards,
                RedCards = match.RedCards
            };
        }

        //Converte MatchesViewModel to FootballMatch
        private FootballMatch ConvertViewModelToFootballMatch(EditFootballMatchViewModel viewModel)
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
                YellowCards = viewModel.YellowCards,
                RedCards = viewModel.RedCards
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

        // GET para editar YellowCards
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> YellowCard(int? id)
        {
            if (id == null)
            {
                return new PageNotFoundViewResult("PageNotFound");
            }

            var footballMatch = await _context.Games.FindAsync(id);
            if (footballMatch == null)
            {
                return new PageNotFoundViewResult("PageNotFound");
            }

            return View(footballMatch);
        }

        //POST para salvar YellowCards
        [HttpPost]
        [ValidateAntiForgeryToken]
        //POST para salvar YellowCards
        public async Task<IActionResult> YellowCard(int id, FootballMatch footballMatch)
        {
            if (id != footballMatch.Id)
            {
                return new PageNotFoundViewResult("PageNotFound");
            }

            var matchToUpdate = await _context.Games.FindAsync(id);
            if (matchToUpdate == null)
            {
                return new PageNotFoundViewResult("PageNotFound");
            }

            if (DateTime.Now <= matchToUpdate.StartDate)
            {
                // Retornar a flag indicando que a operação foi bloqueada
                ViewBag.GameNotHappened = true;
                return View(footballMatch);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    matchToUpdate.YellowCards = footballMatch.YellowCards;
                    _context.Update(matchToUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FootballMatchExists(footballMatch.Id))
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

            return View(footballMatch);
        }



        // GET para editar RedCards
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> RedCard(int? id)
        {
            if (id == null)
            {
                return new PageNotFoundViewResult("PageNotFound");
            }

            var footballMatch = await _context.Games.FindAsync(id);
            if (footballMatch == null)
            {
                return new PageNotFoundViewResult("PageNotFound");
            }

            return View(footballMatch);
        }

        //POST para salvar RedCards
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RedCard(int id, FootballMatch footballMatch)
        {
            if (id != footballMatch.Id)
            {
                return new PageNotFoundViewResult("PageNotFound");
            }

            var matchToUpdate = await _context.Games.FindAsync(id);
            if (matchToUpdate == null)
            {
                return new PageNotFoundViewResult("PageNotFound");
            }

            // Verifica se a data atual é anterior à data de início do jogo
            if (DateTime.Now <= matchToUpdate.StartDate)
            {
                // Retornar a flag indicando que a operação foi bloqueada
                ViewBag.GameNotHappened = true;
                return View(footballMatch);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    matchToUpdate.RedCards = footballMatch.RedCards;
                    _context.Update(matchToUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FootballMatchExists(footballMatch.Id))
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

            return View(footballMatch);
        }

        // GET: FootballMatches/Resultado/5
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> Result(int? id)
        {
            if (id == null)
            {
                return new PageNotFoundViewResult("PageNotFound");
            }

            var footballMatch = await _context.Games
                .Include(f => f.HomeTeam)
                .Include(f => f.VisitTeam)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (footballMatch == null)
            {
                return new PageNotFoundViewResult("PageNotFound");
            }

            // Verifica se a data atual é anterior à data de início do jogo
            if (DateTime.Now <= footballMatch.StartDate)
            {
                // Retornar a flag indicando que a operação foi bloqueada
                ViewBag.GameNotHappened = true;
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
                return new PageNotFoundViewResult("PageNotFound");
            }

            var matchToUpdate = await _context.Games.FindAsync(id);
            if (matchToUpdate == null)
            {
                return new PageNotFoundViewResult("PageNotFound");
            }

            // Verifica se a data atual é anterior à data de início do jogo
            if (DateTime.Now <= matchToUpdate.StartDate)
            {
                // Retornar a flag indicando que a operação foi bloqueada
                ViewBag.GameNotHappened = true;
                return View(footballMatch); // Retorna para a mesma view com os dados atuais
            }

            if (ModelState.IsValid)
            {
                try
                {
                    matchToUpdate.HomeGoals = footballMatch.HomeGoals;
                    matchToUpdate.VisitGoals = footballMatch.VisitGoals;
                    matchToUpdate.YellowCards = footballMatch.YellowCards;
                    matchToUpdate.RedCards = footballMatch.RedCards;

                    _context.Update(matchToUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FootballMatchExists(footballMatch.Id))
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

            return View(footballMatch);
        }


        // GET: FootballMatches/Finalizar/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Finalise(int? id)
        {
            if (id == null)
            {
                return new PageNotFoundViewResult("PageNotFound");
            }

            var footballMatch = await _context.Games.FindAsync(id);
            if (footballMatch == null)
            {
                return new PageNotFoundViewResult("PageNotFound");
            }

            // Verifica se a data atual é anterior à data de início do jogo
            if (DateTime.Now <= footballMatch.StartDate)
            {
                ViewBag.GameNotHappened = true;
                return View("ErrorFinalise"); 
            }

            if (footballMatch.HomeGoals == null || footballMatch.VisitGoals == null)
            {
                return View("ErrorFinalise"); ;
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
                return new PageNotFoundViewResult("PageNotFound");
            }

            var footballMatch = await _context.Games
                .Include(f => f.HomeTeam)
                .Include(f => f.VisitTeam)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (footballMatch == null)
            {
                return new PageNotFoundViewResult("PageNotFound");
            }

            var viewModel = ConvertFootballMatchToMatchViewModel(footballMatch);
            return View(viewModel);
        }

        // GET: FootballMatches/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["HomeTeamId"] = new SelectList(_context.Clubs, "Id", "Name");
            ViewData["VisitTeamId"] = new SelectList(_context.Clubs, "Id", "Name");
            return View();
        }

        // POST: FootballMatches/Create
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new PageNotFoundViewResult("PageNotFound");
            }

            var footballMatch = await _context.Games.FindAsync(id);
            if (footballMatch == null)
            {
                return new PageNotFoundViewResult("PageNotFound");
            }

            ViewData["HomeTeamId"] = new SelectList(_context.Clubs, "Id", "Name", footballMatch.HomeTeamId);
            ViewData["VisitTeamId"] = new SelectList(_context.Clubs, "Id", "Name", footballMatch.VisitTeamId);

            var viewModel = ConvertFootballMatchToMatchViewModel(footballMatch);
            return View(viewModel);
        }

        // POST: FootballMatches/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditFootballMatchViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return new PageNotFoundViewResult("PageNotFound");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var footballMatch = await _context.Games.FindAsync(id);
                    if (footballMatch != null)
                    {
                        footballMatch = await ConvertViewModelToMatchEdit(viewModel, footballMatch);
                        _context.Update(footballMatch);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FootballMatchExists(viewModel.Id))
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

            ViewData["HomeTeamId"] = new SelectList(_context.Clubs, "Id", "Name", viewModel.HomeTeamId);
            ViewData["VisitTeamId"] = new SelectList(_context.Clubs, "Id", "Name", viewModel.VisitTeamId);
            return View(viewModel);
        }

        // GET: FootballMatches/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new PageNotFoundViewResult("PageNotFound");
            }

            var footballMatch = await _context.Games
                .Include(f => f.HomeTeam)
                .Include(f => f.VisitTeam)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (footballMatch == null)
            {
                return new PageNotFoundViewResult("PageNotFound");
            }

            return View(footballMatch);
        }

        // POST: FootballMatches/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> Delete(int id)
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

        public IActionResult PageNotFound()
        {
            return View();
        }
    }
}

