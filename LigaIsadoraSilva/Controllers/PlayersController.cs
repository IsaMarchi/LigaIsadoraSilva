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
            // Incluindo a propriedade Club para carregar o clube associado ao jogador
            // Ordenando jogadores em ordem alfabética pelo nome
            var players = await _context.Players
                .Include(p => p.Club) // Eager loading do clube
                .OrderBy(p => p.Name) // Ordena por nome
                .ToListAsync();

            return View(players);
        }

        // GET: Players/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Players
                .Include(p => p.Club) // Incluindo o clube nos detalhes
                .FirstOrDefaultAsync(m => m.Id == id);

            if (player == null)
            {
                return NotFound();
            }

            return View(player);
        }

        // GET: Players/Create
        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Players.FindAsync(id);
            if (player == null)
            {
                return NotFound();
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
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var oldPlayer = await _context.Players.FindAsync(player.Id);

                if (oldPlayer == null)
                {
                    return NotFound();
                }

                try
                {
                    if (player.ImageFile != null)
                    {
                        // Fazendo upload da nova imagem
                        string newImageUrl = await _imageHelper.UploadImageAsync(player.ImageFile, "Player");
                        oldPlayer.Photo = newImageUrl; // Atualizando a imagem no objeto rastreado
                    }

                    // Atualizando os outros campos
                    oldPlayer.Birth = player.Birth;
                    oldPlayer.Surname = player.Surname;
                    oldPlayer.Name = player.Name;
                    oldPlayer.Nationality = player.Nationality;
                    oldPlayer.ClubId = player.ClubId;

                    // Salvando as alterações no banco de dados
                    _context.Update(oldPlayer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlayerExists(player.Id))
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

            var clubes = _context.Clubs;
            ViewData["ClubId"] = new SelectList(clubes, "Id", "Name", player.ClubId);
            return View(player);
        }

        // GET: Players/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Players
                .Include(p => p.Club) // Incluindo o clube ao buscar para deletar
                .FirstOrDefaultAsync(m => m.Id == id);

            if (player == null)
            {
                return NotFound();
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
    }
}

