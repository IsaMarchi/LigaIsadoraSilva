using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LigaIsadoraSilva.Data;
using LigaIsadoraSilva.Data.Entities;
using LigaIsadoraSilva.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace LigaIsadoraSilva.Controllers
{
    public class PlayersController : Controller
    {
        private readonly DataContext _context;
        private readonly IImageHelper _imageHelper;

        public PlayersController(DataContext context, IImageHelper imageHelper)
        {
            _context = context;
            _imageHelper = imageHelper;
        }

        // GET: Players
        public async Task<IActionResult> Index()
        {
            var players = await _context.Players
                .Include(p => p.Club)
                .OrderBy(p => p.Name)
                .ToListAsync();

            // Se o usuário não estiver autenticado, fornecer dados simplificados
            if (!User.Identity.IsAuthenticated)
            {
                // Retornar apenas os dados simplificados para o usuário anônimo
                var anonymousPlayers = players.Select(p => new
                {
                    p.Name,
                    Position = p.Position, // Agora usamos a propriedade Position diretamente
                    Club = p.Club.Name
                });

                // Passar dados simplificados para a view (utilize ViewBag ou crie um view model)
                ViewBag.AnonymousPlayers = anonymousPlayers;
                return View("AnonymousIndex");
            }

            return View(players);
        }

        // GET: Players/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new PageNotFoundViewResult("PageNotFound");
            }

            var player = await _context.Players
                .Include(p => p.Club)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (player == null)
            {
                return new PageNotFoundViewResult("PageNotFound");
            }

            return View(player);
        }

        // GET: Players/Create
        [Authorize(Roles = "Team")]
        public IActionResult Create()
        {
            var clubes = _context.Clubs;
            ViewData["ClubId"] = new SelectList(clubes, "Id", "Name");
            return View();
        }

        // POST: Players/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Player player)
        {
            if (ModelState.IsValid)
            {
                if (player.ImageFile != null)
                {
                    string imageUrl = await _imageHelper.UploadImageAsync(player.ImageFile, "Player");
                    player.Photo = imageUrl;
                }
                _context.Add(player);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var clubes = _context.Clubs;
            ViewData["ClubId"] = new SelectList(clubes, "Id", "Name", player.ClubId);
            return View(player);
        }

        // GET: Players/Edit/5
        [Authorize(Roles = "Team")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new PageNotFoundViewResult("PageNotFound");
            }

            var player = await _context.Players.FindAsync(id);
            if (player == null)
            {
                return new PageNotFoundViewResult("PageNotFound");
            }

            var clubes = _context.Clubs;
            ViewData["ClubId"] = new SelectList(clubes, "Id", "Name", player.ClubId);
            return View(player);
        }

        // POST: Players/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Player player)
        {
            if (id != player.Id)
            {
                return new PageNotFoundViewResult("PageNotFound");
            }

            if (ModelState.IsValid)
            {
                var oldPlayer = await _context.Players.FindAsync(player.Id);

                if (oldPlayer == null)
                {
                    return new PageNotFoundViewResult("PageNotFound");
                }

                try
                {
                    if (player.ImageFile != null)
                    {
                        string newImageUrl = await _imageHelper.UploadImageAsync(player.ImageFile, "Player");
                        oldPlayer.Photo = newImageUrl;
                    }

                    oldPlayer.Birth = player.Birth;
                    oldPlayer.Surname = player.Surname;
                    oldPlayer.Name = player.Name;
                    oldPlayer.Nationality = player.Nationality;
                    oldPlayer.ClubId = player.ClubId;
                    oldPlayer.Position = player.Position; // Atualizando a posição

                    _context.Update(oldPlayer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlayerExists(player.Id))
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

            var clubes = _context.Clubs;
            ViewData["ClubId"] = new SelectList(clubes, "Id", "Name", player.ClubId);
            return View(player);
        }

        // GET: Players/Delete/5
        [Authorize(Roles = "Team")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new PageNotFoundViewResult("PageNotFound");
            }

            var player = await _context.Players
                .Include(p => p.Club)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (player == null)
            {
                return new PageNotFoundViewResult("PageNotFound");
            }

            return View(player);
        }

        // POST: Players/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var player = await _context.Players.FindAsync(id);
            if (player != null)
            {
                _context.Players.Remove(player);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlayerExists(int id)
        {
            return _context.Players.Any(e => e.Id == id);
        }

        public IActionResult PageNotFound()
        {
            return View();
        }
    }
}


